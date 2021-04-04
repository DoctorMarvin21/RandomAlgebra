using System;
using System.Runtime.CompilerServices;
using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.SpecialDistributions;

namespace RandomAlgebra.Distributions
{
    // TODO: make it public, create documentation and random generator.
    internal abstract class BivariateContinuousDistribution
    {
        public BivariateContinuousDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
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

        public double Mean1
        {
            get;
        }

        public double Mean2
        {
            get;
        }

        public double Sigma1
        {
            get;
        }

        public double Sigma2
        {
            get;
        }

        public double Variance1
        {
            get;
        }

        public double Variance2
        {
            get;
        }

        public double Correlation
        {
            get;
        }

        public int Samples
        {
            get;
        }

        public abstract double SupportMinLeft
        {
            get;
        }

        public abstract double SupportMaxLeft
        {
            get;
        }

        public abstract double SupportMinRight
        {
            get;
        }

        public abstract double SupportMaxRight
        {
            get;
        }

        public abstract BivariateContinuousDistribution Rotate();

        public double ProbabilityDensityFunction(double x, double y)
        {
            if (x < SupportMinLeft || x > SupportMaxLeft || y < SupportMinRight || y > SupportMaxRight)
            {
                return 0;
            }
            else
            {
                return InnerProbabilityDensityFunction(x, y);
            }
        }

        public virtual BaseDistribution GetSum()
        {
            return BivariateMath.GetSum(this);
        }

        public virtual BaseDistribution GetDifference()
        {
            return BivariateMath.GetDifference(this);
        }

        public virtual BaseDistribution GetProduct()
        {
            return BivariateMath.GetProduct(this);
        }

        public virtual BaseDistribution GetRatio()
        {
            return BivariateMath.GetRatio(this);
        }

        public virtual BaseDistribution GetPower()
        {
            return BivariateMath.GetPower(this);
        }

        public virtual BaseDistribution GetLog()
        {
            return BivariateMath.GetLog(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract double InnerProbabilityDensityFunction(double x, double y);
    }

    internal class BivariateNormalDistribution : BivariateContinuousDistribution
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

        public override double SupportMinLeft
        {
            get
            {
                return supportMinLeft;
            }
        }

        public override double SupportMaxLeft
        {
            get
            {
                return supportMaxLeft;
            }
        }

        public override double SupportMinRight
        {
            get
            {
                return supportMinRight;
            }
        }

        public override double SupportMaxRight
        {
            get
            {
                return supportMaxRight;
            }
        }

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

    internal class BivariateTDistribution : BivariateContinuousDistribution
    {
        private readonly double supportMinLeft;
        private readonly double supportMaxLeft;
        private readonly double supportMinRight;
        private readonly double supportMaxRight;
        private readonly double k;
        private readonly double d;
        private readonly double p;

        private readonly double vInv1;
        private readonly double vInv2;
        private readonly double sProdInv;

        public BivariateTDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, double degreesOfFreedom, int samples)
            : base(mean1, mean2, sigma1, sigma2, rho, samples)
        {
            if (degreesOfFreedom < 1)
            {
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.DegreesOfFreedomMustNotBeLessThenOne);
            }

            DegressOfFreedom = degreesOfFreedom;

            ContinuousDistribution left = new ContinuousDistribution(new StudentGeneralizedDistribution(mean1, sigma1, degreesOfFreedom), samples);
            ContinuousDistribution right = new ContinuousDistribution(new StudentGeneralizedDistribution(mean2, sigma2, degreesOfFreedom), samples);

            supportMinLeft = left.MinX;
            supportMaxLeft = left.MaxX;
            supportMinRight = right.MinX;
            supportMaxRight = right.MaxX;

            k = 1d / (2d * Math.PI * sigma1 * sigma2 * Math.Sqrt(1 - Math.Pow(rho, 2)));
            d = degreesOfFreedom * (1d - Math.Pow(rho, 2));
            p = -(degreesOfFreedom + 2d) / 2d;

            vInv1 = 1d / Variance1;
            vInv2 = 1d / Variance2;
            sProdInv = 1d / (Sigma1 * Sigma2);
        }

        public override double SupportMinLeft
        {
            get
            {
                return supportMinLeft;
            }
        }

        public override double SupportMaxLeft
        {
            get
            {
                return supportMaxLeft;
            }
        }

        public override double SupportMinRight
        {
            get
            {
                return supportMinRight;
            }
        }

        public override double SupportMaxRight
        {
            get
            {
                return supportMaxRight;
            }
        }

        public double DegressOfFreedom
        {
            get;
        }

        public override BivariateContinuousDistribution Rotate()
        {
            return new BivariateTDistribution(Mean2, Mean1, Sigma2, Sigma1, Correlation, DegressOfFreedom, Samples);
        }

        protected override double InnerProbabilityDensityFunction(double x, double y)
        {
            double p1 = Math.Pow(x - Mean1, 2) * vInv1;
            double p2 = Math.Pow(y - Mean2, 2) * vInv2;
            double p3 = 2 * Correlation * (x - Mean1) * (y - Mean2) * sProdInv;

            return k * Math.Pow(1 + ((p1 + p2 - p3) / d), p);
        }
    }
}
