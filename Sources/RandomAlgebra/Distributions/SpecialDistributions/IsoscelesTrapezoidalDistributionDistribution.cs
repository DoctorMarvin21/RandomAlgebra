using Accord;
using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class IsoscelesTrapezoidalDistribution : UnivariateContinuousDistribution
        {
            readonly double _a, _b, _c, _d, _r, _s, _h;
            readonly DoubleRange _support;
            public IsoscelesTrapezoidalDistribution(double a, double b, double r)
            {
                _a = a;
                _b = b;
                _c = a + r;
                _r = r;
                _s = b - a - 2 * r;
                //2 * r + s = b - a   s = b - a - 2r;
                _d = _c + _s;
                _h = 1d / (b - a - r);
                _support = new DoubleRange(_a, _b);
            }

            public override double Mean
            {
                get
                {
                    return (_a + _b) / 2d;
                }
            }

            public override double Variance
            {
                get
                {
                    return (2 * Math.Pow(_r, 2) + 2 * _r * _s + Math.Pow(_s, 2)) / 12;
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
                    return _support;
                }
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                if (x >= _a && x < _c)
                {
                    return (x - _a) / _r * _h;
                }
                else if (x >= _c && x < _d)
                {
                    return _h;
                }
                else if (x >= _d && x < _b)
                {
                    return (_b - x) / _r * _h;
                }
                else
                {
                    return 0;
                }
            }


            protected override double InnerDistributionFunction(double x)
            {
                if (x >= _a && x < _c)
                {
                    return Math.Pow(x - _a, 2) / (2 * _r) * _h;
                }
                else if (x >= _c && x < _d)
                {
                    return _h / 2 * _r + _h * (x - _c);
                }
                else if (x >= _d && x < _b)
                {
                    return 1 - Math.Pow(_b - x, 2) / (2 * _r) * _h;
                }
                else
                {
                    return 0;
                }
            }
            public override object Clone()
            {
                throw new NotImplementedException();
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return String.Empty;
            }
        }
    }
}
