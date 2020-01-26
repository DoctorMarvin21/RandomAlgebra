using RandomAlgebra.Distributions;
using RandomAlgebra.Distributions.Settings;
using RandomAlgebra.DistributionsEvaluation;
using System;
using System.Collections.Generic;

namespace DistributionsWpf
{
    public static class DistributionManager
    {
        public static BaseDistribution RandomsAlgebraDistribution(string expression, Dictionary<string, DistributionSettings> univariate, Dictionary<string[], MultivariateDistributionSettings> multivariate, int samples)
        {
            Dictionary<string, BaseDistribution> arguments = new Dictionary<string, BaseDistribution>();
            List<CorrelatedPair> correlations = new List<CorrelatedPair>();

            foreach(var distr in univariate)
            {
                arguments.Add(distr.Key, distr.Value.GetDistribution(samples));
            }

            if (multivariate != null)
            {
                foreach (var distr in multivariate)
                {
                    var bivariate = distr.Value.GetBivariatePair(samples);

                    arguments.Add(distr.Key[0], bivariate.BaseLeft);
                    arguments.Add(distr.Key[1], bivariate.BaseRight);
                    correlations.Add(bivariate);
                }
            }

            return new DistributionsEvaluator(expression).EvaluateDistributions(arguments, correlations);
        }

        public static BaseDistribution MonteCarloDistribution(string expression, Dictionary<string, DistributionSettings> univariate, Dictionary<string[], MultivariateDistributionSettings> multivariate, int experiments, int pockets)
        {
            return new MonteCarloDistribution(expression, univariate, multivariate, experiments, pockets);
        }

        public static List<DistributionParameters> GetParameters(DistributionsPair distributions, double p)
        {
            var randomsAlgebra = distributions.RandomsAlgebra;
            var monteCarlo = distributions.MonteCarlo;

            List<DistributionParameters> parameters = new List<DistributionParameters>();
            parameters.Add(new DistributionParameters("t, ms", distributions.RandomsAlgebraTime?.TotalMilliseconds, distributions.MonteCarloTime?.TotalMilliseconds));
            parameters.Add(new DistributionParameters("μ", randomsAlgebra?.Mean, monteCarlo?.Mean));
            parameters.Add(new DistributionParameters("σ", randomsAlgebra?.StandardDeviation, monteCarlo?.StandardDeviation));
            parameters.Add(new DistributionParameters("σ²", randomsAlgebra?.Variance, monteCarlo?.Variance));
            parameters.Add(new DistributionParameters("U⁺", randomsAlgebra?.QuantileUpper(p), monteCarlo?.QuantileUpper(p)));
            parameters.Add(new DistributionParameters("U⁻", randomsAlgebra?.QuantileLower(p), monteCarlo?.QuantileLower(p)));
            parameters.Add(new DistributionParameters("γ", randomsAlgebra?.Skewness, monteCarlo?.Skewness));
            parameters.Add(new DistributionParameters("U±", randomsAlgebra?.QuantileRange(p), monteCarlo?.QuantileRange(p)));

            return parameters;
        }
    }

    public class DistributionParameters
    {
        public DistributionParameters(string name, double? randomsAlgebra, double? monteCarlo)
        {
            Name = name;
            RandomsAlgebra = randomsAlgebra;
            MonteCarlo = monteCarlo;
        }

        public string Name
        {
            get;
            set;
        }

        public double? RandomsAlgebra
        {
            get;
            set;
        }

        public double? MonteCarlo
        {
            get;
            set;
        }

        public double? PersentRatio
        {
            get
            {
                return GetPersentRatio(RandomsAlgebra, MonteCarlo);
            }
        }

        private double? GetPersentRatio(double? v1, double? v2)
        {
            if (v1.HasValue && v2.HasValue)
            {
                double v1V = v1.Value;
                double v2V = v2.Value;
                return ((v1V - v2V) / v1V * 100d);
            }
            else
            {
                return null;
            }
        }
    }

    public class DistributionsPair
    {
        public BaseDistribution RandomsAlgebra
        {
            get;
            set;
        }

        public BaseDistribution MonteCarlo
        {
            get;
            set;
        }

        public TimeSpan? RandomsAlgebraTime
        {
            get;
            set;
        }

        public TimeSpan? MonteCarloTime
        {
            get;
            set;
        }
    }
}
