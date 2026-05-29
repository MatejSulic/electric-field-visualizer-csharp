using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UPG_SP_2024
{
    internal class MagnetLogic
    {
        // Metoda pro změnu síly magnetů
        public void ChangeMagnetsForce(Magnet[] magnets, float speed_magnet)
        {
            float t = Environment.TickCount / 1000f * speed_magnet; // Získání aktuálního času v sekundách a zohlednění rychlosti magnetu
            float q1 = 1 + 0.5f * (float)Math.Sin(Math.PI / 2 * t); // Výpočet nové síly pro první magnet
            float q2 = 1 - 0.5f * (float)Math.Sin(Math.PI / 2 * t); // Výpočet nové síly pro druhý magnet

            // Nastavení nové síly pro magnety
            magnets[0].force = q1;
            magnets[1].force = q2;
        }

        // Metoda pro výběr scénáře a inicializaci magnetů
        public Magnet[] ChooseScenario(int scenario, Boolean DynamicMagnets, MainForm main)
        {
            Magnet magnet1 = new Magnet(0, 0, "", 0);
            Magnet magnet2 = new Magnet(0, 0, "", 0);
            Magnet magnet3 = new Magnet(0, 0, "", 0);
            Magnet magnet4 = new Magnet(0, 0, "", 0);
            switch (scenario.ToString())
            {
                case "0":
                    magnet1.polarity = "+";
                    magnet1.force = 1;
                    return new Magnet[] { magnet1 };
                case "1":
                    magnet1.X = -1;
                    magnet1.Y = 0;
                    magnet1.polarity = "+";
                    magnet1.force = 1;

                    magnet2.X = 1;
                    magnet2.Y = 0;
                    magnet2.polarity = "+";
                    magnet2.force = 1;
                    return new Magnet[] { magnet1, magnet2 };
                case "2":
                    magnet1.X = -1;
                    magnet1.Y = 0;
                    magnet1.polarity = "-";
                    magnet1.force = 1;

                    magnet2.X = 1;
                    magnet2.Y = 0;
                    magnet2.polarity = "+";
                    magnet2.force = 2;

                    return new Magnet[] { magnet1, magnet2 };
                case "3":
                    magnet1.X = -1;
                    magnet1.Y = -1;
                    magnet1.polarity = "+";
                    magnet1.force = 1;

                    magnet2.X = 1;
                    magnet2.Y = -1;
                    magnet2.polarity = "+";
                    magnet2.force = 2;

                    magnet3.X = 1;
                    magnet3.Y = 1;
                    magnet3.polarity = "-";
                    magnet3.force = 3;

                    magnet4.X = -1;
                    magnet4.Y = 1;
                    magnet4.polarity = "-";
                    magnet4.force = 4;

                    return new Magnet[] { magnet1, magnet2, magnet3, magnet4 };

                case "4":
                    main.DynamicMagnets = true;
                    magnet1.X = -1;
                    magnet1.Y = 0;
                    magnet1.polarity = "+";
                    magnet1.force = 1;

                    magnet2.X = 1;
                    magnet2.Y = 0;
                    magnet2.polarity = "+";
                    magnet2.force = 1;
                    return new Magnet[] { magnet1, magnet2 };

                default:
                    magnet1.polarity = "+";
                    magnet1.force = 1;

                    return new Magnet[] { magnet1 };
            }
        }
    }
}
