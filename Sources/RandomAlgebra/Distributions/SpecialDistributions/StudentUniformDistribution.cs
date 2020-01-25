using Accord;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class StudentUniformDistribution : UnivariateContinuousDistribution
        {
            private readonly double ua, ub, df, tm, ts;

            private readonly TDistribution _base;
            private readonly double _mean, _variance;
            private readonly DoubleRange _range = new DoubleRange(double.NegativeInfinity, double.PositiveInfinity);

            public StudentUniformDistribution(double uniformLowerBound, double uniformUpperBound, double mean, double tStd, double degreesOfFreedom)
            {
                ua = uniformLowerBound;
                ub = uniformUpperBound;
                df = degreesOfFreedom;
                tm = mean;
                ts = tStd;

                _base = new TDistribution(df);
                _mean = (ua + ub) / 2d + tm;
                _variance = Math.Pow(ts, 2) * df / (df - 2) + Math.Pow(ub - ua, 2) / 12d;
            }

            public StudentUniformDistribution(double n, double degreesOfFreedom)
            {
                ts = Math.Sqrt(1d / (Math.Pow(n, 2) + 1d));
                double a = n * ts * Math.Sqrt(3);

                df = degreesOfFreedom;
                ua = -a;
                ub = a;
                tm = 0;

                _base = new TDistribution(df);
                _mean = 0;
                _variance = Math.Pow(ts, 2) * df / (df - 2) + Math.Pow(a, 2) / 3d;
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return 1d / (ub - ua) * (_base.DistributionFunction((x - tm - ua) / ts) - _base.DistributionFunction((x - tm - ub) / ts));
            }

            protected override double InnerDistributionFunction(double x)
            {
                return 1d / (ub - ua) * (IntegralFunction((x - tm - ua) / ts) - IntegralFunction((x - tm - ub) / ts));
            }

            private double IntegralFunction(double x)
            {
                return ts * (x * _base.DistributionFunction(x) + _base.ProbabilityDensityFunction(x) * (df + Math.Pow(x, 2)) / (df - 1d));
            }

            public override double Mean
            {
                get
                {
                    return _mean;
                }
            }

            public override double Variance
            {
                get
                {
                    return _variance;
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
                    return _range;
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
        }
    }
}
