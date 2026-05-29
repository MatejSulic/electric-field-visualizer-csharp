using ElectricFieldVis;
using HarfBuzzSharp;
using System.DirectoryServices;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Svg;
using Svg.Transforms;

namespace UPG_SP_2024
{
    public partial class MainForm : Form
    {
        float scale; // Měřítko pro vykreslování
        public bool DynamicMagnets; // Určuje, zda jsou magnety dynamické
        int scenario; // Scénář pro zobrazení magnetů
        public int test_scenario; // Testovací scénář
        public long m_startTime; // Počáteční čas pro měření

        public bool StaticProbe; // Určuje, zda je sonda statická
        float static_probe_x; // X-ová pozice statické sondy
        float static_probe_y; // Y-ová pozice statické sondy
        float static_probe_size; // Velikost statické sondy

        private List<float> intensityValues = new List<float>(); // Seznam hodnot intenzity
        private List<int> timeValues = new List<int>(); // Seznam hodnot času
        public ChartForm? chartForm; // Formulář pro zobrazení grafu
        private int elapsedTimeCounter = 0; // Počítadlo pro sledování uplynulého času

        public Magnet[] magnets; // Pole magnetů
        bool isDragging = false; // Určuje, zda je magnet táhnut myší
        private PointF mouseOffset; // Posun myši při táhnutí magnetu

        private Magnet? selectedMagnet; // Aktuálně vybraný magnet
        private Magnet? lastClickedMagnet;

        public int spacing_x; // Rozestup mřížky v ose X
        public int spacing_y; // Rozestup mřížky v ose Y

        public float simulation_speed;        // Aktuální rychlost pohybu kruhové sondy (0.5x, 1x, 2x)
        public float simulation_speed_before; // Předchozí rychlost pro cyklování mezi hodnotami
        public float magnet_speed;           // Aktuální rychlost změny síly dynamických magnetů
        public float magnet_speed_before;    // Předchozí rychlost pro cyklování mezi hodnotami

        public MainForm(int scenario, int spacing_x, int spacing_y)
        {
            

            this.scenario = scenario;
            this.spacing_x = spacing_x;
            this.spacing_y = spacing_y;
            this.simulation_speed = 1;
            this.simulation_speed_before = 1;

            this.magnet_speed = 1;
            this.magnet_speed_before = 1;

            // Nastavení panelu
            InitializeComponent();
            this.MinimumSize = new Size(800, 600);
            this.drawingPanel.BackColor = Color.Orange;
            this.drawingPanel.MouseWheel += drawingPanel_MouseWheel!;
            EnableDoubleBuffering(drawingPanel);

            // Měřítko pro vykreslování
            scale = Math.Min(this.drawingPanel.Width, this.drawingPanel.Width) / 8;

            // Časovač pro animaci
            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 50;
            timer.Tick += Timer_Tick!;
            m_startTime = Environment.TickCount;
            timer.Start();

            DynamicMagnets = false; // Nastavení dynamických magnetů na false
            MagnetLogic MagnetLogic = new MagnetLogic();
            magnets = MagnetLogic.ChooseScenario(this.scenario, DynamicMagnets, this);
        }

        private void drawingPanel_Resize(object sender, EventArgs e)
        {
            drawingPanel.Invalidate(); // Obnoví vykreslování panelu při změně velikosti
        }

        private void drawingPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            g.TranslateTransform(drawingPanel.Width / 2, drawingPanel.Height / 2);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Drawer Drawer = new Drawer((int)scale, m_startTime, drawingPanel);
            CalculationsFunctions Calc = new CalculationsFunctions();
            MagnetLogic MagnetLogic = new MagnetLogic();

            Drawer.DrawCoordinateSystem(g, spacing_x, spacing_y);

            if (DynamicMagnets)
            {
                MagnetLogic.ChangeMagnetsForce(magnets, magnet_speed);
            }

            Drawer.DrawHeatMap(g, magnets);

            Drawer.DrawMagnets(g, magnets);

            if (StaticProbe)
            {
                PointF probe2 = Drawer.DrawStaticProbe(g, static_probe_x, static_probe_y, static_probe_size);
                PointF vector2 = Calc.CountIntensityVector(magnets, probe2);
                Drawer.DrawVector(g, probe2, vector2, Calc.CountIntensity(vector2), 0);
            }

            Drawer.DrawGrid(g, magnets, spacing_x, spacing_y);
            PointF probe = Drawer.DrawCircularProbe(g, this.simulation_speed);
            PointF vector = Calc.CountIntensityVector(magnets, probe);
            Drawer.DrawVector(g, probe, vector, Calc.CountIntensity(vector), 1);
            Drawer.DrawLegend(g, this.simulation_speed, this.magnet_speed, scenario);
        }

        public void EnableDoubleBuffering(Control control)
        {
            var doubleBufferPropertyInfo = control.GetType().GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferPropertyInfo?.SetValue(control, true, null); // Povolení dvojitého bufferování pro plynulejší vykreslování
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTimeCounter += 50; // Zvýšení počítadla času

            if (elapsedTimeCounter >= 1000)
            {
                elapsedTimeCounter = 0; // Resetování počítadla času

                if (StaticProbe)
                {
                    CalculationsFunctions Calc = new CalculationsFunctions();
                    MagnetLogic MagnetLogic = new MagnetLogic();

                    if (DynamicMagnets)
                    {
                        MagnetLogic.ChangeMagnetsForce(magnets, magnet_speed); // Změní sílu magnetů, pokud jsou dynamické
                    }

                    PointF probe2 = new PointF(static_probe_x, static_probe_y);
                    PointF vector2 = Calc.CountIntensityVector(magnets, probe2);
                    double intensity = Calc.CountIntensity(vector2);

                    intensityValues.Add((float)intensity); // Přidání hodnoty intenzity do seznamu
                    int elapsed = (int)((Environment.TickCount - m_startTime) / 1000);
                    timeValues.Add(elapsed); // Přidání hodnoty času do seznamu

                    if (chartForm != null)
                    {
                        chartForm.UpdateChart(intensityValues, timeValues); // Aktualizace grafu
                    }
                }
            }

            this.drawingPanel.Invalidate(); // Obnoví vykreslování panelu
        }


        // Zpracovává kliknutí myší pro umístění statické měřící sondy a vytvoření grafu intenzity
        private void drawingPanel_MouseClick(object sender, MouseEventArgs e)
        {
            // Kontrola, zda se netáhne magnet - sonda se umístí pouze při prostém kliknutí
            if (!isDragging)
            {
                this.StaticProbe = true;

                this.static_probe_x = (e.X - drawingPanel.Width / 2) / scale;

                // Pro Y souřadnici odečítáme od středu, protože osa Y je v grafice převrácená
                this.static_probe_y = (drawingPanel.Height / 2 - e.Y) / scale;

                // Nastavení velikosti sondy v závislosti na měřítku
                this.static_probe_size = 15 * scale / 100;

                // Vyčištění starých dat měření
                intensityValues.Clear();   
                timeValues.Clear();       
                m_startTime = Environment.TickCount;   // Nový počáteční čas měření

                // Odstranění případného starého grafu
                if (chartForm != null)
                {
                    chartForm.Close();
                }

                // Vytvoření a zobrazení nového grafu pro měření
                chartForm = new ChartForm(intensityValues, timeValues);
                chartForm.Show();
            }
        }





        //Zachytí stisknutí tlačítka myši a začne táhnout magnet, pokud je kurzor nad ním
        private void drawingPanel_MouseDown(object sender, MouseEventArgs e)
        {
            // Převod souřadnic myši na relativní pozici vůči středu panelu
            PointF mousePosition = new PointF(e.X - drawingPanel.Width / 2, e.Y - drawingPanel.Height / 2);

            foreach (Magnet magnet in magnets)
            {
                // Výpočet pozice magnetu v pixelech (Y je převrácený kvůli souřadnicovému systému)
                PointF magnetPosition = new PointF(magnet.X * scale, magnet.Y * scale * -1);

                // Kontrola, zda je kurzor nad magnetem
                if (checkIfMouseIsOverMagnet(mousePosition, magnet))
                {
                    selectedMagnet = magnet;           // Uložení vybraného magnetu pro táhnutí
                    lastClickedMagnet = magnet;        // Zapamatování posledního kliknutého magnetu pro možnost smazání

                    // Výpočet offsetu mezi pozicí myši a magnetu pro plynulé táhnutí
                    mouseOffset = new PointF(mousePosition.X - magnetPosition.X, mousePosition.Y - magnetPosition.Y);

                    isDragging = true;     // Označení začátku táhnutí
                    break;                 // Ukončení hledání - našli jsme magnet
                }
            }
        }


        // Kontroluje, zda je kurzor myši nad magnetem
        bool checkIfMouseIsOverMagnet(PointF mousePosition, Magnet magnet)
        {
            // Výpočet poloměru magnetu - závisí na měřítku a síle magnetu
            float radius = 30 * scale / 100 + 20 * magnet.force;

            // Převod pozice magnetu na pixely
            PointF magnetPosition = new PointF(magnet.X * scale, magnet.Y * scale * -1);

            // Výpočet vzdálenosti mezi kurzorem a magnetem pomocí Pythagorovy věty
            float distance = (float)Math.Sqrt(
                Math.Pow(mousePosition.X - magnetPosition.X, 2) +
                Math.Pow(mousePosition.Y - magnetPosition.Y, 2));

            // Vrací true, pokud je vzdálenost menší nebo rovna poloměru magnetu
            return distance <= radius;
        }

        private void drawingPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && selectedMagnet != null)
            {
                PointF mousePosition = new PointF(e.X - drawingPanel.Width / 2, e.Y - drawingPanel.Height / 2);
                selectedMagnet.X = (mousePosition.X - mouseOffset.X) / scale;
                selectedMagnet.Y = (mousePosition.Y - mouseOffset.Y) / scale * -1;
                drawingPanel.Invalidate(); // Obnoví vykreslování panelu
            }
        }

        private void drawingPanel_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false; // Ukončení táhnutí
            selectedMagnet = null!; // Zrušení vybraného magnetu
        }

       
        // Funkce pro změnu síly magnetu pomocí kolečka myši
        private void drawingPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            // Výpočet relativní pozice myši vzhledem ke středu panelu
            PointF mousePosition = new PointF(e.X - drawingPanel.Width / 2, e.Y - drawingPanel.Height / 2);

            // Projít všechny magnety a najít ten, nad kterým je kurzor
            foreach (Magnet magnet in magnets)
            {
                // Pokud je kurzor nad magnetem
                if (checkIfMouseIsOverMagnet(mousePosition, magnet))
                {
                    // Výpočet změny síly z hodnoty otočení kolečka myši
                    // Násobení 0.5f zpomaluje změnu síly pro jemnější ovládání
                    float delta = e.Delta / 120 * 0.5f;

                    // Pro zápornou polaritu otočíme směr změny síly
                    if (magnet.polarity == "-")
                    {
                        delta *= -1;
                    }

                    // Výpočet nové síly přičtením změny
                    float newForce = magnet.force + delta;

                    // Pokud síla klesne pod minimum (0.5), přepólujeme magnet
                    // a nastavíme minimální sílu, aby magnety nezmizely úplně
                    if (newForce < 0.5f)
                    {
                        // Změna polarity z + na - nebo naopak
                        magnet.polarity = (magnet.polarity == "+") ? "-" : "+";
                        // Nastavení minimální povolené síly
                        newForce = 0.5f;
                    }

                    // Uložení nové síly do magnetu
                    magnet.force = newForce;
                    // Překreslení panelu pro zobrazení změn
                    drawingPanel.Invalidate();
                    // Ukončení cyklu, protože jsme našli správný magnet
                    break;
                }
            }
        }
    


        private void button_speed_Click(object sender, EventArgs e)
        {
            float previousSpeed = simulation_speed;
            intensityValues.Clear(); // Vymazání seznamu hodnot intenzity
            timeValues.Clear(); // Vymazání seznamu hodnot času

            if (simulation_speed == 1)
            {
                if (simulation_speed_before == 2)
                {
                    simulation_speed = 0.5f; // Zpomalení
                    simulation_speed_before = 0.5f;
                }
                else
                {
                    simulation_speed = 2; // Zrychlení
                    simulation_speed_before = 2;
                }
            }
            else
            {
                simulation_speed = 1; // Návrat do normálu
            }

            // Reset elapsed time
            elapsedTimeCounter = 0;
            m_startTime = Environment.TickCount;
        }

        private void button_speed_magnet_Click(object sender, EventArgs e)
        {
            float previousSpeed = magnet_speed;
            intensityValues.Clear(); // Vymazání seznamu hodnot intenzity
            timeValues.Clear(); // Vymazání seznamu hodnot času

            if (magnet_speed == 1)
            {
                if (magnet_speed_before == 2)
                {
                    magnet_speed = 0.5f; // Zpomalení
                    magnet_speed_before = 0.5f;
                }
                else
                {
                    magnet_speed = 2; // Zrychlení
                    magnet_speed_before = 2;
                }
            }
            else
            {
                magnet_speed = 1; // Návrat do normálu
            }

            // Reset elapsed time
            elapsedTimeCounter = 0;
            m_startTime = Environment.TickCount;
        }


       
        // Funkce pro export aktuálního zobrazení do SVG souboru
        private void exportSvgButton_Click(object sender, EventArgs e)
        {
            // Vytvoření dialogu pro uložení souboru s nastavením pro SVG formát
            using (var saveFileDialog = new SaveFileDialog
            {
                Filter = "SVG Files (*.svg)|*.svg",
                DefaultExt = "svg",
                AddExtension = true
            })
            {
                // Pokud uživatel vybral umístění a potvrdil uložení
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Výpočet celkových rozměrů SVG dokumentu
                        // Zahrnuje hlavní panel a případně i graf
                        var totalWidth = drawingPanel.Width;
                        var totalHeight = drawingPanel.Height;

                        // Přičtení šířky grafu, pokud existuje
                        if (chartForm != null)
                        {
                            totalWidth += chartForm.CartesianChart.Width;
                            totalHeight = Math.Max(totalHeight, chartForm.CartesianChart.Height);
                        }

                        // Vytvoření nového SVG dokumentu s vypočtenými rozměry
                        var svgDocument = new SvgDocument
                        {
                            Width = new SvgUnit(totalWidth),
                            Height = new SvgUnit(totalHeight)
                        };

                        // Export hlavního panelu simulace
                        using (var mainBitmap = new Bitmap(drawingPanel.Width, drawingPanel.Height))
                        {
                            // Převod obsahu panelu do bitmapy
                            drawingPanel.DrawToBitmap(mainBitmap, drawingPanel.ClientRectangle);

                            // Procházení všech pixelů bitmapy
                            for (int y = 0; y < mainBitmap.Height; y++)
                            {
                                for (int x = 0; x < mainBitmap.Width; x++)
                                {
                                    var color = mainBitmap.GetPixel(x, y);
                                    // Přidání pouze viditelných pixelů (alpha > 0)
                                    if (color.A > 0)
                                    {
                                        // Vytvoření SVG obdélníku pro každý pixel
                                        var rect = new Svg.SvgRectangle
                                        {
                                            X = x,
                                            Y = y,
                                            Width = 1,
                                            Height = 1,
                                            Fill = new Svg.SvgColourServer(color)
                                        };
                                        svgDocument.Children.Add(rect);
                                    }
                                }
                            }
                        }

                        // Export grafu, pokud je zobrazen
                        if (chartForm?.CartesianChart != null)
                        {
                            using (var chartBitmap = new Bitmap(chartForm.CartesianChart.Width, chartForm.CartesianChart.Height))
                            {
                                // Převod grafu do bitmapy
                                chartForm.CartesianChart.DrawToBitmap(chartBitmap, chartForm.CartesianChart.ClientRectangle);

                                // Vytvoření skupiny pro graf s posunutím vedle hlavního panelu
                                var chartGroup = new SvgGroup
                                {
                                    Transforms = new SvgTransformCollection
                           {
                               new SvgTranslate(drawingPanel.Width, 0)
                           }
                                };

                                // Procházení pixelů grafu stejným způsobem jako hlavní panel
                                for (int y = 0; y < chartBitmap.Height; y++)
                                {
                                    for (int x = 0; x < chartBitmap.Width; x++)
                                    {
                                        var color = chartBitmap.GetPixel(x, y);
                                        if (color.A > 0)
                                        {
                                            var rect = new Svg.SvgRectangle
                                            {
                                                X = x,
                                                Y = y,
                                                Width = 1,
                                                Height = 1,
                                                Fill = new Svg.SvgColourServer(color)
                                            };
                                            chartGroup.Children.Add(rect);
                                        }
                                    }
                                }

                                // Přidání skupiny grafu do SVG dokumentu
                                svgDocument.Children.Add(chartGroup);
                            }
                        }

                        // Uložení SVG dokumentu do vybraného souboru
                        svgDocument.Write(saveFileDialog.FileName);
                        MessageBox.Show("Export SVG byl úspěšný!", "Úspěch", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Chyba při exportu: {ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        //Metoda pro změnu scénáře pomocí ComboBoxu
        private void ScenarioComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Získá index vybraného scénáře (přičte 1, protože indexy začínají od 0)
            int index = ScenarioComboBox.SelectedIndex + 1;
            MessageBox.Show("Scenario changed to: " + index);
            this.scenario = index;
            this.DynamicMagnets = false;

            // Znovu inicializuje magnety podle nového scénáře
            MagnetLogic magnetLogic = new MagnetLogic();
            this.magnets = magnetLogic.ChooseScenario(this.scenario, DynamicMagnets, this);

            // Resetuje všechny potřebné vlastnosti
            this.StaticProbe = false;
            this.intensityValues.Clear();
            this.timeValues.Clear();
            this.m_startTime = Environment.TickCount;

            // Vyvolá překreslení panelu
            drawingPanel.Invalidate();
        }

        //Metoda pro přidání magnetu doprostřed souřadného systému
        private void add_magnet_Click(object sender, EventArgs e)
        {
            // Vytvoří nový magnet s kladnou polaritou a silou 1 na souřadnicích (0,0)
            Magnet newMagnet = new Magnet(0, 0, "+", 1);

            // Přidá nový magnet do pole magnetů
            List<Magnet> magnetList = new List<Magnet>(magnets);
            magnetList.Add(newMagnet);
            magnets = magnetList.ToArray();

            // Vyvolá překreslení panelu
            drawingPanel.Invalidate();
        }

        //Metoda pro odstranění magnetu
        private void remove_magnet_Click(object sender, EventArgs e)
        {
            // Pokud je vybrán magnet a není poslední, odstraní ho
            if (lastClickedMagnet != null && magnets.Length > 1)
            {
                magnets = magnets.Where(m => m != lastClickedMagnet).ToArray();
                lastClickedMagnet = null;
                drawingPanel.Invalidate();
            }
            // Pokud je to poslední magnet, zobrazí varování
            else if (magnets.Length <= 1)
            {
                MessageBox.Show("Nelze odstranit poslední magnet!");
            }
            // Pokud není vybrán žádný magnet
            else
            {
                MessageBox.Show("Nejdříve vyberte magnet!");
            }
        }

        //Metoda pro vrácení do původního stavu stsknutím tlačítka
        private void back_default_Click(object sender, EventArgs e)
        {
            // Vypne dynamické magnety
            this.DynamicMagnets = false;
            MagnetLogic magnetLogic = new MagnetLogic();

            // Obnoví magnety do původního stavu aktuálního scénáře
            this.magnets = magnetLogic.ChooseScenario(this.scenario, DynamicMagnets, this);

            // Resetuje všechny potřebné vlastnosti
            this.StaticProbe = false;
            this.intensityValues.Clear();
            this.timeValues.Clear();
            this.m_startTime = Environment.TickCount;
        }
    }


}

