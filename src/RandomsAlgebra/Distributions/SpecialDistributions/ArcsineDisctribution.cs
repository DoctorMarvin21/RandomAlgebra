using Accord;
using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomsAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class ArcsineDistribution : UnivariateContinuousDistribution
        {
            readonly double _mean;
            readonly double _variance;
            readonly DoubleRange _support = new DoubleRange(0, 1);

            public ArcsineDistribution() : this(0, 1)
            {

            }

            public ArcsineDistribution(double a, double b)
            {
                LowerBound = a;
                UpperBound = b;

                _support = new DoubleRange(a, b);

                _mean = (a + b) / 2d;
                _variance = 0.125 * Math.Pow(b - a, 2);
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

            public double LowerBound
            {
                get;
            }

            public double UpperBound
            {
                get;
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
                    return _support;
                }
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return 1d / (Math.PI * Math.Sqrt((x - LowerBound) * (UpperBound - x)));

            }

            protected override double InnerDistributionFunction(double x)
            {
                return 2d / Math.PI * Math.Asin(Math.Sqrt((x - LowerBound) / (UpperBound - LowerBound)));
            }

            public override object Clone()
            {
                return new ArcsineDistribution();
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return ToString();
            }
        }
    }
}
