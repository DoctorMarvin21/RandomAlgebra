using System;
using Accord;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions
{
    namespace CustomDistributions
    {
        internal class ChiDistribution : UnivariateContinuousDistribution
        {
            private readonly double mean;
            private readonly double variance;
            private readonly DoubleRange support = new DoubleRange(0, double.PositiveInfinity);

            public ChiDistribution(int degreesOfFreedom)
            {
                DegreesOfFreedom = degreesOfFreedom;

                mean = Math.Sqrt(2) * Accord.Math.Gamma.Function((DegreesOfFreedom + 1) / 2d) / Accord.Math.Gamma.Function(DegreesOfFreedom / 2d);
                variance = DegreesOfFreedom - Math.Pow(mean, 2);
            }

            public override double Mean => mean;

            public override double Variance => variance;

            public int DegreesOfFreedom { get; }

            public override double Entropy => throw new NotImplementedException();

            public override DoubleRange Support => support;

            public override object Clone()
            {
                throw new NotImplementedException();
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return ToString();
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                double k = DegreesOfFreedom;

                double a = Math.Pow(x, k - 1) * Math.Exp(-Math.Pow(x, 2) / 2d);
                double b = Math.Pow(2, (k / 2d) - 1) * Accord.Math.Gamma.Function(k / 2d);

                return a / b;
            }

            protected override double InnerDistributionFunction(double x)
            {
                double a = DegreesOfFreedom / 2d;
                double b = Math.Pow(x, 2) / 2d;

                return Accord.Math.Gamma.LowerIncomplete(a, b);
            }
        }
    }
}
