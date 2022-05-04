using System;
using Accord.Statistics.Distributions.Univariate;

namespace RandomAlgebra.Distributions
{
    /// <summary>
    /// Bivariate normal distribution.
    /// </summary>
    public class BivariateNormalDistribution : BivariateContinuousDistribution
    {
        private readonly double supportMinLeft;
        private readonly double supportMaxLeft;
        private readonly double supportMinRight;
        private readonly double supportMaxRight;
        private readonly double k;
        private readonly double e;

        private readonly double vInv1;
        private readonly double vInv2;
        private readonly double sProdInv;

        /// <summary>
        /// Initializes a new instance of the <see cref="BivariateNormalDistribution"/> class by distribution parameters.
        /// </summary>
        /// <param name="mean1">Expected value of 1-st distribution.</param>
        /// <param name="mean2">Expected value of 2-nd distribution.</param>
        /// <param name="sigma1">Standard deviation of 1-st distribution.</param>
        /// <param name="sigma2">Standard deviation of 2-nd distribution.</param>
        /// <param name="rho">Correlation between first and second distributions.</param>
        /// <param name="samples">Samples count used for discretization.</param>
        public BivariateNormalDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
            : base(mean1, mean2, sigma1, sigma2, rho, samples)
        {
            ContinuousDistribution left = new ContinuousDistribution(new NormalDistribution(Mean1, Sigma1), samples);
            ContinuousDistribution right = new ContinuousDistribution(new NormalDistribution(Mean2, Sigma2), samples);

            supportMinLeft = left.MinX;
            supportMaxLeft = left.MaxX;
            supportMinRight = right.MinX;
            supportMaxRight = right.MaxX;

            k = 1d / (2d * Math.PI * sigma1 * sigma2 * Math.Sqrt(1 - Math.Pow(rho, 2)));
            e = -1d / (2d * (1 - Math.Pow(rho, 2)));

            vInv1 = 1d / Variance1;
            vInv2 = 1d / Variance2;
            sProdInv = 1d / (Sigma1 * Sigma2);
        }

        public override double SupportMinLeft => supportMinLeft;

        public override double SupportMaxLeft => supportMaxLeft;

        public override double SupportMinRight => supportMinRight;

        public override double SupportMaxRight => supportMaxRight;

        public override BivariateContinuousDistribution Rotate()
        {
            return new BivariateNormalDistribution(Mean2, Mean1, Sigma2, Sigma1, Correlation, Samples);
        }

        public override BaseDistribution GetSum()
        {
            return new Settings.BivariateBasedNormalDistributionSettings(Mean1, Mean2, Sigma1, Sigma2, Correlation).GetDistribution(Samples);
        }

        public override BaseDistribution GetDifference()
        {
            return new Settings.BivariateBasedNormalDistributionSettings(Mean1, -Mean2, Sigma1, Sigma2, -Correlation).GetDistribution(Samples);
        }

        protected override double InnerProbabilityDensityFunction(double x, double y)
        {
            double p1 = Math.Pow(x - Mean1, 2) * vInv1;
            double p2 = Math.Pow(y - Mean2, 2) * vInv2;
            double p3 = 2 * Correlation * (x - Mean1) * (y - Mean2) * sProdInv;

            return k * Math.Exp(e * (p1 + p2 - p3));
        }
    }
}
