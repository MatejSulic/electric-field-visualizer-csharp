using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPG_SP_2024
{
    public class Magnet
    {
        // Souřadnice X magnetu
        public float X;

        // Souřadnice Y magnetu
        public float Y;

        // Polarita magnetu, může být "+" nebo "-"
        public string polarity;

        // Síla magnetu
        public float force;

        // Inicializuje novou instanci třídy Magnet s danými souřadnicemi, polaritou a silou
        public Magnet(float X, float Y, string polarity, float force)
        {
            this.X = X;
            this.Y = Y;
            this.polarity = polarity;
            this.force = force;
        }
    }
}
