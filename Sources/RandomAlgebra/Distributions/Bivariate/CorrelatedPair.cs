using System;
using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.CustomDistributions;

namespace RandomAlgebra.Distributions
{
    /// <summary>
    /// Pair of multivariate distribution random variables and correlation between them.
    /// </summary>
    public class CorrelatedPair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelatedPair"/> class by distributions and correlation between them.
        /// </summary>
        /// <param name="left">1-st distribution.</param>
        /// <param name="right">2-nd distribution.</param>
        /// <param name="rho">Correlation coefficient.</param>
        public CorrelatedPair(BaseDistribution left, BaseDistribution right, double rho)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            if (left.DistributionType != DistributionType.Continious || right.DistributionType != DistributionType.Continious)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ForCorrelationPairBothOfDistributionsMustBeContinuous);
            }

            BaseLeft = (ContinuousDistribution)left;
            BaseRight = (ContinuousDistribution)right;
            Correlation = rho;
        }

        /// <summary>
        /// 1-st distribution.
        /// </summary>
        public ContinuousDistribution BaseLeft { get; }

        /// <summary>
        /// 2-nd distribution.
        /// </summary>
        public ContinuousDistribution BaseRight { get; }

        /// <summary>
        /// Correlation coefficient.
        /// </summary>
        public double Correlation { get; }

        internal bool Used { get; set; }

        /// <summary>
        /// Creates <see cref="BivariateContinuousDistribution"/> instance.
        /// </summary>
        /// <returns><see cref="BivariateContinuousDistribution"/> instance.</returns>
        public BivariateContinuousDistribution GetBivariate()
        {
            var contLeft = BaseLeft;
            var contRight = BaseRight;

            var samples = Math.Max(contLeft.Samples, contRight.Samples);

            if (contLeft.BaseDistribution is NormalDistribution && contRight.BaseDistribution is NormalDistribution)
            {
                return new BivariateNormalDistribution(contLeft.Mean, contRight.Mean, contLeft.StandardDeviation, contRight.StandardDeviation, Correlation * Math.Sign(contRight.Coefficient), samples);
            }
            else if (contLeft.BaseDistribution is StudentGeneralizedDistribution && contRight.BaseDistribution is StudentGeneralizedDistribution)
            {
                var leftT = (StudentGeneralizedDistribution)contLeft.BaseDistribution;
                var rightT = (StudentGeneralizedDistribution)contRight.BaseDistribution;

                if (leftT.DegreesOfFreedom != rightT.DegreesOfFreedom)
                {
                    throw new DistributionsArgumentException(DistributionsArgumentExceptionType.BivariateTDistributionMustHaveSameDegreesOfFreedom);
                }

                return new BivariateTDistribution(contLeft.Mean, contRight.Mean, leftT.ScaleCoefficient * contLeft.Coefficient, rightT.ScaleCoefficient * contRight.Coefficient, Correlation * Math.Sign(contRight.Coefficient), leftT.DegreesOfFreedom, samples);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
