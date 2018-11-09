using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Accord;
using Accord.Statistics.Distributions.Univariate;

namespace RandomsAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class BhattacharjeeDistribution : UnivariateContinuousDistribution
        {
            private readonly double ua, ub, nm, ns;

            private readonly NormalDistribution _base;
            private readonly double _mean, _variance;
            private readonly DoubleRange _range = new DoubleRange(double.NegativeInfinity, double.PositiveInfinity);


            public BhattacharjeeDistribution(double uniformLowerBound, double uniformUpperBound, double normalMean, double normalStd)
            {
                ua = uniformLowerBound;
                ub = uniformUpperBound;
                nm = normalMean;
                ns = normalStd;

                _base = new NormalDistribution(0, 1);
                _mean = (ua + ub) / 2d + nm;
                _variance = Math.Pow(ns, 2d) + Math.Pow(ub - ua, 2) / 12d;
            }

            public BhattacharjeeDistribution(double n)
            {
                ns = Math.Sqrt(1d / (Math.Pow(n, 2) + 1d));
                double a = n * ns * Math.Sqrt(3);


                ua = -a;
                ub = a;
                nm = 0;

                _base = new NormalDistribution(0, 1);
                _mean = 0;
                _variance = Math.Pow(ns, 2d) + Math.Pow(a, 2) / 3d;
            }



            protected override double InnerProbabilityDensityFunction(double x)
            {
                return 1d / (ub - ua) * (_base.DistributionFunction((x - nm - ua) / ns) - _base.DistributionFunction((x - nm - ub) / ns));
            }

            protected override double InnerDistributionFunction(double x)
            {
                return 1d / (ub - ua) * (IntegralFunction((x - nm - ua) / ns) - IntegralFunction((x - nm - ub) / ns));
            }

            private double IntegralFunction(double x)
            {
                return ns * (x * _base.DistributionFunction(x) + _base.ProbabilityDensityFunction(x));
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
        }
    }
}
