using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace UPG_SP_2024
{
    internal static class Program
    {
        /// <summary>
        ///  Hlavní vstupní bod aplikace.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Regulární výraz pro kontrolu, zda řetězec obsahuje pouze číslice
            Regex regex = new Regex("^[0-9]+$");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Kontrola počtu argumentů předaných aplikaci
            if (args.Length == 0)
            {
                // Žádné argumenty, spuštění aplikace s výchozími parametry
                Application.Run(new MainForm(0, 0, 0));
            }
            else if (args.Length == 1)
            {
                // Jeden argument, kontrola, zda je to platné celé číslo mezi 0 a 4
                if (int.TryParse(args[0], out int param) && param >= 0 && param <= 4)
                {
                    Application.Run(new MainForm(param, 0, 0));
                }
                else
                {
                    // Zobrazení chybové zprávy, pokud parametr není platný
                    MessageBox.Show("Parametr musí být mezi 0 a 4", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (args.Length == 2)
            {
                // Dva argumenty, kontrola, zda je první z nich číslo
                if (regex.IsMatch(args[0]))
                {
                    string input = args[1];
                    input = input.Substring(2); // Odstranění prvních dvou znaků

                    // Rozdělení druhého argumentu na části X a Y
                    string[] parts = input.Split('x');
                    if (parts.Length == 2 && int.TryParse(parts[0], out int x_s) && int.TryParse(parts[1], out int y_s))
                    {
                        // Spuštění aplikace s parsovanými parametry
                        Application.Run(new MainForm(int.Parse(args[0]), x_s, y_s));
                    }
                    else
                    {
                        // Zobrazení chybové zprávy, pokud druhý parametr není ve správném formátu
                        MessageBox.Show("Druhý parametr musí být ve formátu XxY", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Zobrazení chybové zprávy, pokud první parametr není číslo
                    MessageBox.Show("První parametr musí být číslo", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // Zobrazení chybové zprávy, pokud je příliš mnoho parametrů
                MessageBox.Show("Příliš mnoho parametrů", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
