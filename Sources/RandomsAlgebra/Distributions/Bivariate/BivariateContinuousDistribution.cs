using RandomAlgebra.Distributions.SpecialDistributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    internal abstract class BivariateContinuousDistribution//TODO: make it public, create documentation and random generator
    {
        public BivariateContinuousDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
            if (sigma1 <= 0)
            {
                throw new DistributionsArgumentException("Standard deviation of 1-st distribution must be greater then zero", "Стандартное отклонение 1 должно быть больше 0");
            }
            if (sigma2 <= 0)
            {
                throw new DistributionsArgumentException("Standard deviation of 2-nd distribution must be greater then zero", "Стандартное отклонение 2 должно быть больше 0");
            }
            if (rho <= -1 || rho >= 1)
            {
                throw new DistributionsArgumentException("Correlation must be in range (-1, 1)", "Коэффициент корреляции должен быть в пределах (-1, 1)");
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected abstract double InnerProbabilityDensityFunction(double x, double y);

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

        public virtual BaseDistribution GetLog()
        {
            return BivariateMath.GetLog(this);//TODO?
        }

        public virtual BaseDistribution GetPower()
        {
            return BivariateMath.GetPower(this);//TODO?
        }
    }

    internal class BivariateNormalDistribution : BivariateContinuousDistribution
    {
        readonly double supportMinLeft;
        readonly double supportMaxLeft;
        readonly double supportMinRight;
        readonly double supportMaxRight;
        readonly double k;
        readonly double e;

        public BivariateNormalDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples) : base(mean1, mean2, sigma1, sigma2, rho, samples)
        {
            ContinuousDistribution left = new ContinuousDistribution(new Accord.Statistics.Distributions.Univariate.NormalDistribution(Mean1, Sigma1), samples);
            ContinuousDistribution right = new ContinuousDistribution(new Accord.Statistics.Distributions.Univariate.NormalDistribution(Mean2, Sigma2), samples);

            supportMinLeft = left.MinX;
            supportMaxLeft = left.MaxX;
            supportMinRight = right.MinX;
            supportMaxRight = right.MaxX;

            k = 1d / (2d * Math.PI * sigma1 * sigma2 * Math.Sqrt(1 - Math.Pow(rho, 2)));
            e = -1d / (2d * (1 - Math.Pow(rho, 2)));
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
            double p1 = Math.Pow(x - Mean1, 2) / Variance1;
            double p2 = Math.Pow(y - Mean2, 2) / Variance2;
            double p3 = 2 * Correlation * (x - Mean1) * (y - Mean2) / (Sigma1 * Sigma2);

            return k * Math.Exp(e * (p1 + p2 - p3));
        }
    }

    internal class BivariateTDistribution : BivariateContinuousDistribution
    {
        readonly double supportMinLeft;
        readonly double supportMaxLeft;
        readonly double supportMinRight;
        readonly double supportMaxRight;
        readonly double k;
        readonly double d;
        readonly double p;

        public BivariateTDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, double degreesOfFreedom, int samples) : base(mean1, mean2, sigma1, sigma2, rho, samples)
        {
            ContinuousDistribution left = new ContinuousDistribution(new StudentGeneralizedDistribution(mean1, sigma1, degreesOfFreedom), samples);
            ContinuousDistribution right = new ContinuousDistribution(new StudentGeneralizedDistribution(mean2, sigma2, degreesOfFreedom), samples);

            supportMinLeft = left.MinX;
            supportMaxLeft = left.MaxX;
            supportMinRight = right.MinX;
            supportMaxRight = right.MaxX;

            k = 1d / (2d * Math.PI * sigma1 * sigma2 * Math.Sqrt(1 - Math.Pow(rho, 2)));
            d = degreesOfFreedom * (1d - Math.Pow(rho, 2));
            p = -(degreesOfFreedom + 2d) / 2d;
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

        protected override double InnerProbabilityDensityFunction(double x, double y)
        {
            double s = Math.Pow(x - Mean1, 2) / Variance1 + Math.Pow(y - Mean2, 2) / Variance2 - 2 * Correlation * (x - Mean1) * (y - Mean2) / (Sigma1 * Sigma2);
            return k * Math.Pow(1 + s / d, p);
        }
    }
}
