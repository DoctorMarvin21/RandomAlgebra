﻿using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.SpecialDistributions;
using System;

namespace RandomAlgebra.Distributions
{
    public class CorrelatedPair
    {
        public CorrelatedPair(BaseDistribution left, BaseDistribution right, double rho)
        {
            if (left == null)
                throw new ArgumentNullException(nameof(left));
            if (right == null)
                throw new ArgumentNullException(nameof(right));

            if (left.DistributionType != DistributionType.Continious || right.DistributionType != DistributionType.Continious)
                throw new DistributionsArgumentException("For correlation pair both if distributions must be continuous", "Для коррелирующей пары оба распределения должны быть непрерывными");

            BaseLeft = (ContinuousDistribution)left;
            BaseRight = (ContinuousDistribution)right;
            Correlation = rho;
        }

        internal bool CheckDistributions(BaseDistribution left, BaseDistribution right)
        {
            var leftCont = left as ContinuousDistribution;
            var rightCont = right as ContinuousDistribution;

            if (BaseLeft.BaseDistribution == leftCont?.BaseDistribution && BaseRight.BaseDistribution == rightCont?.BaseDistribution)
            {
                return true;
            }
            else if (BaseLeft.BaseDistribution == rightCont?.BaseDistribution && BaseRight.BaseDistribution == leftCont?.BaseDistribution)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public ContinuousDistribution BaseLeft
        {
            get;
        }

        public ContinuousDistribution BaseRight
        {
            get;
        }

        public double Correlation
        {
            get;
        }

        internal bool Used
        {
            get;
            set;
        }

        internal BivariateContinuousDistribution GetBivariate(BaseDistribution left, BaseDistribution right)
        {
            if (CheckDistributions(left, right))
            {
                Used = true;

                var contLeft = (ContinuousDistribution)left;
                var contRight = (ContinuousDistribution)right;

                var samples = Math.Max(contLeft.Samples, contRight.Samples);

                if (contLeft.BaseDistribution is NormalDistribution && contRight.BaseDistribution is NormalDistribution)
                {
                    return new BivariateNormalDistribution(left.Mean, right.Mean, left.StandardDeviation, right.StandardDeviation, Correlation, samples);
                }
                else if (contLeft.BaseDistribution is StudentGeneralizedDistribution && contRight.BaseDistribution is StudentGeneralizedDistribution)
                {
                    var leftT = (StudentGeneralizedDistribution)contLeft.BaseDistribution;
                    var rightT = (StudentGeneralizedDistribution)contRight.BaseDistribution;

                    if (leftT.DegreesOfFreedom != rightT.DegreesOfFreedom)
                        throw new ArgumentException();

                    return new BivariateTDistribution(left.Mean, right.Mean, leftT.ScaleCoefficient * contLeft.Coefficient, rightT.ScaleCoefficient * contRight.Coefficient, Correlation, leftT.DegreesOfFreedom, samples);
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else
            {
                throw new DistributionsInvalidOperationException();
            }
        }
    }
}
