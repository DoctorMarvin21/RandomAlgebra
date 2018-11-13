using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            return Bivariate(distribution, DistributionsOperation.Power);
        }

        public static BaseDistribution GetLog(BivariateContinuousDistribution distribution)
        {
            return Bivariate(distribution, DistributionsOperation.Log);
        }

        private static DiscreteDistribution Bivariate(BivariateContinuousDistribution distribution, DistributionsOperation operation)
        {
            int samples = distribution.Samples;

            double[] range = CommonRandomMath.GetRange(distribution.SupportLeft[0], distribution.SupportLeft[1], distribution.SupportRight[0], distribution.SupportRight[1], operation);

            double[] rightAxis = CommonRandomMath.GenerateXAxis(distribution.SupportRight[0], distribution.SupportRight[1], samples, out double rightStep);
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

                            for (int j = 0; j < samples; j++)
                            {
                                double m = rightAxis[j];
                                sum += distribution.ProbabilityDensityFunction(x - m, m);
                            }

                            result[i] = sum * rightStep;

                            //result[i] = func.Invoke(0, x);
                        });

                        break;
                    }
                case DistributionsOperation.Sub:
                    {
                        Parallel.For(0, xAxis.Length, i =>
                        {
                            double x = xAxis[i];
                            double sum = 0;

                            for (int j = 0; j < samples; j++)
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

                            for (int j = 0; j < samples; j++)
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

                            for (int j = 0; j < samples; j++)
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
                default:
                    {
                        throw new DistributionsInvalidOperationException();
                    }
            }

            return new DiscreteDistribution(xAxis, result);
        }
    }
}
