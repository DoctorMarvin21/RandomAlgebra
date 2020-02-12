using System;
using Accord;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class ArcsineDistribution : UnivariateContinuousDistribution
        {
            private readonly double mean;
            private readonly double variance;
            private readonly DoubleRange support = new DoubleRange(0, 1);

            public ArcsineDistribution()
                : this(0, 1)
            {
            }

            public ArcsineDistribution(double a, double b)
            {
                LowerBound = a;
                UpperBound = b;

                support = new DoubleRange(a, b);

                mean = (a + b) / 2d;
                variance = 0.125 * Math.Pow(b - a, 2);
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
                    return support;
                }
            }

            public override object Clone()
            {
                return new ArcsineDistribution();
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return ToString();
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return 1d / (Math.PI * Math.Sqrt((x - LowerBound) * (UpperBound - x)));
            }

            protected override double InnerDistributionFunction(double x)
            {
                return 2d / Math.PI * Math.Asin(Math.Sqrt((x - LowerBound) / (UpperBound - LowerBound)));
            }
        }
    }
}
