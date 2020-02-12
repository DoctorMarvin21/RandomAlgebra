using System;
using Accord;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class StudentUniformDistribution : UnivariateContinuousDistribution
        {
            private readonly double ua, ub, df, tm, ts;

            private readonly TDistribution baseDistribution;
            private readonly double mean, variance;
            private readonly DoubleRange range = new DoubleRange(double.NegativeInfinity, double.PositiveInfinity);

            public StudentUniformDistribution(double uniformLowerBound, double uniformUpperBound, double mean, double tStd, double degreesOfFreedom)
            {
                ua = uniformLowerBound;
                ub = uniformUpperBound;
                df = degreesOfFreedom;
                tm = mean;
                ts = tStd;

                baseDistribution = new TDistribution(df);
                this.mean = ((ua + ub) / 2d) + tm;
                variance = (Math.Pow(ts, 2) * df / (df - 2)) + (Math.Pow(ub - ua, 2) / 12d);
            }

            public StudentUniformDistribution(double n, double degreesOfFreedom)
            {
                ts = Math.Sqrt(1d / (Math.Pow(n, 2) + 1d));
                double a = n * ts * Math.Sqrt(3);

                df = degreesOfFreedom;
                ua = -a;
                ub = a;
                tm = 0;

                baseDistribution = new TDistribution(df);
                mean = 0;
                variance = (Math.Pow(ts, 2) * df / (df - 2)) + (Math.Pow(a, 2) / 3d);
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

            public override object Clone()
            {
                return new StudentUniformDistribution(ua, ub, ts, tm, df);
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return ToString();
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return 1d / (ub - ua) * (baseDistribution.DistributionFunction((x - tm - ua) / ts) - baseDistribution.DistributionFunction((x - tm - ub) / ts));
            }

            protected override double InnerDistributionFunction(double x)
            {
                return 1d / (ub - ua) * (IntegralFunction((x - tm - ua) / ts) - IntegralFunction((x - tm - ub) / ts));
            }

            private double IntegralFunction(double x)
            {
                return ts * ((x * baseDistribution.DistributionFunction(x)) + (baseDistribution.ProbabilityDensityFunction(x) * (df + Math.Pow(x, 2)) / (df - 1d)));
            }
        }
    }
}
