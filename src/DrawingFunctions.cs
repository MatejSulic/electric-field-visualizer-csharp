using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace UPG_SP_2024
{
    internal class Drawer
    {
        // Pole pro měřítko, startovní čas a panel pro kreslení
        int scale; // Určuje měřítko pro vykreslování
        long m_startTime; // Čas zahájení, může být použit pro časové operace
        Panel drawingPanel; // Panel, na kterém se bude kreslit
        CalculationsFunctions Calc = new CalculationsFunctions(); // Objekt pro výpočty

        // Konstruktor třídy Drawer
        public Drawer(int scale, long startTime, Panel drawingPanel)
        {
            this.scale = scale; // Nastavení měřítka
            m_startTime = startTime; // Uložení času zahájení
            this.drawingPanel = drawingPanel; // Uložení odkazu na panel
        }

        // Metoda pro vykreslení všech magnetů
        public void DrawMagnets(Graphics g, Magnet[] magnets)
        {
            foreach (Magnet magnet in magnets) // Pro každý magnet
            {
                DrawMagnet(g, magnet.X, magnet.Y, magnet.polarity, magnet.force); // Vykreslení jednotlivého magnetu
            }
        }

        // Metoda pro vykreslení jednoho magnetu
        public void DrawMagnet(Graphics g, float X, float Y, string polarity, float force)
        {
            var oldTR = g.Transform;
            float size_R = 30 * scale / 100 + 20 * force;    // Velikost magnetu závisí na jeho síle
            X *= scale;
            Y *= scale * -1;    // Převrácení Y osy pro správnou orientaci

            // Text magnetu se zvětšuje podle jeho síly
            float fontSize = 12 + force * 2;
            var font = new Font("Arial", fontSize, FontStyle.Bold);

            force = (float)Math.Round((double)force, 2);
            string s = polarity + force.ToString() + "C";
            SizeF size_string = g.MeasureString(s, font);

            // Vytvoření gradientu pro magnet
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(X - size_R / 2, Y - size_R / 2, size_R, size_R);
                using (var gradientBrush = new PathGradientBrush(path))
                {
                    gradientBrush.CenterColor = Color.White;
                    gradientBrush.SurroundColors = new Color[] { Color.Gray };
                    gradientBrush.FocusScales = new PointF(0.2f, 0.2f);
                    g.FillEllipse(gradientBrush, X - size_R / 2, Y - size_R / 2, size_R, size_R);
                }
            }

            // Načtení a vykreslení obrázku magnetu
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "magnet.png");
            DrawImageInCenter(g, X, Y, imagePath, size_R * 0.8f);

            // Obrys textu pro lepší čitelnost
            g.DrawString(s, font, Brushes.White, X - size_string.Width / 2 - 1, Y - size_string.Height / 2 - 1);
            g.DrawString(s, font, Brushes.White, X - size_string.Width / 2 + 1, Y - size_string.Height / 2 - 1);
            g.DrawString(s, font, Brushes.White, X - size_string.Width / 2 - 1, Y - size_string.Height / 2 + 1);
            g.DrawString(s, font, Brushes.White, X - size_string.Width / 2 + 1, Y - size_string.Height / 2 + 1);

            // Samotný text
            g.DrawString(s, font, Brushes.Black, X - size_string.Width / 2, Y - size_string.Height / 2);

            g.Transform = oldTR;
        }






        // Načte a vycentruje obrázek na dané souřadnice
        public void DrawImageInCenter(Graphics g, float X, float Y, string imagePath, float size_R)
        {
            try
            {
                using (Image img = Image.FromFile(imagePath))
                {
                    float x = X - size_R / 2;
                    float y = Y - size_R / 2;

                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.SmoothingMode = SmoothingMode.AntiAlias;

                    g.DrawImage(img, x, y, size_R, size_R);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Chyba při načítání obrázku: {ex.Message}");
            }
        }


        // Vytvoří barevnou mapu intenzity pole pro každý magnet
        public void DrawHeatMap(Graphics g, in Magnet[] magnets)
        {
            foreach (var magnet in magnets)
            {
                float size_R = 90 * scale / 100 + 20 * magnet.force;
                float x = magnet.X * scale;
                float y = magnet.Y * scale * -1;

                // Červená pro kladný, modrá pro záporný magnet
                Color startColor = magnet.polarity == "+" ? Color.Red : Color.Blue;

                var path = new GraphicsPath();
                path.AddEllipse(x - size_R, y - size_R, size_R * 2, size_R * 2);

                using (var brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = startColor;
                    brush.SurroundColors = new Color[] { Color.Transparent };
                    brush.FocusScales = new PointF(0.2f, 0.2f);
                    g.FillPath(brush, path);
                }
            }
        }


        // Vykreslí legendu s vysvětlivkami a aktuálními hodnotami
        public void DrawLegend(Graphics g, float probeSpeed, float magnetSpeed, int currentScenario)
        {
            var oldTr = g.Transform;

            // Posun legendy do levého horního rohu
            g.TranslateTransform(-drawingPanel.Width / 2 + 10, -drawingPanel.Height / 2 + 10);

            // Rozměry legendy
            int legendWidth = 160;
            int legendHeight = 140;
            int padding = 10;

            // Černé pozadí legendy
            g.FillRectangle(Brushes.Black, 0, 0, legendWidth, legendHeight);

            var font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
            g.DrawString("Simulation Legend", font, Brushes.White, padding, padding);

            // Ukázka kladné polarity
            g.FillRectangle(Brushes.Red, padding, padding + 20, 20, 20);
            g.DrawString("Positive Polarity", font, Brushes.White, padding + 30, padding + 20);

            // Ukázka záporné polarity
            g.FillRectangle(Brushes.Blue, padding, padding + 40, 20, 20);
            g.DrawString("Negative Polarity", font, Brushes.White, padding + 30, padding + 40);

            // Zobrazení aktuálních hodnot
            g.DrawString($"Probe Speed: {probeSpeed}x", font, Brushes.White, padding, padding + 70);
            g.DrawString($"Magnet Speed: {magnetSpeed}x", font, Brushes.White, padding, padding + 90);
            g.DrawString($"Scenario: {currentScenario}", font, Brushes.White, padding, padding + 110);

            g.Transform = oldTr;
        }



        // Metoda pro vykreslení souřadnicového systému
        public void DrawCoordinateSystem(Graphics g, int space_x, int space_y)
        {
            var oldTr = g.Transform; // Uložení aktuální transformace

            // Pokud nejsou definovány mezery, použije se měřítko
            if (space_x == 0) space_x = scale;
            if (space_y == 0) space_y = scale;

            int numColumns = drawingPanel.Width / space_x; // Počet sloupců
            int numRows = drawingPanel.Height / space_y; // Počet řádků

            // Vykreslení svislých čar
            for (int i = -numColumns / 2; i <= numColumns / 2; i++)
            {
                float x = i * space_x;
                g.DrawLine(Pens.Gray, x, -drawingPanel.Height / 2, x, drawingPanel.Height / 2);
            }

            // Vykreslení vodorovných čar
            for (int j = -numRows / 2; j <= numRows / 2; j++)
            {
                float y = j * space_y;
                g.DrawLine(Pens.Gray, -drawingPanel.Width / 2, y, drawingPanel.Width / 2, y);
            }

            Pen pen = new Pen(Color.White, 3); // Silnější čára pro osy

            // Vykreslení osy X
            g.DrawLine(pen, -drawingPanel.Width / 2, 0, drawingPanel.Width / 2, 0);
            g.DrawString("X", new System.Drawing.Font("Arial", 12), Brushes.White, drawingPanel.Width / 2 - 20, -20);

            // Vykreslení osy Y
            g.DrawLine(pen, 0, -drawingPanel.Height / 2, 0, drawingPanel.Height / 2);
            g.DrawString("Y", new System.Drawing.Font("Arial", 12), Brushes.White, 10, -drawingPanel.Height / 2 + 10);

            g.Transform = oldTr; // Obnovení původní transformace
        }

        // Metoda pro vykreslení mřížky s intenzitními vektory
        public void DrawGrid(Graphics g, Magnet[] magnets, int space_x, int space_y)
        {
            int a_size = 0;

            // Nastavení velikosti buněk
            if (space_x == 0)
            {
                space_x = scale;
                a_size = scale / 3;
            }
            else
            {
                a_size = Math.Min(space_x, space_y) / 2;
            }
            if (space_y == 0) space_y = scale;

            int gridWidth = drawingPanel.Width / space_x; // Šířka mřížky
            int gridHeight = drawingPanel.Height / space_y + 1; // Výška mřížky

            // Iterace přes jednotlivé buňky mřížky
            for (int i = -gridWidth / 2 - 1; i <= gridWidth / 2; i++)
            {
                for (int j = -gridHeight / 2; j <= gridHeight / 2; j++)
                {
                    float x = (i + 0.5f) * space_x; // Střed buňky na ose X
                    float y = (j + 0.5f) * space_y; // Střed buňky na ose Y

                    PointF probe = new PointF(x / scale, y / scale); // Sonda
                    PointF intensityVector = Calc.CountIntensityVector(magnets, probe); // Výpočet intenzitního vektoru

                    DrawArrowInCell(g, x, y, intensityVector, a_size); // Vykreslení šipky v buňce
                }
            }
        }

        // Metoda pro vykreslení šipky intenzitního vektoru
        public void DrawArrowInCell(Graphics g, float cellCenterX, float cellCenterY, PointF vector, int a_size)
        {
            var oldTr = g.Transform; // Uložení aktuální transformace

            g.TranslateTransform(cellCenterX, -cellCenterY); // Posunutí do středu buňky

            vector = Calc.NormalizeVector(vector); // Normalizace vektoru
            var pen = new Pen(Color.Brown, 2); // Pero pro šipku
            var arrowCap = new System.Drawing.Drawing2D.AdjustableArrowCap(4, 6); // Konec šipky
            pen.CustomEndCap = arrowCap;

            float arrowLength = a_size; // Délka šipky
            g.DrawLine(pen, 0, 0, vector.X * arrowLength, -vector.Y * arrowLength); // Vykreslení šipky

            g.Transform = oldTr; // Obnovení původní transformace
        }



        public void DrawVector(Graphics g, PointF start, PointF end, double description, int c)
        {
            // Převod a zaokrouhlení hodnoty popisu
            description = description / 1000000; // Převod na MN/C (miliony newtonů na coulomb)
            description = Math.Round(description, 2); // Zaokrouhlení na 2 desetinná místa

            var oldTr = g.Transform; // Uložení aktuální transformace

            // Posunutí výchozího bodu do počátku
            g.TranslateTransform(start.X * scale, -start.Y * scale);

            // Nastavení pera podle parametru `c`
            Pen pen = new Pen(Color.Orange, 2);
            if (c == 1)
            {
                pen = new Pen(Color.White, 2); // Bílé pero, např. pro jiný typ vektoru
            }
            else
            {
                pen = new Pen(Color.Red, 2); // Červené pero, např. pro intenzitu pole
            }

            // Nastavení šipky na konci čáry
            var arrowCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5);
            pen.CustomEndCap = arrowCap;

            // Normalizace vektoru koncového bodu
            end = Calc.NormalizeVector(end);

            // Vykreslení čáry mezi výchozím bodem (0, 0) a koncovým bodem
            g.DrawLine(pen, 0, 0, end.X * scale, -end.Y * scale);

            // Vykreslení textového popisu vektoru
            var font = new System.Drawing.Font("Arial", 12);
            SizeF size = g.MeasureString(description.ToString() + "MN/C", font);
            g.DrawString(description.ToString() + "MN/C", font, Brushes.White, 10, 10);

            // Obnovení původní transformace
            g.Transform = oldTr;
        }

        public PointF DrawCircularProbe(Graphics g, float speed)
        {
            var old_tr = g.Transform; // Uložení aktuální transformace
            float path_r = 1 * scale; // Poloměr kruhové dráhy
            float probe_size = 24 * scale / 100; // Velikost sondy

            // Výpočet uplynulého času od začátku animace
            float elapsed = (Environment.TickCount - (int)m_startTime) / 1000f * speed;

            // Výpočet rychlosti v radiánech a stupních
            double radian_speed = Math.PI / 6 ; // 30° za sekundu
            double degrees_speed = (Math.PI / 6) * (180 / Math.PI); // Převod na stupně

            // Výpočet pozice sondy na kruhové dráze
            float x = path_r * (float)Math.Cos(radian_speed * elapsed);
            float y = path_r * (float)Math.Sin(radian_speed * elapsed);

            // Rotace grafiky podle času
            g.RotateTransform((float)degrees_speed * elapsed);

            // Vykreslení sondy jako elipsy
            g.FillEllipse(Brushes.White, path_r - probe_size / 2, -probe_size / 2, probe_size, probe_size);

            g.Transform = old_tr; // Obnovení původní transformace

            // Vrácení aktuální pozice sondy
            return new PointF(x / scale, -y / scale);
        }

        public PointF DrawStaticProbe(Graphics g, float x, float y, float probe_size)
        {
            // Převod pozice podle měřítka
            x *= scale;
            y *= scale * -1;

            // Vykreslení sondy jako červené elipsy
            g.FillEllipse(Brushes.Red, x - (probe_size / 2), y - (probe_size / 2), probe_size, probe_size);

            // Vrácení aktuální pozice sondy
            return new PointF(x / scale, -y / scale);
        }

        public void DrawCoordinates(Graphics g, PointF point)
        {
            var olr_tr = g.Transform; // Uložení aktuální transformace

            // Posunutí grafiky do levého horního rohu
            g.TranslateTransform(-drawingPanel.Width / 2, -drawingPanel.Height / 2);

            // Text s pozicí sondy
            var font = new System.Drawing.Font("Arial", 12);
            string position_string = "Probe position:\n" + "X: " + point.X.ToString() + "\nY: " + point.Y.ToString();

            // Vykreslení černého pozadí pro text
            g.FillRectangle(Brushes.Black, 0, 0, 140, 60);

            // Vykreslení textu
            g.DrawString(position_string, font, Brushes.White, 0, 0);

            g.Transform = olr_tr; // Obnovení původní transformace
        }

       
    }
}
