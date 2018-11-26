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
        internal class GammaCorrectedDistribution : UnivariateContinuousDistribution
        {
            readonly GammaDistribution _baseGamma;
            readonly DoubleRange _range = new DoubleRange(double.NegativeInfinity, double.PositiveInfinity);
            readonly double _theta;
            readonly double _k;

            /// <summary>
            ///   Constructs a Gamma distribution.
            /// </summary>
            /// 
            /// <param name="theta">The scale parameter θ (theta). Default is 1.</param>
            /// <param name="k">The shape parameter k. Default is 1.</param>
            public GammaCorrectedDistribution(double theta, double k)
            {
                _baseGamma = new GammaDistribution(theta, k);
                _theta = theta;
                _k = k;
            }


            public override double[] Generate(int samples, double[] result, Random source)
            {
                return _baseGamma.Generate(samples, result, source);
            }

            public override double Generate(Random source)
            {
                return _baseGamma.Generate(source);
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return 1d / (Gamma.Function(_k) * Math.Pow(_theta, _k)) * Math.Pow(x, _k - 1) * Math.Exp(-x / _theta);
            }

            protected override double InnerDistributionFunction(double x)
            {
                return _baseGamma.DistributionFunction(x);
            }

            public override double Mean
            {
                get
                {
                    return _baseGamma.Mean;
                }
            }

            public override double Variance
            {
                get
                {
                    return _baseGamma.Variance;
                }
            }

            public override double Entropy
            {
                get
                {
                    return _baseGamma.Entropy;
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
                return _baseGamma.ToString(format, formatProvider);
            }
        }
    }
}
