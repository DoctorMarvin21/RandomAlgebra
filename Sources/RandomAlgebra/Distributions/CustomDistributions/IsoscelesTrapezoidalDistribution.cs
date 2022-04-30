using System;
using Accord;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions
{
    namespace CustomDistributions
    {
        internal class IsoscelesTrapezoidalDistribution : UnivariateContinuousDistribution
        {
            private readonly double a, b, c, d, r, s, h;
            private readonly DoubleRange support;

            public IsoscelesTrapezoidalDistribution(double a, double b, double r)
            {
                this.a = a;
                this.b = b;
                c = a + r;
                this.r = r;
                s = b - a - (2 * r);
                d = c + s;
                h = 1d / (b - a - r);
                support = new DoubleRange(this.a, this.b);
            }

            public override double Mean => (a + b) / 2d;

            public override double Variance => ((2 * Math.Pow(r, 2)) + (2 * r * s) + Math.Pow(s, 2)) / 12;

            public override double Entropy => throw new NotImplementedException();

            public override DoubleRange Support => support;

            public override object Clone()
            {
                throw new NotImplementedException();
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return string.Empty;
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                if (x >= a && x < c)
                {
                    return (x - a) / r * h;
                }
                else if (x >= c && x < d)
                {
                    return h;
                }
                else if (x >= d && x < b)
                {
                    return (b - x) / r * h;
                }
                else
                {
                    return 0;
                }
            }

            protected override double InnerDistributionFunction(double x)
            {
                if (x >= a && x < c)
                {
                    return Math.Pow(x - a, 2) / (2 * r) * h;
                }
                else if (x >= c && x < d)
                {
                    return (h / 2 * r) + (h * (x - c));
                }
                else if (x >= d && x < b)
                {
                    return 1 - (Math.Pow(b - x, 2) / (2 * r) * h);
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}
