using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UPG_SP_2024
{
    internal class CalculationsFunctions
    {
        // Metoda pro výpočet intenzitního vektoru v daném bodě (probe) na základě pole magnetů
        public PointF CountIntensityVector(Magnet[] magnets, PointF probe)
        {
            PointF[] vectors = new PointF[magnets.Length];

            // Pro každý magnet v poli magnetů
            for (int i = 0; i <= magnets.Length - 1; i++)
            {
                Magnet magnet = magnets[i];
                double distance = CountDistance(magnet, probe); // Výpočet vzdálenosti mezi magnetem a sondou
                PointF vector = CountDirectionVector(magnet, probe); // Výpočet směrového vektoru mezi magnetem a sondou

                // Výpočet intenzitního vektoru na základě síly magnetu a vzdálenosti
                PointF intesity_vector = new PointF(magnet.force * (vector.X / (float)Math.Pow(distance, 3)), magnet.force * (vector.Y / (float)Math.Pow(distance, 3)));
                vectors[i] = intesity_vector;
            }

            // Součet všech intenzitních vektorů
            PointF Intensity_vector_sum = VectorsSum(vectors);
            float ix = Intensity_vector_sum.X;
            float iy = Intensity_vector_sum.Y;

            return Intensity_vector_sum;
        }

        // Metoda pro výpočet vzdálenosti mezi magnetem a sondou
        public double CountDistance(Magnet magnet, PointF probe)
        {
            double distance = Math.Sqrt(Math.Pow(magnet.X - probe.X, 2) + Math.Pow(magnet.Y - probe.Y, 2));
            return distance;
        }

        // Metoda pro výpočet směrového vektoru mezi magnetem a sondou
        public PointF CountDirectionVector(Magnet magnet, PointF probe)
        {
            double distance = CountDistance(magnet, probe);

            PointF vector = new PointF(0, 0);

            // Podmínka pro určení směru vektoru na základě polarity magnetu
            if (magnet.polarity == "+")
            {
                vector.X = (float)((probe.X - magnet.X) / distance);
                vector.Y = (float)((probe.Y - magnet.Y) / distance);
            }
            else
            {
                vector.X = (float)((magnet.X - probe.X) / distance);
                vector.Y = (float)((magnet.Y - probe.Y) / distance);
            }

            return vector;
        }

        // Metoda pro součet pole vektorů
        public PointF VectorsSum(PointF[] vectors)
        {
            PointF vector_sum = new PointF(0, 0);
            foreach (PointF vector in vectors)
            {
                vector_sum.X += vector.X;
                vector_sum.Y += vector.Y;
            }
            return vector_sum;
        }

        // Metoda pro normalizaci vektoru
        public PointF NormalizeVector(PointF vector)
        {
            double vector_len = Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
            PointF vector_normalized = new PointF((float)(vector.X / vector_len), (float)(vector.Y / vector_len));
            return vector_normalized;
        }

        // Metoda pro výpočet intenzity elektrického pole na základě intenzitního vektoru
        public double CountIntensity(PointF IntensityVector)
        {
            double e0 = 8.854e-12;
            double q = 1 / (4 * (float)Math.PI * e0);
            double intensity = q * CountDistance(new Magnet(0, 0, "", 0), IntensityVector);

            return intensity;
        }
    }
}
