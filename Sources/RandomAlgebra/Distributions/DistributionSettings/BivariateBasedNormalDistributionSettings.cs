using System;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Sum of two correlated normal distributions.
    /// </summary>
    public class BivariateBasedNormalDistributionSettings : DistributionSettings
    {
        private double standardDeviation1 = 1;
        private double standardDeviation2 = 1;
        private double correlation = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="BivariateBasedNormalDistributionSettings"/> class.
        /// Base constructor that creates sum of two not correlated standard normal distributions.
        /// </summary>
        public BivariateBasedNormalDistributionSettings()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BivariateBasedNormalDistributionSettings"/> class.
        /// Creates instance of sum of two correlated normal distributions with exact parameters.
        /// </summary>
        /// <param name="mean1">Expected value of 1-st normal distribution.</param>
        /// <param name="mean2">Expected value of 2-nd normal distribution.</param>
        /// <param name="std1">Standard deviation of 1-st normal distribution.</param>
        /// <param name="std2">Standard deviation of 2-nd normal distribution.</param>
        /// <param name="correlation">Correlation between first and second normal distributions.</param>
        public BivariateBasedNormalDistributionSettings(double mean1, double mean2, double std1, double std2, double correlation)
        {
            Mean1 = mean1;
            Mean2 = mean2;
            standardDeviation1 = std1;
            standardDeviation2 = std2;
            this.correlation = correlation;

            CheckParameters();
        }

        /// <summary>
        /// Expected value of 1-st normal distribution.
        /// </summary>
        public double Mean1 { get; set; }

        /// <summary>
        /// Expected value of 2-nd normal distribution.
        /// </summary>
        public double Mean2 { get; set; }

        /// <summary>
        /// Standard deviation of 1-st normal distribution.
        /// </summary>
        public double StandardDeviation1
        {
            get => standardDeviation1;
            set
            {
                standardDeviation1 = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Standard deviation of 2-nd normal distribution.
        /// </summary>
        public double StandardDeviation2
        {
            get => standardDeviation2;
            set
            {
                standardDeviation2 = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Correlation between first and second normal distributions.
        /// </summary>
        public double Correlation
        {
            get => correlation;
            set
            {
                correlation = value;
                CheckParameters();
            }
        }

        /// <summary>
        /// Expected value of sum of correlated normal distributions.
        /// </summary>
        public double TotalMean => Mean1 + Mean2;

        /// <summary>
        /// Standard deviation of sum of correlated normal distributions.
        /// </summary>
        public double TotalStandardDeviation
        {
            get
            {
                return Math.Sqrt(Math.Pow(StandardDeviation1, 2) + Math.Pow(StandardDeviation2, 2) +
                    (2 * Correlation * StandardDeviation1 * StandardDeviation2));
            }
        }

        public override string ToString()
        {
            return $"ρ = {Correlation}; μ = {TotalMean}; σ = {TotalStandardDeviation}";
        }

        internal override UnivariateContinuousDistribution GetUnivariateContinuousDistribution()
        {
            return new NormalDistribution(TotalMean, TotalStandardDeviation);
        }

        protected override void CheckParameters()
        {
            if (standardDeviation1 <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.StandardDeviationOfFirstDistributionMustBeGreaterThenZero);
            }
            if (standardDeviation2 <= 0)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.StandardDeviationOfSecondDistributionMustBeGreaterThenZero);
            }
            if (correlation <= -1 || correlation >= 1)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.CorrelationMustBeInRangeFromMinusOneToOne);
            }
        }
    }
}
