using System;
using Accord;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class BhattacharjeeDistribution : UnivariateContinuousDistribution
        {
            private readonly double ua, ub, nm, ns;

            private readonly NormalDistribution baseDistributions;
            private readonly double mean, variance;
            private readonly DoubleRange range = new DoubleRange(double.NegativeInfinity, double.PositiveInfinity);

            public BhattacharjeeDistribution(double uniformLowerBound, double uniformUpperBound, double normalMean, double normalStd)
            {
                ua = uniformLowerBound;
                ub = uniformUpperBound;
                nm = normalMean;
                ns = normalStd;

                baseDistributions = new NormalDistribution(0, 1);
                mean = ((ua + ub) / 2d) + nm;
                variance = Math.Pow(ns, 2d) + (Math.Pow(ub - ua, 2) / 12d);
            }

            public BhattacharjeeDistribution(double n)
            {
                ns = Math.Sqrt(1d / (Math.Pow(n, 2) + 1d));
                double a = n * ns * Math.Sqrt(3);

                ua = -a;
                ub = a;
                nm = 0;

                baseDistributions = new NormalDistribution(0, 1);
                mean = 0;
                variance = Math.Pow(ns, 2d) + (Math.Pow(a, 2) / 3d);
            }

            public override double Mean
            {
                get
                {
                    return mean;
                }
            }

            public override double Variance
            {
                get
                {
                    return variance;
                }
            }

            public override double Entropy
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            public override DoubleRange Support
            {
                get
                {
                    return range;
                }
            }

            public double UniformMin
            {
                get
                {
                    return ua;
                }
            }

            public double UniformMax
            {
                get
                {
                    return ub;
                }
            }

            public double NormalMean
            {
                get
                {
                    return nm;
                }
            }

            public double NormalSigma
            {
                get
                {
                    return ns;
                }
            }

            public override object Clone()
            {
                throw new NotImplementedException();
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                throw new NotImplementedException();
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return 1d / (ub - ua) * (baseDistributions.DistributionFunction((x - nm - ua) / ns) - baseDistributions.DistributionFunction((x - nm - ub) / ns));
            }

            protected override double InnerDistributionFunction(double x)
            {
                return 1d / (ub - ua) * (IntegralFunction((x - nm - ua) / ns) - IntegralFunction((x - nm - ub) / ns));
            }

            private double IntegralFunction(double x)
            {
                return ns * ((x * baseDistributions.DistributionFunction(x)) + baseDistributions.ProbabilityDensityFunction(x));
            }
        }
    }
}
