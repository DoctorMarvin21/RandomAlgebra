using System;
using System.Linq;

namespace RandomAlgebra.Distributions
{
    internal static class CommonRandomMath
    {
        public static DiscreteDistribution Divide(double value, BaseDistribution dpdf)
        {
            return Operation(dpdf, value, DistributionsOperation.DivideInv);
        }

        public static BaseDistribution Power(BaseDistribution dpdf, double value)
        {
            return Operation(dpdf, value, DistributionsOperation.Power);
        }

        public static DiscreteDistribution Power(double value, BaseDistribution dpdf)
        {
            return Operation(dpdf, value, DistributionsOperation.PowerInv);
        }

        public static DiscreteDistribution Log(BaseDistribution dpdf, double nBase)
        {
            return Operation(dpdf, nBase, DistributionsOperation.Log);
        }

        public static DiscreteDistribution Log(double value, BaseDistribution nBase)
        {
            return Operation(nBase, value, DistributionsOperation.LogInv);
        }

        public static DiscreteDistribution Sin(BaseDistribution distribution)
        {
            return Operation(distribution, 0, DistributionsOperation.Sin);
        }

        public static DiscreteDistribution Cos(BaseDistribution distribution)
        {
            return Operation(distribution, 0, DistributionsOperation.Cos);
        }

        public static DiscreteDistribution Tan(BaseDistribution distribution)
        {
            return Operation(distribution, 0, DistributionsOperation.Tan);
        }

        public static DiscreteDistribution Abs(BaseDistribution dpdf)
        {
            return Operation(dpdf, 0, DistributionsOperation.Abs);
        }

        private static DiscreteDistribution Operation(BaseDistribution dpdf, double value, DistributionsOperation action)
        {
            int length = dpdf.InnerSamples;

            double[] yCoordinates = new double[length];
            double[] xCoordinates = new double[length];


            switch (action)
            {
                case DistributionsOperation.DivideInv:
                    {
                        if (value == 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.DivisionOfZero);

                        if ((dpdf.InnerMinX < 0 && dpdf.InnerMaxX > 0) || dpdf.InnerMinX == 0 || dpdf.InnerMaxX == 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.DivisionByZeroCrossingRandom);

                        double r1 = value / dpdf.InnerMinX;
                        double r2 = value / dpdf.InnerMaxX;
						double step;
                        xCoordinates = GenerateXAxis(r1, r2, length, out step);

                        for (int i = 0; i < length; i++)
                        {
                            double z = xCoordinates[i];
                            double d = value / z;
                            double k = Math.Abs(value) / Math.Pow(z, 2);

                            yCoordinates[i] = dpdf.InnerGetPDFYbyX(d) * k;
                        }

                        break;
                    }
                case DistributionsOperation.Power:
                    {
                        if (value == 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ExponentialOfRandomInZeroPower);

                        if (value < 0 && ((dpdf.InnerMinX < 0 && dpdf.InnerMaxX > 0) || dpdf.InnerMinX == 0 || dpdf.InnerMaxX == 0))
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ExponentialOfZeroCrossingRandomInNegativePower);

                        bool evenPower = Math.Abs(value % 2) == 0;
                        bool naturalPower = value - (int)value == 0;

                        if (dpdf.InnerMinX < 0 && !naturalPower)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ExponentialOfNotPositiveRandomInIrrationalPower);

                        double r1 = Math.Pow(dpdf.InnerMinX, value);
                        double r2 = Math.Pow(dpdf.InnerMaxX, value);

                        if (dpdf.InnerMinX < 0 && dpdf.InnerMaxX > 0 && evenPower)
                        {
                            r2 = Math.Max(r1, r2);
                            r1 = 0;
                        }

						double step;
                        xCoordinates = GenerateXAxis(r1, r2, length, out step);

                        for (int i = 0; i < length; i++)
                        {
                            double d = Math.Sign(xCoordinates[i]) * Math.Pow(Math.Abs(xCoordinates[i]), 1d / value);
                            double k = Math.Abs(Math.Pow(Math.Abs(xCoordinates[i]), (1d - value) / value) / value);

                            yCoordinates[i] = dpdf.InnerGetPDFYbyX(d) * k;

                            if (evenPower)
                            {
                                yCoordinates[i] += dpdf.InnerGetPDFYbyX(-d) * k;
                            }
                        }

                        break;
                    }
                case DistributionsOperation.PowerInv:
                    {
                        if (value == 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ExponentialOfZeroInRandomPower);
                        else if (value < 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ExponentialOfNegativeInRandomPower);
                        else if (value == 1)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ExponentialOfOneInRandomPower);

                        double r1 = Math.Pow(value, dpdf.InnerMinX);
                        double r2 = Math.Pow(value, dpdf.InnerMaxX);
						double step;

                        xCoordinates = GenerateXAxis(r1, r2, length, out step);

                        for (int i = 0; i < length; i++)
                        {
                            double d = Math.Log(xCoordinates[i], value);
                            double k = Math.Abs(Math.Log(value) * xCoordinates[i]);

                            yCoordinates[i] = dpdf.InnerGetPDFYbyX(d) / k;
                        }

                        break;
                    }
                case DistributionsOperation.Log:
                    {
                        if (value == 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmWithZeroBase);
                        else if (value < 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmWithNegativeBase);
                        else if (value == 1)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmWithOneBase);

                        if (dpdf.InnerMinX <= 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmOfNotPositiveRandom);


                        double r1 = Math.Log(dpdf.InnerMinX, value);
                        double r2 = Math.Log(dpdf.InnerMaxX, value);
						double step;
                        xCoordinates = GenerateXAxis(r1, r2, length, out step);

                        for (int i = 0; i < length; i++)
                        {
                            double d = Math.Pow(value, xCoordinates[i]);
                            double k = Math.Abs(Math.Log(value) * d);

                            yCoordinates[i] = dpdf.InnerGetPDFYbyX(d) * k;
                        }
                        break;
                    }
                case DistributionsOperation.LogInv:
                    {
                        if (dpdf.InnerMinX <= 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmWithNotPositiveRandomBase);
                        if ((dpdf.InnerMinX < 1 && dpdf.InnerMaxX > 1) || dpdf.InnerMinX == 1 || dpdf.InnerMaxX == 1)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmWithOneCrossingRandomBase);
                        if (value == 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmOfZeroValue);
                        if (value < 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmOfNegativeValue);

                        double r1 = Math.Log(value, dpdf.InnerMaxX);
                        double r2 = Math.Log(value, dpdf.InnerMinX);
						double step;
                        xCoordinates = GenerateXAxis(r1, r2, length, out step);

                        for (int i = 0; i < length; i++)
                        {
                            double d = Math.Pow(value, 1d / xCoordinates[i]);

                            double k = Math.Abs((d * Math.Log(value)) / Math.Pow(xCoordinates[i], 2));
                            yCoordinates[i] = dpdf.InnerGetPDFYbyX(d) * k;
                        }

                        break;
                    }
                case DistributionsOperation.Abs:
                    {
                        double r1 = Math.Abs(dpdf.InnerMinX);
                        double r2 = Math.Abs(dpdf.InnerMaxX);

                        if (dpdf.InnerMinX <= 0 && dpdf.InnerMaxX >= 0)
                        {
                            r2 = Math.Max(r1, r2);
                            r1 = 0;
                        }

						double step;
                        xCoordinates = GenerateXAxis(r1, r2, length, out step);

                        for (int i = 0; i < length; i++)
                        {
                            double zPow = xCoordinates[i];

                            yCoordinates[i] = dpdf.InnerGetPDFYbyX(zPow) + dpdf.InnerGetPDFYbyX(-zPow);
                        }

                        break;
                    }
                case DistributionsOperation.Sin:
                    {
                        double r1 = -1;
                        double r2 = 1;

                        double step;

                        xCoordinates = GenerateXAxis(r1, r2, length, out step);


                        //max asin [-PI/2, PI/2]
                        int minJ = (int)(dpdf.MinX / (2 * Math.PI)) - 1;
                        int maxJ = (int)(dpdf.MaxX / (2 * Math.PI)) + 1;

                        for (int i = 0; i < length; i++)
                        {
                            double z = xCoordinates[i];

                            double arcsin = Math.Asin(z);
                            double k = 1d / Math.Sqrt(1 - Math.Pow(z, 2));

                            for (double j = minJ; j <= maxJ; j++)
                            {
                                double v = dpdf.InnerGetPDFYbyX(Math.PI * 2 * j - (Math.PI + arcsin)) + dpdf.InnerGetPDFYbyX(Math.PI * 2 * j + arcsin);
                                if (v != 0)
                                    yCoordinates[i] += k * v;
                            }
                        }

                        break;
                    }
                case DistributionsOperation.Cos:
                    {
                        //https://mathoverflow.net/questions/35260/resultant-probability-distribution-when-taking-the-cosine-of-gaussian-distribute
                        double[] range = GetTrigonometricRange(-1, 1, dpdf);

                        double r1 = -1;
                        double r2 = 1;

                        double step;

                        xCoordinates = GenerateXAxis(r1, r2, length, out step);


                        //max acos [0, PI]
                        int minJ = (int)(dpdf.MinX / (2 * Math.PI)) - 1;
                        int maxJ = (int)(dpdf.MaxX / (2 * Math.PI)) + 1;


                        for (int i = 0; i < length; i++)
                        {
                            double z = xCoordinates[i];

                            double acos = Math.Acos(z);
                            double k = 1d / Math.Sqrt(1 - Math.Pow(z, 2));

                            for (double j = minJ; j <= maxJ; j++)
                            {
                                double v = dpdf.InnerGetPDFYbyX(2 * (j + 1) * Math.PI - acos) + dpdf.InnerGetPDFYbyX(2 * j * Math.PI + acos);
                                if (v != 0)
                                    yCoordinates[i] += k * v;
                            }
                        }

                        break;
                    }
                case DistributionsOperation.Tan:
                    {
                        if (dpdf.MaxX - dpdf.MinX >= Math.PI || dpdf.MinX % Math.PI <= -Math.PI / 2 || dpdf.MaxX % Math.PI >= Math.PI / 2)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.TangentOfValueCrossingAsymptote);

                        double r1 = Math.Tan(dpdf.MinX);
                        double r2 = Math.Tan(dpdf.MaxX);

                        double step;

                        xCoordinates = GenerateXAxis(r1, r2, length, out step);

                        int j = dpdf.Mean < 0 ? (int)((dpdf.MinX) / Math.PI) : (int)((dpdf.MaxX) / Math.PI);

                        for (int i = 0; i < length; i++)
                        {
                            double z = xCoordinates[i];

                            double atan = Math.Atan(z);
                            double k = 1d / (Math.Pow(z, 2) + 1);

                            double v = dpdf.InnerGetPDFYbyX(-Math.PI / 2d + Math.PI * j) + dpdf.InnerGetPDFYbyX(atan + j * Math.PI);

                            if (v != 0)
                                yCoordinates[i] = k * v;

                        }

                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            return new DiscreteDistribution(xCoordinates, yCoordinates);
        }

        private static double[] GetTrigonometricRange(double min, double max, BaseDistribution dpdf)
        {
            double[] range = new double[2];
            if (dpdf.MinX >= min && dpdf.MinX <= max)
            {
                range[0] = dpdf.MinX;
            }
            else
            {
                range[0] = min;
            }

            if (dpdf.MaxX >= min && dpdf.MaxX <= max)
            {
                range[1] = dpdf.MaxX;
            }
            else
            {
                range[1] = max;
            }

            return range;
        }

        public static double InterpolateLinear(double x0, double x1, double y0, double y1, double x)
        {
            return y0 + (y1 - y0) / (x1 - x0) * (x - x0);
        }

        public static double GetTolerance(int samples)
        {
            return 0.5d / Math.Pow(samples, 2);
        }

        public static double[] GenerateXAxis(double r1, double r2, int samples, out double step)
        {

            if (double.IsNaN(r1) || double.IsInfinity(r1))
                r1 = 0;

            if (double.IsNaN(r2) || double.IsInfinity(r2))
                r2 = 0;

            double min = Math.Min(r1, r2);
            double max = Math.Max(r1, r2);

            decimal decimalMin = (decimal)min;
            decimal decimalMax = (decimal)max;

            double[] result = new double[samples];

            decimal decimalStep = (decimalMax - decimalMin) / (samples - 1);
            decimal d = decimalMin;

            for (int i = 0; i < samples; i++)
            {
                if (i == 0)
                    result[i] = min;
                else if (i == samples - 1)
                    result[i] = max;
                else
                {
                    result[i] = (double)(decimalMin + i * decimalStep);
                }

                d += decimalStep;
            }

            step = (double)decimalStep;

            return result;

        }

        public static decimal[] GenerateXAxisDecimal(double r1, double r2, int samples, out decimal step)
        {

            if (double.IsNaN(r1) || double.IsInfinity(r1))
                r1 = 0;

            if (double.IsNaN(r2) || double.IsInfinity(r2))
                r2 = 0;

            decimal decimalMin = (decimal)Math.Min(r1, r2);
            decimal decimalMax = (decimal)Math.Max(r1, r2);

            decimal[] result = new decimal[samples];

            decimal decimalStep = (decimalMax - decimalMin) / (samples - 1);
            decimal d = decimalMin;

            for (int i = 0; i < samples; i++)
            {
                if (i == 0)
                    result[i] = decimalMin;
                else if (i == samples - 1)
                    result[i] = decimalMax;
                else
                {
                    result[i] = decimalMin + i * decimalStep;
                }

                d += decimalStep;
            }

            step = decimalStep;

            return result;

        }

        public static double[] GetRange(double min1, double max1, double min2, double max2, DistributionsOperation action)
        {
            double[] allBounds = new double[4];
            allBounds[0] = min1;
            allBounds[1] = max1;
            allBounds[2] = min2;
            allBounds[3] = max2;

            double[] result = new double[2];

            switch (action)
            {
                case DistributionsOperation.Add:
                    {
                        result[0] = min1 + min2;
                        result[1] = max1 + max2;

                        break;
                    }
                case DistributionsOperation.Sub:
                    {
                        result[0] = min1 - max2;
                        result[1] = max1 - min2;
                        break;
                    }
                case DistributionsOperation.Muliply:
                    {
                        double[] variants = new double[4];
                        variants[0] = min1 * min2;
                        variants[1] = min1 * max2;
                        variants[2] = max1 * min2;
                        variants[3] = max1 * max2;

                        result[0] = variants.Min();
                        result[1] = variants.Max();

                        break;
                    }
                case DistributionsOperation.Divide:
                    {
                        if (min2 == 0 || max2 == 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.DivisionByZeroCrossingRandom);
                        if (min2 < 0 && max2 > 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.DivisionByZeroCrossingRandom);


                        double[] variants = new double[4];
                        variants[0] = min1 / min2;
                        variants[1] = min1 / max2;
                        variants[2] = max1 / min2;
                        variants[3] = max1 / max2;

                        result[0] = variants.Min();
                        result[1] = variants.Max();

                        break;
                    }
                case DistributionsOperation.PowerInv:
                    {
                        if (min2 < 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.ExponentialOfNotPositiveRandomInIrrationalPower);

                        double[] variants = new double[4];
                        variants[0] = Math.Pow(min2, min1);
                        variants[1] = Math.Pow(max2, min1);
                        variants[2] = Math.Pow(min2, max1);
                        variants[3] = Math.Pow(max2, max1);

                        result[0] = variants.Min();
                        result[1] = variants.Max();

                        break;
                    }
                case DistributionsOperation.Log:
                    {
                        if (min1 <= 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmOfNotPositiveRandom);
                        if (min2 <= 0)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmWithNotPositiveRandomBase);

                        if ((min2 < 1 && max2 > 1) || min2 == 1 || max2 == 1)
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.LogarithmWithOneCrossingRandomBase);

                        double[] variants = new double[4];
                        variants[0] = Math.Log(min1, min2);
                        variants[1] = Math.Log(max1, min2);
                        variants[2] = Math.Log(min1, max2);
                        variants[3] = Math.Log(max1, max2);

                        result[0] = variants.Min();
                        result[1] = variants.Max();

                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }

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
                int xInt = (int)x;

                if (xInt >= oldLength - 1)
                {
                    newArray[i] = array[oldLength - 1];
                }
                else
                {
                    double k = x - xInt;

                    if (k == 0)
                    {
                        newArray[i] = array[xInt];
                    }
                    else
                    {
                        double min = array[xInt];
                        double max = array[xInt + 1];
                        newArray[i] = (max - min) * k + min;
                    }
                }
            }

            return newArray;
        }

        public static double SimpsonsIntegration(double[] array, double step)
        {
            int length = array.Length;

            double result = array[0] + array[length - 1];

            int c = 0;

            for (int i = 1; i < length / 2 - 1; i++)
            {
                result += 2 * array[i * 2];
                c++;
            }

            for (int i = 1; i < length / 2; i++)
            {
                result += 4 * array[i * 2 - 1];
                c++;
            }

            return result * step / 3d;
        }

    }
}
