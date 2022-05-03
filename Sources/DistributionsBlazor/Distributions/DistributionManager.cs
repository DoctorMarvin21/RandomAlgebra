using RandomAlgebra.Distributions;
using RandomAlgebra.Distributions.Settings;
using RandomAlgebra.DistributionsEvaluation;

namespace DistributionsBlazor
{
    public static class DistributionManager
    {
        public static BaseDistribution RandomsAlgebraDistribution(string expression, Dictionary<string, DistributionSettings> univariate, Dictionary<string[], MultivariateDistributionSettings> multivariate, int samples)
        {
            Dictionary<string, BaseDistribution> arguments = new Dictionary<string, BaseDistribution>();
            List<CorrelatedPair> correlations = new List<CorrelatedPair>();

            foreach (var distr in univariate)
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
    }
}
