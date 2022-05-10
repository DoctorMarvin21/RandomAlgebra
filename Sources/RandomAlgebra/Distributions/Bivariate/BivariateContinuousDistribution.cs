using System;

namespace RandomAlgebra.Distributions
{
    // TODO: rewrite all this to multivariate distribution from Accord.NET

    /// <summary>
    /// Base class of bivariate continuous distributions.
    /// </summary>
    public abstract class BivariateContinuousDistribution
    {
        protected BivariateContinuousDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
            if (sigma1 <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.StandardDeviationOfFirstDistributionMustBeGreaterThenZero);
            }
            if (sigma2 <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.StandardDeviationOfSecondDistributionMustBeGreaterThenZero);
            }
            if (rho <= -1 || rho >= 1)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CorrelationMustBeInRangeFromMinusOneToOne);
            }

            Mean1 = mean1;
            Mean2 = mean2;
            Sigma1 = sigma1;
            Sigma2 = sigma2;

            Variance1 = Math.Pow(sigma1, 2);
            Variance2 = Math.Pow(Sigma2, 2);
            Correlation = rho;
            Samples = samples;
        }

        /// <summary>
        /// Expected value of 1-st distribution.
        /// </summary>
        public double Mean1 { get; }

        /// <summary>
        /// Expected value of 2-nd distribution.
        /// </summary>
        public double Mean2 { get; }

        /// <summary>
        /// Standard deviation of 1-st distribution.
        /// </summary>
        public double Sigma1 { get; }

        /// <summary>
        /// Standard deviation of 2-nd distribution.
        /// </summary>
        public double Sigma2 { get; }

        /// <summary>
        /// Variance of 1-st distribution.
        /// </summary>
        public double Variance1 { get; }

        /// <summary>
        /// Variance of 2-nd distribution.
        /// </summary>
        public double Variance2 { get; }

        /// <summary>
        /// Correlation between first and second distributions.
        /// </summary>
        public double Correlation { get; }

        /// <summary>
        /// Samples count used for discretization.
        /// </summary>
        public int Samples { get; }

        /// <summary>
        /// Minimum value of the support of 1-st distribution.
        /// </summary>
        public abstract double SupportMinLeft { get; }

        /// <summary>
        /// Maximum value of the support of 1-st distribution.
        /// </summary>
        public abstract double SupportMaxLeft { get; }

        /// <summary>
        /// Minimum value of the support of 2-nd distribution.
        /// </summary>
        public abstract double SupportMinRight { get; }

        /// <summary>
        /// Maximum value of the support of 2-nd distribution.
        /// </summary>
        public abstract double SupportMaxRight { get; }

        /// <summary>
        /// Creates rotated <see cref="BivariateContinuousDistribution"/>.
        /// </summary>
        /// <returns>Rotated <see cref="BivariateContinuousDistribution"/></returns>
        public abstract BivariateContinuousDistribution Rotate();

        /// <summary>
        /// Returns probability density by 2 dimensions.
        /// </summary>
        /// <param name="x">Coordinate of 1-st dimension.</param>
        /// <param name="y">Coordinate of 2-nd dimension.</param>
        /// <returns>Probability density.</returns>
        public abstract double ProbabilityDensityFunction(double x, double y);

        /// <summary>
        /// Sum (convolution) of correlated distributions.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Sum result.</returns>
        public virtual BaseDistribution GetSum()
        {
            return BivariateMath.GetSum(this);
        }

        /// <summary>
        /// Difference between correlated distributions.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Difference result.</returns>
        public virtual BaseDistribution GetDifference()
        {
            return BivariateMath.GetDifference(this);
        }

        /// <summary>
        /// Correlated distributions product.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Product result.</returns>
        public virtual BaseDistribution GetProduct()
        {
            return BivariateMath.GetProduct(this);
        }

        /// <summary>
        /// Correlated distributions ratio.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Ratio result.</returns>
        public virtual BaseDistribution GetRatio()
        {
            return BivariateMath.GetRatio(this);
        }

        /// <summary>
        /// Returns 1-st distribution in power of 2-nd distribution.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Power result.</returns>
        public virtual BaseDistribution GetPower()
        {
            return BivariateMath.GetPower(this);
        }

        /// <summary>
        /// Returns logarithm of 1-st distribution with base of 2-nd distribution.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <returns>Power result.</returns>
        public virtual BaseDistribution GetLog()
        {
            return BivariateMath.GetLog(this);
        }
    }
}
