using System;
using Accord;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions
{
    namespace SpecialDistributions
    {
        internal class StudentGeneralizedDistribution : UnivariateContinuousDistribution
        {
            private readonly double mean, variance;
            private readonly DoubleRange range = new DoubleRange(double.NegativeInfinity, double.PositiveInfinity);
            private readonly NormalDistribution baseNormal;
            private readonly TDistribution baseT;
            private readonly GammaDistribution gammaDistr;

            public StudentGeneralizedDistribution(double degreesOfFreedom)
                : this(0, 1, degreesOfFreedom)
            {
            }

            public StudentGeneralizedDistribution(double mean, double std, double degreesOfFreedom)
            {
                ScaleCoefficient = std;
                this.mean = mean;
                DegreesOfFreedom = degreesOfFreedom;

                baseT = new TDistribution(degreesOfFreedom);
                baseNormal = new NormalDistribution(0, 1);
                gammaDistr = new GammaDistribution(2.0, 0.5 * degreesOfFreedom);

                if (degreesOfFreedom > 2)
                {
                    variance = Math.Pow(ScaleCoefficient, 2) * degreesOfFreedom / (degreesOfFreedom - 2);
                }
                else
                {
                    variance = double.NaN;
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
                    return mean;
                }
            }

            public double ScaleCoefficient { get; }

            public override double Variance
            {
                get
                {
                    return variance;
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
                    return range;
                }
            }

            public override double Generate(Random source)
            {
                double x = baseNormal.Generate(source);
                double y = gammaDistr.Generate(source);
                return (x / Math.Sqrt(y / DegreesOfFreedom) * ScaleCoefficient) + mean;
            }

            public override object Clone()
            {
                return new StudentGeneralizedDistribution(Mean, ScaleCoefficient, DegreesOfFreedom);
            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return ToString();
            }

            protected override double InnerProbabilityDensityFunction(double x)
            {
                return baseT.ProbabilityDensityFunction((x - mean) / ScaleCoefficient) / ScaleCoefficient;
            }

            protected override double InnerDistributionFunction(double x)
            {
                return baseT.DistributionFunction((x - mean) / ScaleCoefficient);
            }
        }
    }
}
