using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    internal static class CommonRandomMath
    {
        public static DiscreteDistribution Divide(double value, BaseDistribution dpdf)
        {
            return Operation(dpdf, value, DistributionsOperation.DivideInv);
        }

        public static DiscreteDistribution Power(BaseDistribution dpdf, double value)
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
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.DivisionOfZero);

                        if ((dpdf.InnerMinX < 0 && dpdf.InnerMaxX > 0) || dpdf.InnerMinX == 0 || dpdf.InnerMaxX == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.DivisionByZeroCrossingRandom);

                        double r1 = value / dpdf.InnerMinX;
                        double r2 = value / dpdf.InnerMaxX;
						double step;
                        xCoordinates = CommonRandomMath.GenerateXAxis(r1, r2, length, out step);

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
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfRandomInZeroPower);

                        if (value < 0 && ((dpdf.InnerMinX < 0 && dpdf.InnerMaxX > 0) || dpdf.InnerMinX == 0 || dpdf.InnerMaxX == 0))
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfZeroCrossingRandomInNegativePower);

                        bool evenPower = Math.Abs(value % 2) == 0;
                        bool naturalPower = value - (int)value == 0;

                        if (dpdf.InnerMinX < 0 && !naturalPower)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfNotPositiveRandomInIrrationalPower);

                        double r1 = Math.Pow(dpdf.InnerMinX, value);
                        double r2 = Math.Pow(dpdf.InnerMaxX, value);

                        if (dpdf.InnerMinX < 0 && dpdf.InnerMaxX > 0 && evenPower)
                        {
                            r2 = Math.Max(r1, r2);
                            r1 = 0;
                        }

						double step;
                        xCoordinates = CommonRandomMath.GenerateXAxis(r1, r2, length, out step);

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
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfZeroInRandomPower);
                        else if (value < 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfNegativeInRandomPower);
                        else if (value == 1)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfOneInRandomPower);

                        double r1 = Math.Pow(value, dpdf.InnerMinX);
                        double r2 = Math.Pow(value, dpdf.InnerMaxX);
						double step;

                        xCoordinates = CommonRandomMath.GenerateXAxis(r1, r2, length, out step);

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
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithZeroBase);
                        else if (value < 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithNegativeBase);
                        else if (value == 1)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithOneBase);

                        if (dpdf.InnerMinX <= 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmOfNotPositiveRandom);


                        double r1 = Math.Log(dpdf.InnerMinX, value);
                        double r2 = Math.Log(dpdf.InnerMaxX, value);
						double step;
                        xCoordinates = CommonRandomMath.GenerateXAxis(r1, r2, length, out step);

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
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithNotPositiveRandomBase);
                        if ((dpdf.InnerMinX < 1 && dpdf.InnerMaxX > 1) || dpdf.InnerMinX == 1 || dpdf.InnerMaxX == 1)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithOneCrossingRandomBase);
                        if (value == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmOfZeroValue);
                        if (value < 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmOfNegativeValue);

                        double r1 = Math.Log(value, dpdf.InnerMaxX);
                        double r2 = Math.Log(value, dpdf.InnerMinX);
						double step;
                        xCoordinates = CommonRandomMath.GenerateXAxis(r1, r2, length, out step);

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
                        xCoordinates = CommonRandomMath.GenerateXAxis(r1, r2, length, out step);

                        for (int i = 0; i < length; i++)
                        {
                            double zPow = xCoordinates[i];

                            yCoordinates[i] = dpdf.InnerGetPDFYbyX(zPow) + dpdf.InnerGetPDFYbyX(-zPow);
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
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.DivisionByZeroCrossingRandom);
                        if (min2 < 0 && max2 > 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.DivisionByZeroCrossingRandom);


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
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfNotPositiveRandomInIrrationalPower);

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
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmOfNotPositiveRandom);
                        if (min2 <= 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithNotPositiveRandomBase);

                        if ((min2 < 1 && max2 > 1) || min2 == 1 || max2 == 1)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithOneCrossingRandomBase);

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
    }
}
