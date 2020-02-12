using System;
using Accord;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class GammaCorrectedDistribution : UnivariateContinuousDistribution
        {
            private readonly GammaDistribution baseGamma;
            private readonly DoubleRange range = new DoubleRange(double.NegativeInfinity, double.PositiveInfinity);
            private readonly double theta;
            private readonly double k;

            public GammaCorrectedDistribution(double theta, double k)
            {
                baseGamma = new GammaDistribution(theta, k);
                this.theta = theta;
                this.k = k;
            }

            public override double Mean
            {
                get
                {
                    return baseGamma.Mean;
                }
            }

            public override double Variance
            {
                get
                {
                    return baseGamma.Variance;
                }
            }

            public override double Entropy
            {
                get
                {
                    return baseGamma.Entropy;
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
                throw new NotImplementedException();
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return baseGamma.ToString(format, formatProvider);
            }

            public override double[] Generate(int samples, double[] result, Random source)
            {
                return baseGamma.Generate(samples, result, source);
            }

            public override double Generate(Random source)
            {
                return baseGamma.Generate(source);
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return 1d / (Gamma.Function(k) * Math.Pow(theta, k)) * Math.Pow(x, k - 1) * Math.Exp(-x / theta);
            }

            protected override double InnerDistributionFunction(double x)
            {
                return baseGamma.DistributionFunction(x);
            }
        }
    }
}
