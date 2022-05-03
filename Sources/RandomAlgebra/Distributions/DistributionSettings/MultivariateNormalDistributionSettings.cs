using System;
using System.Linq;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Multivariate normal distribution that generates random variables vector and sum of normal distributions.
    /// </summary>
    public class MultivariateNormalDistributionSettings : MultivariateDistributionSettings
    {
        private static readonly NormalDistribution BaseNormal = new NormalDistribution();

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateNormalDistributionSettings"/> class by measured values.
        /// </summary>
        /// <param name="input">Matrix of measured values.</param>
        public MultivariateNormalDistributionSettings(double[,] input)
            : base(input)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultivariateNormalDistributionSettings"/> class
        /// by means vector and covariance matrix.
        /// </summary>
        /// <param name="means">Means vector.</param>
        /// <param name="covarianceMatrix">Covariance matrix.</param>
        public MultivariateNormalDistributionSettings(double[] means, double[,] covarianceMatrix)
            : base(means, covarianceMatrix)
        {
        }

        /// <summary>
        /// Generates new normal distribution settings as a sum of correlated normal distributions with coefficients <paramref name="coeffs"/> (c1*A+c2*B+c3*C etc.)
        /// </summary>
        /// <param name="coeffs">Coefficients vector that will be applied to the sum of normal distributions.</param>
        /// <returns>Normal distribution settings instance.</returns>
        public NormalDistributionSettings GetUnivariateDistributionSettings(double[] coeffs)
        {
            if (coeffs == null)
            {
                coeffs = Vector.Ones(Dimension);
            }

            if (coeffs.Length != Dimension)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.VectorOfCoeffitientsMustBeEqualToDimension);
            }

            if (coeffs.All(x => x == 1))
            {
                return new NormalDistributionSettings(Means.Sum(), Math.Sqrt(CovarianceMatrix.Sum()));
            }

            double[,] ltf = Chol.LeftTriangularFactor;

            var weighted = Matrix.Diagonal(coeffs).Dot(ltf);
            var colSumPow = weighted.Transpose().Dot(Vector.Ones(Dimension)).Pow(2);

            double variance = colSumPow.Sum();

            double mean = Elementwise.Multiply(coeffs, Means).Sum();

            return new NormalDistributionSettings(mean, Math.Sqrt(variance));
        }

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

            result = Elementwise.Add(result, u);

            return result;
        }

        protected override CorrelatedPair GetBivariatePairInternal(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
            return new CorrelatedPair(
                new NormalDistributionSettings(mean1, sigma1).GetDistribution(samples),
                new NormalDistributionSettings(mean2, sigma2).GetDistribution(samples),
                rho);
        }
    }
}
