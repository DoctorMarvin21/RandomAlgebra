using System;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    internal static class BivariateMath
    {
        public static BaseDistribution GetSum(BivariateContinuousDistribution distribution)
        {
            return Bivariate(distribution, DistributionsOperation.Add);
        }

        public static BaseDistribution GetDifference(BivariateContinuousDistribution distribution)
        {
            return Bivariate(distribution, DistributionsOperation.Sub);
        }

        public static BaseDistribution GetProduct(BivariateContinuousDistribution distribution)
        {
            return Bivariate(distribution, DistributionsOperation.Muliply);
        }

        public static BaseDistribution GetRatio(BivariateContinuousDistribution distribution)
        {
            return Bivariate(distribution, DistributionsOperation.Divide);
        }

        public static BaseDistribution GetPower(BivariateContinuousDistribution distribution)
        {
            return Bivariate(distribution, DistributionsOperation.PowerInv);
        }

        public static BaseDistribution GetLog(BivariateContinuousDistribution distribution)
        {
            return Bivariate(distribution, DistributionsOperation.Log);
        }

        private static DiscreteDistribution Bivariate(BivariateContinuousDistribution distribution, DistributionsOperation operation)
        {
            if (operation == DistributionsOperation.PowerInv)
            {
                distribution = distribution.Rotate();
            }

            int samples = distribution.Samples;

            double[] range = CommonRandomMath.GetRange(distribution.SupportMinLeft, distribution.SupportMaxLeft, distribution.SupportMinRight, distribution.SupportMaxRight, operation);

            double[] rightAxis = CommonRandomMath.GenerateXAxis(distribution.SupportMinRight, distribution.SupportMaxRight, samples, out double rightStep);
            double[] xAxis = CommonRandomMath.GenerateXAxis(range[0], range[1], samples, out double step);

            double[] result = new double[samples];

            switch (operation)
            {
                case DistributionsOperation.Add:
                    {
                        Parallel.For(0, xAxis.Length, i =>
                        {
                            double x = xAxis[i];
                            double sum = 0;

                            // TODO: there is some bug here, we also need to find reason why can't I swap distributions on bivariate math
                            // Trap rule is useless because both normal and t-distributions are smoooth.
                            for (int j = 1; j < samples; j++)
                            {
                                double m = rightAxis[j];

                                sum += distribution.ProbabilityDensityFunction(x - m, m);
                            }

                            result[i] = sum * rightStep;
                        });

                        break;
                    }
                case DistributionsOperation.Sub:
                    {
                        Parallel.For(0, xAxis.Length, i =>
                        {
                            double x = xAxis[i];
                            double sum = 0;

                            for (int j = 1; j < samples; j++)
                            {
                                double m = rightAxis[j];
                                sum += distribution.ProbabilityDensityFunction(x + m, m);
                            }

                            result[i] = sum * rightStep;
                        });

                        break;
                    }
                case DistributionsOperation.Muliply:
                    {
                        Parallel.For(0, xAxis.Length, i =>
                        {
                            double x = xAxis[i];
                            double sum = 0;

                            for (int j = 1; j < samples; j++)
                            {
                                double m = rightAxis[j];

                                if (m != 0)
                                {
                                    sum += distribution.ProbabilityDensityFunction(x / m, m) / Math.Abs(m);
                                }
                            }

                            result[i] = sum * rightStep;
                        });

                        break;
                    }
                case DistributionsOperation.Divide:
                    {
                        Parallel.For(0, xAxis.Length, i =>
                        {
                            double x = xAxis[i];
                            double sum = 0;

                            for (int j = 1; j < samples; j++)
                            {
                                double m = rightAxis[j];

                                if (m != 0)
                                {
                                    sum += distribution.ProbabilityDensityFunction(x * m, m) * Math.Abs(m);
                                }
                            }

                            result[i] = sum * rightStep;
                        });

                        break;
                    }
                case DistributionsOperation.PowerInv:
                    {
                        Parallel.For(0, xAxis.Length, i =>
                        {
                            double m = 0;
                            double sum = 0;
                            double x = xAxis[i];
                            double d = 0;
                            double k = 0;
                            for (int j = 1; j < samples; j++)
                            {
                                m = rightAxis[j];

                                d = Math.Log(x, m);
                                k = Math.Abs(Math.Log(m) * x);

                                sum += distribution.ProbabilityDensityFunction(d, m) / k;
                            }

                            result[i] = sum * rightStep;
                        });

                        break;
                    }
                case DistributionsOperation.Log:
                    {
                        Parallel.For(0, xAxis.Length, i =>
                        {
                            double m = 0;
                            double sum = 0;
                            double x = xAxis[i];
                            double d = 0;
                            double k = 0;

                            for (int j = 1; j < samples; j++)
                            {
                                m = rightAxis[j];

                                d = Math.Pow(m, x);
                                k = Math.Abs(Math.Log(m) * d);
                                sum += distribution.ProbabilityDensityFunction(d, m) * k;
                            }

                            result[i] = sum * rightStep;
                        });

                        break;
                    }
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }

            return new DiscreteDistribution(xAxis, result);
        }
    }
}
