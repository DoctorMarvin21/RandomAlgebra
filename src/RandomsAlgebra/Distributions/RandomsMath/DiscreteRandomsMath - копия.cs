using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomsAlgebra.Distributions
{
    internal static class DiscreteRandomsMath
    {
        private enum DistributionsOperation
        {
            Add,
            Sub,
            SubInv,
            Muliply,
            Divide,
            DivideInv,
            Power,
            PowerInv,
            Log,
            LogInv
        }

        public static DiscreteDistribution Add(DiscreteDistribution dpdf, double value)
        {
            return DiscreteDistributionAndValue(dpdf, value, DistributionsOperation.Add);
        }
        public static DiscreteDistribution Sub(DiscreteDistribution dpdf, double value)
        {
            return DiscreteDistributionAndValue(dpdf, value, DistributionsOperation.Sub);
        }
        public static DiscreteDistribution Sub(double value, DiscreteDistribution dpdf)
        {
            return DiscreteDistributionAndValue(dpdf, value, DistributionsOperation.SubInv);
        }
        public static DiscreteDistribution Multiply(DiscreteDistribution dpdf, double value)
        {
            return DiscreteDistributionAndValue(dpdf, value, DistributionsOperation.Muliply);
        }
        public static DiscreteDistribution Divide(DiscreteDistribution dpdf, double value)
        {
            return DiscreteDistributionAndValue(dpdf, value, DistributionsOperation.Divide);
        }
        public static DiscreteDistribution Divide(double value, DiscreteDistribution dpdf)
        {
            return DiscreteDistributionAndValue(dpdf, value, DistributionsOperation.DivideInv);
        }
        public static DiscreteDistribution Log(DiscreteDistribution dpdf, double nBase)
        {
            return DiscreteDistributionAndValue(dpdf, nBase, DistributionsOperation.Log);
        }
        public static DiscreteDistribution Log(double value, DiscreteDistribution nBase)
        {
            return DiscreteDistributionAndValue(nBase, value, DistributionsOperation.LogInv);
        }

        public static DiscreteDistribution Log(DiscreteDistribution dpdf, DiscreteDistribution nBase)
        {
            return TwoDiscreteDistributions(dpdf, nBase, DistributionsOperation.Log);
        }

        public static DiscreteDistribution Abs(DiscreteDistribution dpdf)
        {
            int length = dpdf.Length;
            double[] yCoordinates = new double[length];
            double[] xCoordinates = new double[length];

            double r1 = Math.Abs(dpdf.MinXInternal);
            double r2 = Math.Abs(dpdf.MaxXInternal);

            if (dpdf.MinXInternal <= 0 && dpdf.MaxXInternal >= 0)
            {
                r2 = Math.Max(r1, r2);
                r1 = 0;
            }


            xCoordinates = CommonMath.GenerateXAxis(r1, r2, length, out double step);

            for (int i = 0; i < length; i++)
            {
                double zPow = xCoordinates[i];

                yCoordinates[i] = dpdf.GetPDFYbyXInternal(zPow) + dpdf.GetPDFYbyXInternal(-zPow);
            }

            return new DiscreteDistribution(xCoordinates, yCoordinates, dpdf.Optimizations);
        }
        public static DiscreteDistribution Negate(DiscreteDistribution value)
        {
            return Multiply(value, -1);
        }

        public static DiscreteDistribution Power(DiscreteDistribution dpdf, double value)
        {
            return DiscreteDistributionAndValue(dpdf, value, DistributionsOperation.Power);
        }

        public static DiscreteDistribution Power(double value, DiscreteDistribution dpdf)
        {
            return DiscreteDistributionAndValue(dpdf, value, DistributionsOperation.PowerInv);
        }

        public static DiscreteDistribution Power(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight)
        {
            //так быстрее, и не теряются корни. Никогда.
            return TwoDiscreteDistributions(dpdfRight, dpdfLeft, DistributionsOperation.PowerInv);
            //return TwoDiscreteDistributions(dpdfX, dpdfY, DistributionsOperation.Power);
        }


        private static DiscreteDistribution DiscreteDistributionAndValue(DiscreteDistribution dpdf, double value, DistributionsOperation action)
        {
            int length = dpdf.Length;
            double[] leftX = dpdf.XCoordinatesInternal;
            double[] leftY = dpdf.YCoordinatesInternal;

            double[] yCoordinates = new double[length];
            double[] xCoordinates = new double[length];


            switch (action)
            {
                case DistributionsOperation.Add:
                    {
                        for (int i = 0; i < length; i++)
                        {
                            yCoordinates[i] = leftY[i];
                            xCoordinates[i] = leftX[i] + value;
                        }
                        break;
                    }
                case DistributionsOperation.Sub:
                    {
                        for (int i = 0; i < length; i++)
                        {
                            yCoordinates[i] = leftY[i];
                            xCoordinates[i] = leftX[i] - value;
                        }
                        break;
                    }
                case DistributionsOperation.SubInv:
                    {
                        for (int i = 0; i < length; i++)
                        {
                            yCoordinates[length - i - 1] = leftY[i];
                            xCoordinates[length - i - 1] = value - leftX[i];
                        }
                        break;
                    }
                case DistributionsOperation.Muliply:
                    {
                        if (value == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.MultiplyRandomByZero);

                        if (value < 0)
                        {
                            for (int i = 0; i < length; i++)
                            {
                                yCoordinates[length - i - 1] = leftY[i] / Math.Abs(value);
                                xCoordinates[length - i - 1] = leftX[i] * value;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < length; i++)
                            {
                                yCoordinates[i] = leftY[i] / Math.Abs(value);
                                xCoordinates[i] = leftX[i] * value;
                            }
                        }
                        break;
                    }
                case DistributionsOperation.Divide:
                    {
                        if (value == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.DivisionByZero);

                        if (value < 0)
                        {
                            for (int i = 0; i < length; i++)
                            {
                                yCoordinates[length - i - 1] = leftY[i] * Math.Abs(value);
                                xCoordinates[length - i - 1] = leftX[i] / value;
                            }
                        }
                        else
                        {
                            for (int i = 0; i < length; i++)
                            {
                                yCoordinates[i] = leftY[i] * Math.Abs(value);
                                xCoordinates[i] = leftX[i] / value;
                            }
                        }
                        break;
                    }
                case DistributionsOperation.DivideInv:
                    {
                        if (value == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.DivisionOfZero);

                        if ((dpdf.MinXInternal < 0 && dpdf.MaxXInternal > 0)|| dpdf.MinXInternal == 0 || dpdf.MaxXInternal == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.DivisionByZeroCrossingRandom);

                        double r1 = value / dpdf.MinXInternal;
                        double r2 = value / dpdf.MaxXInternal;

                        xCoordinates = CommonMath.GenerateXAxis(r1, r2, length, out double step);

                        for (int i = 0; i < length; i++)
                        {
                            double z = xCoordinates[i];
                            double d = value / z;
                            double k = Math.Abs(value) / Math.Pow(z, 2);

                            yCoordinates[i] = dpdf.GetPDFYbyXInternal(d) * k;
                        }

                        break;
                    }
                case DistributionsOperation.Power:
                    {
                        if (value == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfRandomInZeroPower);

                        if (value < 0 && ((dpdf.MinXInternal < 0 && dpdf.MaxXInternal > 0) || dpdf.MinXInternal == 0 || dpdf.MaxXInternal == 0))
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfZeroCrossingRandomInNegativePower);

                        bool evenPower = Math.Abs(value % 2) == 0;
                        bool naturalPower = value - (int)value == 0;

                        if (dpdf.MinXInternal <= 0 && !naturalPower)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.ExponentialOfNotPositiveRandomInIrrationalPower);

                        double r1 = Math.Pow(dpdf.MinXInternal, value);
                        double r2 = Math.Pow(dpdf.MaxXInternal, value);

                        if (dpdf.MinXInternal < 0 && dpdf.MaxXInternal > 0 && evenPower)
                        {
                            r2 = Math.Max(r1, r2);
                            r1 = 0;
                        }


                        xCoordinates = CommonMath.GenerateXAxis(r1, r2, length, out double step);

                        for (int i = 0; i < length; i++)
                        {
                            double d =  Math.Sign(xCoordinates[i]) * Math.Pow(Math.Abs(xCoordinates[i]), 1d / value);
                            double k = Math.Abs(Math.Pow(Math.Abs(xCoordinates[i]), (1d - value) / value) / value);

                            yCoordinates[i] = dpdf.GetPDFYbyXInternal(d) * k;

                            if (evenPower)
                            {
                                yCoordinates[i] += dpdf.GetPDFYbyXInternal(-d) * k;
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

                        double r1 = Math.Pow(value, dpdf.MinXInternal);
                        double r2 = Math.Pow(value, dpdf.MaxXInternal);

                        xCoordinates = CommonMath.GenerateXAxis(r1, r2, length, out double step);

                        for (int i = 0; i < length; i++)
                        {
                            double d = Math.Log(xCoordinates[i], value);
                            double k = Math.Abs(Math.Log(value) * xCoordinates[i]);

                            yCoordinates[i] = dpdf.GetPDFYbyXInternal(d) / k;
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

                        if (dpdf.MinXInternal <= 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmOfNotPositiveRandom);


                        double r1 = Math.Log(dpdf.MinXInternal, value);
                        double r2 = Math.Log(dpdf.MaxXInternal, value);

                        xCoordinates = CommonMath.GenerateXAxis(r1, r2, length, out double step);

                        for (int i = 0; i < length; i++)
                        {
                            double d = Math.Pow(value, xCoordinates[i]);
                            double k = Math.Abs(Math.Log(value) * d);

                            yCoordinates[i] = dpdf.GetPDFYbyXInternal(d) * k;
                        }
                        break;
                    }
                case DistributionsOperation.LogInv:
                    {
                        if (dpdf.MinXInternal <= 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithNotPositiveRandomBase);
                        if ((dpdf.MinXInternal < 1 && dpdf.MaxXInternal > 1) || dpdf.MinXInternal == 1 || dpdf.MaxXInternal == 1)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmWithOneCrossingRandomBase);
                        if (value == 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmOfZeroValue);
                        if (value < 0)
                            CommonExceptions.ThrowCommonExcepton(CommonExceptionType.LogarithmOfNegativeValue);

                        double r1 = Math.Log(value, dpdf.MaxXInternal);
                        double r2 = Math.Log(value, dpdf.MinXInternal);

                        xCoordinates = CommonMath.GenerateXAxis(r1, r2, length, out double step);

                        for (int i = 0; i < length; i++)
                        {
                            double d = Math.Pow(value, 1d / xCoordinates[i]);

                            double k = Math.Abs((d * Math.Log(value)) / Math.Pow(xCoordinates[i], 2));
                            yCoordinates[i] = dpdf.GetPDFYbyXInternal(d) * k;
                        }

                        break;
                    }
            }

            return new DiscreteDistribution(xCoordinates, yCoordinates, dpdf.Optimizations);
        }

        public static DiscreteDistribution Add(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight)
        {
            if (dpdfLeft.Optimizations.UseFFTConvolution && dpdfLeft.Optimizations.UseFFTConvolution)
            {
                return FFT.Convolute(dpdfLeft, dpdfRight, dpdfLeft.Length);
            }
            else
            {
                return TwoDiscreteDistributions(dpdfLeft, dpdfRight, DistributionsOperation.Add);
            }
            
        }
        public static DiscreteDistribution Sub(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight)
        {
            if (dpdfLeft.Optimizations.UseFFTConvolution && dpdfLeft.Optimizations.UseFFTConvolution)
            {
                return FFT.Convolute(dpdfLeft, Multiply(dpdfRight, -1), dpdfLeft.Length);
            }
            else
            {
                return TwoDiscreteDistributions(dpdfLeft, dpdfRight, DistributionsOperation.Sub);
            }
        }
        public static DiscreteDistribution Multiply(DiscreteDistribution dpdfX, DiscreteDistribution dpdfY)
        {
            return TwoDiscreteDistributions(dpdfX, dpdfY, DistributionsOperation.Muliply);
        }
        public static DiscreteDistribution Divide(DiscreteDistribution dpdfX, DiscreteDistribution dpdfY)
        {
            return TwoDiscreteDistributions(dpdfX, dpdfY, DistributionsOperation.Divide);
        }

        private static DiscreteDistribution TwoDiscreteDistributions(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight, DistributionsOperation action)
        {
            if (dpdfLeft.Optimizations.SwapDiscreteDistributions && dpdfRight.Optimizations.SwapDiscreteDistributions)
            {
                var exchange = Swap(dpdfLeft, dpdfRight, action, out var newAction);

                action = newAction;

                dpdfLeft = exchange[0];
                dpdfRight = exchange[1];
            }

            int lengthLeft = dpdfLeft.Length;
            int lengthRight = dpdfRight.Length;

            double stepRight = dpdfRight.Step;

            double[] leftX = dpdfLeft.XCoordinatesInternal;
            double[] leftY = dpdfLeft.YCoordinatesInternal;

            double[] rightX = dpdfRight.XCoordinatesInternal;
            double[] rightY = dpdfRight.YCoordinatesInternal;

            double[] yCoordinates = new double[lengthRight];

            double[] range = GetRange(dpdfLeft.MinXInternal, dpdfLeft.MaxXInternal, dpdfRight.MinXInternal, dpdfRight.MaxXInternal, action);
            double[] xCoordinates = CommonMath.GenerateXAxis(range[0], range[1], lengthRight, out double stepX0);

            switch (action)
            {
                case DistributionsOperation.Add:
                    {
                        Parallel.For(0, lengthRight, i =>
                        {
                            double m = 0;
                            double x = xCoordinates[i];
                            double sum = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(x - m);
                            }

                            yCoordinates[i] = sum * stepRight;
                        });

                        break;
                    }
                case DistributionsOperation.Sub:
                    {
                        Parallel.For(0, lengthRight, i =>
                        {
                            double m = 0;
                            double x = xCoordinates[i];

                            double sum = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(x - m);
                            }

                            yCoordinates[i] = sum * stepRight;
                        });

                        break;
                    }
                case DistributionsOperation.Muliply:
                    {
                        Parallel.For(0, lengthRight, i =>
                        {
                            double m = 0;
                            double sum = 0;
                            double x = xCoordinates[i];
                            double k = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                k = Math.Abs(m);

                                if (k > 0)
                                {
                                    sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(x / m) / k;
                                }
                            }

                            yCoordinates[i] = sum * stepRight;
                        });

                        break;
                    }
                case DistributionsOperation.Divide:
                    {
                        Parallel.For(0, lengthRight, i =>
                        {
                            double m = 0;
                            double sum = 0;
                            double x = xCoordinates[i];
                            double k = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                k = Math.Abs(m);

                                if (k > 0)
                                {
                                    sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(x * m) * k;
                                }
                            }

                            yCoordinates[i] = sum * stepRight;
                        });

                        break;
                    }
                case DistributionsOperation.PowerInv:
                    {
                        Parallel.For(0, lengthRight, i =>
                        {
                            double m = 0;
                            double sum = 0;
                            double x = xCoordinates[i];
                            double d = 0;
                            double k = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];

                                d = Math.Log(x, m);
                                k = Math.Abs(Math.Log(m) * x);
                                sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(d) / k;
                            }

                            yCoordinates[i] = sum * stepRight;
                        });

                        break;
                    }
                case DistributionsOperation.Log:
                    {
                        Parallel.For(0, lengthRight, i =>
                        {
                            double m = 0;
                            double sum = 0;
                            double x = xCoordinates[i];
                            double d = 0;
                            double k = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];

                                d = Math.Pow(m, x); 
                                k = Math.Abs(Math.Log(m) * d);
                                sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(d) * k;
                            }

                            yCoordinates[i] = sum * stepRight;
                        });

                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            var result = new DiscreteDistribution(xCoordinates, yCoordinates, dpdfLeft.Optimizations);
            return result;
        }

        private static DiscreteDistribution[] Swap(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight, DistributionsOperation action, out DistributionsOperation newAction)
        {
            double stepX = dpdfLeft.Step;
            double stepY = dpdfRight.Step;

            switch (action)
            {
                case DistributionsOperation.Add:
                case DistributionsOperation.Muliply:
                    {
                        newAction = action;

                        if (stepX < stepY)
                        {
                            return new DiscreteDistribution[] { dpdfRight, dpdfLeft };
                        }
                        else
                        {
                            return new DiscreteDistribution[] { dpdfLeft, dpdfRight };
                        }
                    }
                case DistributionsOperation.Sub:
                    {
                        if (stepX < stepY)
                        {
                            newAction = DistributionsOperation.Add;
                            return new DiscreteDistribution[] { Multiply(dpdfRight, -1), dpdfLeft };
                        }
                        else
                        {
                            newAction = action;
                            return new DiscreteDistribution[] { dpdfLeft, dpdfRight };
                        }
                    }
                case DistributionsOperation.Divide:
                    {
                        if (stepY / stepX > 2)
                        {
                            newAction = DistributionsOperation.Muliply;
                            return new DiscreteDistribution[] { Power(dpdfRight, -1), dpdfLeft };
                        }
                        else
                        {
                            newAction = action;
                            return new DiscreteDistribution[] { dpdfLeft, dpdfRight };
                        }
                    }
                default:
                    {
                        newAction = action;
                        return new DiscreteDistribution[] { dpdfLeft, dpdfRight };
                    }
            }
        }

        private static double[] GetRange(double min1, double max1, double min2, double max2, DistributionsOperation action)
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
                        if (min2 <= 0)
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
