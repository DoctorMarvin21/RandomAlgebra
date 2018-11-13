using RandomAlgebra.Distributions.SpecialDistributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    internal abstract class BivariateContinuousDistribution//TODO: make it public, create documentation and random generator
    {
        public BivariateContinuousDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples)
        {
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

        public abstract double ProbabilityDensityFunction(double x, double y);

        public abstract double[] SupportLeft//array of four values
        {
            get;
        }

        public abstract double[] SupportRight//array of four values
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
        readonly double[] supportLeft;
        readonly double[] supportRight;
        readonly double k;
        readonly double e;

        public BivariateNormalDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, int samples) : base(mean1, mean2, sigma1, sigma2, rho, samples)
        {
            ContinuousDistribution left = new ContinuousDistribution(new Accord.Statistics.Distributions.Univariate.NormalDistribution(Mean1, Sigma1), samples);
            ContinuousDistribution right = new ContinuousDistribution(new Accord.Statistics.Distributions.Univariate.NormalDistribution(Mean2, Sigma2), samples);

            supportLeft = new double[] { left.MinX, left.MaxX };
            supportRight = new double[] { right.MinX, right.MaxX };

            k = 1d / (2d * Math.PI * sigma1 * sigma2 * Math.Sqrt(1 - Math.Pow(rho, 2)));
            e = -1d / (2d * (1 - Math.Pow(rho, 2)));
        }

        public override double[] SupportLeft
        {
            get
            {
                return supportLeft;
            }
        }

        public override double[] SupportRight
        {
            get
            {
                return supportRight;
            }
        }

        public override double ProbabilityDensityFunction(double x, double y)
        {
            double p1 = Math.Pow(x - Mean1, 2) / Variance1;
            double p2 = Math.Pow(y - Mean2, 2) / Variance2;
            double p3 = 2 * Correlation * (x - Mean1) * (y - Mean2) / (Sigma1 * Sigma2);

            return k * Math.Exp(e * (p1 + p2 - p3));
        }
    }

    internal class BivariateTDistribution : BivariateContinuousDistribution
    {
        readonly double[] supportLeft;
        readonly double[] supportRight;
        readonly double k;
        readonly double d;
        readonly double p;

        public BivariateTDistribution(double mean1, double mean2, double sigma1, double sigma2, double rho, double degreesOfFreedom, int samples) : base(mean1, mean2, sigma1, sigma2, rho, samples)
        {
            ContinuousDistribution left = new ContinuousDistribution(new StudentGeneralizedDistribution(mean1, sigma1, degreesOfFreedom), samples);
            ContinuousDistribution right = new ContinuousDistribution(new StudentGeneralizedDistribution(mean2, sigma2, degreesOfFreedom), samples);

            supportLeft = new double[] { left.MinX, left.MaxX };
            supportRight = new double[] { right.MinX, right.MaxX };

            k = 1d / (2d * Math.PI * sigma1 * sigma2 * Math.Sqrt(1 - Math.Pow(rho, 2)));
            d = degreesOfFreedom * (1d - Math.Pow(rho, 2));
            p = -(degreesOfFreedom + 2d) / 2d;
        }

        public override double[] SupportLeft
        {
            get
            {
                return supportLeft;
            }
        }

        public override double[] SupportRight
        {
            get
            {
                return supportRight;
            }
        }

        //TODO: override sum and difference

        public override double ProbabilityDensityFunction(double x, double y)
        {
            double s = Math.Pow(x - Mean1, 2) / Variance1 + Math.Pow(y - Mean2, 2) / Variance2 - 2 * Correlation * (x - Mean1) * (y - Mean2) / (Sigma1 * Sigma2);
            return k * Math.Pow(1 + s / d, p);
        }
    }
}
