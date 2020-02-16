using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RandomAlgebra.Distributions.Settings;

namespace RandomAlgebra.Distributions
{
    internal enum DistributionsOperation
    {
        Add,
        Sub,
        SubInv,
        Abs,
        Muliply,
        Divide,
        DivideInv,
        Power,
        PowerInv,
        Log,
        LogInv,
        Sin,
        Cos,
        Tan
    }

    internal static class DiscreteRandomMath
    {
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
            return TwoDiscreteDistributions(dpdfRight, dpdfLeft, DistributionsOperation.PowerInv);
        }

        public static DiscreteDistribution Add(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight)
        {
            if (Optimizations.UseFftConvolution && dpdfLeft.Step / dpdfRight.Step < FFT.MaxStepRate && dpdfRight.Step / dpdfLeft.Step < FFT.MaxStepRate)
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
            if (Optimizations.UseFftConvolution)
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
                        {
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.MultiplyRandomByZero);
                        }

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
                        {
                            throw new DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType.DivisionByZero);
                        }

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

        private static DiscreteDistribution TwoDiscreteDistributions(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight, DistributionsOperation action)
        {
            var exchange = Swap(dpdfLeft, dpdfRight, action, out DistributionsOperation newAction);

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

            double[] range = CommonRandomMath.GetRange(dpdfLeft.InnerMinX, dpdfLeft.InnerMaxX, dpdfRight.InnerMinX, dpdfRight.InnerMaxX, action);

            double[] xCoordinates = CommonRandomMath.GenerateXAxis(range[0], range[1], lengthRight, out double stepX0);

            switch (action)
            {
                case DistributionsOperation.Add:
                    {
                        List<int> operations = new List<int>();

                        Parallel.For(0, lengthRight, i =>
                        {
                            double m = 0;
                            double x = xCoordinates[i];
                            double sum = 0;
                            double y = 0;
                            double r = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                y = rightY[j];

                                r = y * GetYByX(x - m, leftY, leftMinX, leftMaxX, stepLeft, lengthLeft);

                                if (j == 0 || j == lengthRight - 1)
                                {
                                    r /= 2;
                                }

                                sum += r;
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
                            double r = 0;
                            double y = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                y = rightY[j];

                                r = y * GetYByX(x + m, leftY, leftMinX, leftMaxX, stepLeft, lengthLeft);

                                if (j == 0 || j == lengthRight - 1)
                                {
                                    r /= 2;
                                }

                                sum += r;
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
                            double r = 0;
                            double y = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                y = rightY[j];
                                k = Math.Abs(m);

                                if (k != 0)
                                {
                                    r = y * GetYByX(x / m, leftY, leftMinX, leftMaxX, stepLeft, lengthLeft) / k;

                                    if (j == 0 || j == lengthRight - 1)
                                    {
                                        r /= 2;
                                    }

                                    sum += r;
                                }
                            }

                            yCoordinates[i] = sum * stepRight;
                        });

                        // in case when both of distributions cross Oy it is inf in zero
                        if (dpdfLeft.MinX <= 0 && dpdfLeft.MaxX >= 0 && dpdfRight.MinX <= 0 && dpdfRight.MaxX >= 0)
                        {
                            for (int i = 0; i < lengthRight - 1; i++)
                            {
                                if (xCoordinates[i] <= 0 && xCoordinates[i + 1] > 0)
                                {
                                    yCoordinates[i] = double.PositiveInfinity;
                                    break;
                                }
                            }
                        }

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
                            double r = 0;
                            double y = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                y = rightY[j];
                                k = Math.Abs(m);

                                if (k != 0)
                                {
                                    r = y * GetYByX(x * m, leftY, leftMinX, leftMaxX, stepLeft, lengthLeft) * k;

                                    if (j == 0 || j == lengthRight - 1)
                                    {
                                        r /= 2;
                                    }

                                    sum += r;
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
                            double k = 0;
                            double r = 0;
                            double y = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                y = rightY[j];
                                k = Math.Abs(Math.Log(m) * x);

                                if (k != 0)
                                {
                                    r = y * GetYByX(Math.Log(x, m), leftY, leftMinX, leftMaxX, stepLeft, lengthLeft) / k;

                                    if (j == 0 || j == lengthRight - 1)
                                    {
                                        r /= 2;
                                    }

                                    sum += r;
                                }
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
                            double r = 0;
                            double y = 0;

                            for (int j = 0; j < lengthRight; j++)
                            {
                                m = rightX[j];
                                y = rightY[j];
                                d = Math.Pow(m, x);
                                k = Math.Abs(Math.Log(m) * d);

                                if (k != 0)
                                {
                                    r = y * GetYByX(d, leftY, leftMinX, leftMaxX, stepLeft, lengthLeft) * k;

                                    if (j == 0 || j == lengthRight - 1)
                                    {
                                        r /= 2;
                                    }

                                    sum += r;
                                }
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static double GetYByX(double x, double[] coordinates, double minX, double maxX, double step, int length)
        {
            if (x < minX || x > maxX)
            {
                return 0;
            }

            double ind = (x - minX) / step;

            int indInt = (int)ind;

            if (indInt < 0)
            {
                return coordinates[0];
            }
            else if (indInt >= length - 1)
            {
                return coordinates[length - 1];
            }
            else
            {
                double k = ind - indInt;

                if (k == 0)
                {
                    return coordinates[indInt];
                }
                else
                {
                    double min = coordinates[indInt];
                    double max = coordinates[indInt + 1];
                    return min + ((max - min) * k);
                }
            }
        }

        private static DiscreteDistribution[] Swap(DiscreteDistribution dpdfLeft, DiscreteDistribution dpdfRight, DistributionsOperation action, out DistributionsOperation newAction)
        {
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
                case DistributionsOperation.Muliply:
                    {
                        newAction = action;

                        // When one of distributions crossing Oy we need to exact locate them to eliminate 1/0 error on numerical integration
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
                default:
                    {
                        newAction = action;
                        return new DiscreteDistribution[] { dpdfLeft, dpdfRight };
                    }
            }
        }
    }
}
