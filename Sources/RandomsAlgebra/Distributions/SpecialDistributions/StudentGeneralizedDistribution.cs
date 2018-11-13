using Accord;
using Accord.Statistics.Distributions.Univariate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class StudentGeneralizedDistribution : UnivariateContinuousDistribution
        {
            private readonly double _mean, _variance, _std;
            private readonly DoubleRange _range = new DoubleRange(double.NegativeInfinity, double.PositiveInfinity);
            private readonly NormalDistribution _baseNormal;
            private readonly TDistribution _baseT;
            private readonly GammaDistribution _gammaDistr;

            public StudentGeneralizedDistribution(double degreesOfFreedom) : this(0, 1, degreesOfFreedom)
            {

            }

            public StudentGeneralizedDistribution(double mean, double std, double degreesOfFreedom)
            {
                _std = std;
                _mean = mean;
                DegreesOfFreedom = degreesOfFreedom;

                _baseT = new TDistribution(degreesOfFreedom);
                _baseNormal = new NormalDistribution(0, 1);
                _gammaDistr = new GammaDistribution(2.0, 0.5 * degreesOfFreedom);

                if (degreesOfFreedom > 2)
                {
                    _variance = Math.Pow(_std, 2) * degreesOfFreedom / (degreesOfFreedom - 2);
                }
                else
                {
                    _variance = double.NaN;
                }
            }

            public double DegreesOfFreedom
            {
                get;
            }

            public override double Mean
            {
                get
                {
                    return _mean;
                }
            }

            public double ScaleCoefficient
            {
                get
                {
                    return _std;
                }
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return _baseT.ProbabilityDensityFunction((x - _mean) / _std) / _std;
            }
            protected override double InnerDistributionFunction(double x)
            {
                return _baseT.DistributionFunction((x - _mean) / _std);
            }

            public override double Generate(Random source)
            {
                double x = _baseNormal.Generate(source);
                double y = _gammaDistr.Generate(source);
                return x / Math.Sqrt(y / DegreesOfFreedom) * _std + _mean;
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
