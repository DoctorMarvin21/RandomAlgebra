using System;
using Accord.Math;
using Accord.Math.Decompositions;
using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.Settings;

namespace RandomAlgebra.Distributions
{
    internal class MultivariateDistributionRandomSource
    {
        private readonly MultivariateDistributionSettings settings;
        private readonly CholeskyDecomposition chol;
        private readonly UnivariateContinuousDistribution baseDistribution;

        public MultivariateDistributionRandomSource(MultivariateDistributionSettings settings)
        {
            this.settings = settings;
            baseDistribution = settings.BaseSettings.GetUnivariateContinuousDistribution();

            chol = new CholeskyDecomposition(settings.CovarianceMatrix);

            if (!chol.IsPositiveDefinite)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CovarianceMatrixMustBePositiveDefined);
            }
        }

        public double[] GenerateRandom(Random rnd)
        {
            double[] result = new double[settings.Dimension];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = baseDistribution.Generate(rnd);
            }

            result = Matrix.Dot(chol.LeftTriangularFactor, result);
            result = Elementwise.Add(result, settings.Means);

            return result;
        }
    }
}
