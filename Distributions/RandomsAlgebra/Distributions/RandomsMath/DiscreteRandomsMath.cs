using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public static DiscreteDistribution Log(DiscreteDistribution dpdf, DiscreteDistribution nBase)
        {
            return TwoDiscreteDistributions(dpdf, nBase, DistributionsOperation.Log);
        }

        public static DiscreteDistribution Negate(DiscreteDistribution value)
        {
            return Multiply(value, -1);
        }

        public static DiscreteDistribution Power(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight)
        {
            //так быстрее, и не теряются корни. Никогда.
            return TwoDiscreteDistributions(dpdfRight, dpdfLeft, DistributionsOperation.PowerInv);
            //return TwoDiscreteDistributions(dpdfX, dpdfY, DistributionsOperation.Power);
        }


        private static DiscreteDistribution DiscreteDistributionAndValue(DiscreteDistribution dpdf, double value, DistributionsOperation action)
        {
            int length = dpdf.InnerSamples;
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
                default:
                    {
                        throw new NotImplementedException();
                    }
            }

            return new DiscreteDistribution(xCoordinates, yCoordinates);
        }

        public static DiscreteDistribution Add(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight)
        {
            if (Optimizations.UseFFTConvolution && dpdfLeft.Step / dpdfRight.Step < FFT.MaxStepRate && dpdfRight.Step / dpdfLeft.Step < FFT.MaxStepRate)
            {
                return FFT.Convolute(dpdfLeft, dpdfRight, dpdfLeft.InnerSamples);
            }
            else
            {
                return TwoDiscreteDistributions(dpdfLeft, dpdfRight, DistributionsOperation.Add);
            }
            
        }
        public static DiscreteDistribution Sub(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight)
        {
            if (Optimizations.UseFFTConvolution)
            {
                return FFT.Convolute(dpdfLeft, Multiply(dpdfRight, -1), dpdfLeft.InnerSamples);
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
            DistributionsOperation newAction;
            var exchange = Swap(dpdfLeft, dpdfRight, action, out newAction);

            action = newAction;

            dpdfLeft = exchange[0];
            dpdfRight = exchange[1];

            int lengthLeft = dpdfLeft.InnerSamples;
            int lengthRight = dpdfRight.InnerSamples;

            double stepRight = dpdfRight.Step;
            double stepLeft = dpdfLeft.Step;

            double leftMinX = dpdfLeft.MinX;
            double leftMaxX = dpdfLeft.MaxX;

            double[] leftX = dpdfLeft.XCoordinatesInternal;
            double[] leftY = dpdfLeft.YCoordinatesInternal;

            double[] rightX = dpdfRight.XCoordinatesInternal;
            double[] rightY = dpdfRight.YCoordinatesInternal;

            double[] yCoordinates = new double[lengthRight];

            double[] range = GetRange(dpdfLeft.InnerMinX, dpdfLeft.InnerMaxX, dpdfRight.InnerMinX, dpdfRight.InnerMaxX, action);
			double stepX0;
            double[] xCoordinates = CommonMath.GenerateXAxis(range[0], range[1], lengthRight, out stepX0);

            switch (action)
            {
                case DistributionsOperation.Add:
                    {
                        Parallel.For(0, lengthRight, i =>
                        {
                            double m = 0;
                            double x = xCoordinates[i];
                            double sum = 0;
                            double s = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                //sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(x - m);

                                s = rightY[j] * InterpolateInlined(x - m, leftMinX, leftMaxX, stepLeft, leftY);
                                if (i == 0 || i == lengthRight - 1)
                                    s /= 2d;

                                sum += s;
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
                            double s = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                //sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(x + m);
                                s = rightY[j] * InterpolateInlined(x + m, leftMinX, leftMaxX, stepLeft, leftY);
                                if (i == 0 || i == lengthRight - 1)
                                    s /= 2d;
                                sum += s;
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
                            double s = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                k = Math.Abs(m);

                                if (k > 0)
                                {
                                    //sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(x / m) / k;
                                    var l = InterpolateInlined(x / m, leftMinX, leftMaxX, stepLeft, leftY);
                                    s = rightY[j] * l / k;
                                    if (i == 0 || i == lengthRight - 1)
                                        s /= 2d;
                                    sum += s;
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
                            double s = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                k = Math.Abs(m);

                                if (k > 0)
                                {
                                    //sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(x * m) * k;
                                    s = rightY[j] * InterpolateInlined(x * m, leftMinX, leftMaxX, stepLeft, leftY) * k;
                                    if (i == 0 || i == lengthRight - 1)
                                        s /= 2d;
                                    sum += s;
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
                            double s = 0;
                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];

                                d = Math.Log(x, m);
                                k = Math.Abs(Math.Log(m) * x);
                                //sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(d) / k;
                                s = rightY[j] * InterpolateInlined(d, leftMinX, leftMaxX, stepLeft, leftY) / k;
                                if (i == 0 || i == lengthRight - 1)
                                    s /= 2d;
                                sum += s;
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
                            double s = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];

                                d = Math.Pow(m, x); 
                                k = Math.Abs(Math.Log(m) * d);
                                //sum += rightY[j] * dpdfLeft.GetPDFYbyXInternal(d) * k;
                                s = rightY[j] * InterpolateInlined(d, leftMinX, leftMaxX, stepLeft, leftY) * k;
                                if (i == 0 || i == lengthRight - 1)
                                    s /= 2d;
                                sum += s;
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
            var result = new DiscreteDistribution(xCoordinates, yCoordinates);
            return result;
        }

        private static DiscreteDistribution[] Swap(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight, DistributionsOperation action, out DistributionsOperation newAction)
        {
            //TODO:Tests and enhance
            double stepX = dpdfLeft.Step;
            double stepY = dpdfRight.Step;

            switch (action)
            {
                case DistributionsOperation.Add:
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
                case DistributionsOperation.Muliply:
                    {
                        newAction = action;
                        //в случаях, когда одна из случайных величин переспекает 0, все плохо
                        if (dpdfRight.MinX <= 0 && dpdfRight.MaxX >= 0 && !(dpdfLeft.MinX <= 0 && dpdfLeft.MaxX >= 0))
                        {
                            return new DiscreteDistribution[] { dpdfRight, dpdfLeft };
                        }
                        else if (dpdfLeft.MinX <= 0 && dpdfLeft.MaxX >= 0 && !(dpdfRight.MinX <= 0 && dpdfRight.MaxX >= 0))
                        {
                            return new DiscreteDistribution[] { dpdfLeft, dpdfRight };
                        }
                        else
                        {
                            if (stepX < stepY)
                            {
                                return new DiscreteDistribution[] { dpdfRight, dpdfLeft };
                            }
                            else
                            {
                                return new DiscreteDistribution[] { dpdfLeft, dpdfRight };
                            }
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
                            return new DiscreteDistribution[] { CommonRandomsMath.Power(dpdfRight, -1), dpdfLeft };
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double InterpolateInlined(double x, double minX, double maxX, double step, double[] coordinates)
        {
            if (x < minX || x > maxX)
                return 0;

            double ind = (x - minX) / step;

            int intInt = (int)ind;
            double k = ind - intInt;

            if (k == 0)
            {
                return coordinates[intInt];
            }
            else
            {
                if (intInt < 0 || (intInt >= coordinates.Length - 1))
                    return 0;
                else
                {
                    double min = coordinates[intInt];
                    double max = coordinates[intInt + 1];
                    return (max - min) * k + min;
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
