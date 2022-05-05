using System;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Multivariate t distribution settings that generates random variables vector.
    /// </summary>
    public class MultivariateTDistributionSettings : MultivariateDistributionSettings
    {
        private static readonly NormalDistribution BaseNormal = new NormalDistribution();
        private readonly GammaDistribution baseGamma;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateTDistributionSettings"/> class with 2 dimensions,
        /// zero means, covariance matrix is [{1,0}, {0, 1}] and degrees of freedom is 10.
        /// </summary>
        public MultivariateTDistributionSettings()
            : base(2)
        {
            DegreesOfFreedom = 10;
            baseGamma = new GammaDistribution(2.0, 0.5 * DegreesOfFreedom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateTDistributionSettings"/> class
        /// by measured value with degrees of freedom N-1.
        /// </summary>
        /// <param name="input">Matrix of measured values.</param>
        public MultivariateTDistributionSettings(double[,] input)
            : this(input, input.GetLength(1) - 1)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateTDistributionSettings"/> class
        /// by measured value with degrees of freedom N-1.
        /// </summary>
        /// <param name="input">Matrix of measured values.</param>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public MultivariateTDistributionSettings(double[,] input, double degreesOfFreedom)
            : base(input)
        {
            DegreesOfFreedom = degreesOfFreedom;
            baseGamma = new GammaDistribution(2.0, 0.5 * degreesOfFreedom);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateTDistributionSettings"/> class
        /// by means vector, covariance matrix and degrees of freedom.
        /// </summary>
        /// <param name="means">Means vector.</param>
        /// <param name="covarianceMatrix">Covariance matrix.</param>
        /// <param name="degreesOfFreedom">Degrees of freedom.</param>
        public MultivariateTDistributionSettings(double[] means, double[,] covarianceMatrix, double degreesOfFreedom)
            : base(means, covarianceMatrix)
        {
            DegreesOfFreedom = degreesOfFreedom;
            baseGamma = new GammaDistribution(2.0, 0.5 * degreesOfFreedom);
        }

        /// <summary>
        /// Degrees of freedom applied to every dimension.
        /// </summary>
        public double DegreesOfFreedom { get; }

        protected override double[] GenerateRandomInternal(Random rnd)
        {
            double[,] ltf = Chol.LeftTriangularFactor;
            double[] result = new double[Dimension];
            double[] u = Means;

            for (int i = 0; i < Dimension; i++)
            {
                result[i] = BaseNormal.Generate(rnd);
            }

            result = Matrix.Dot(ltf, result);
            double[] gammaSqrt = baseGamma.Generate(Dimension, rnd).Divide(DegreesOfFreedom).Sqrt();

            result = Elementwise.Divide(result, gammaSqrt);
            result = Elementwise.Add(result, u);

            return result;
        }

        protected override CorrelatedPair GetBivariatePairInternal(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
            return new CorrelatedPair(
                new StudentGeneralizedDistributionSettings(mean1, sigma1, DegreesOfFreedom).GetDistribution(samples),
                new StudentGeneralizedDistributionSettings(mean2, sigma2, DegreesOfFreedom).GetDistribution(samples),
                rho);
        }
    }
}
