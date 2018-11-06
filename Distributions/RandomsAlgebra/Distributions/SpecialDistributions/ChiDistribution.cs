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
        internal class ChiDistribution : UnivariateContinuousDistribution
        {
            readonly double _mean;
            readonly double _variance;
            readonly DoubleRange _support = new DoubleRange(0, double.PositiveInfinity);


            public ChiDistribution(int degreesOfFreedom)
            {
                DegreesOfFreedom = degreesOfFreedom;

                _mean = Math.Sqrt(2) * Accord.Math.Gamma.Function((DegreesOfFreedom + 1) / 2d) / Accord.Math.Gamma.Function(DegreesOfFreedom / 2d);
                _variance = DegreesOfFreedom - Math.Pow(_mean, 2);
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

            public int DegreesOfFreedom
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
                double k = DegreesOfFreedom;

                double a = Math.Pow(x, k - 1) * Math.Exp(-Math.Pow(x, 2) / 2d);
                double b = Math.Pow(2, k / 2d - 1) * Accord.Math.Gamma.Function(k / 2d);

                return a / b; 
            }

            protected override double InnerDistributionFunction(double x)
            {
                double a = DegreesOfFreedom / 2d;
                double b = Math.Pow(x, 2) / 2d;

                return Accord.Math.Gamma.LowerIncomplete(a, b);
            }

            public override object Clone()
            {
                return new ChiDistribution(DegreesOfFreedom);
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return ToString();
            }
        }
    }
}
