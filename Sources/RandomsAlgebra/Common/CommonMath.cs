using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomAlgebra
{
    internal static class CommonMath
    {
        public static double InterpolateLinear(double x0, double x1, double y0, double y1, double x)
        {
            return y0 + (y1 - y0) / (x1 - x0) * (x - x0);
        }

        public static double[] GenerateXAxis(double r1, double r2, int samples, out double step)
        {
            double min = Math.Min(r1, r2);
            double max = Math.Max(r1, r2);

            double[] result = new double[samples];

            step = (max - min) / (samples - 1);

            for (int i = 0; i < samples; i++)
            {
                result[i] = min + i * step;
            }

            return result;

        }

        public static double[] Resample(double[] array, int newLength)
        {
            int oldLength = array.Length;

            double[] newArray = new double[newLength];
            double scale = (double)(oldLength - 1) / (newLength - 1);


            newArray[0] = array[0];

            for (int i = 1; i < newLength; i++)
            {
                double x = i * scale;
                int x0 = (int)(i * scale);
                int x1 = x0 + 1;

                if (x1 >= oldLength - 1)
                {
                    newArray[i] = array[oldLength - 1];
                }
                else
                {
                    double y0 = array[x0];
                    double y1 = array[x1];
                    newArray[i] = InterpolateLinear(x0, x1, y0, y1, x);
                }
            }

            return newArray;
        }
    }
}
