using RandomAlgebra.Distributions;
using System.Diagnostics;

namespace DistributionsBlazor
{
    public class DistributionsPair
    {
        private readonly object processLocker = new object();

        public DistributionsPair()
        {
        }

        public BaseDistribution RandomAlgebra { get; set; }

        public BaseDistribution MonteCarlo { get; set; }

        public TimeSpan? RandomsAlgebraTime { get; set; }

        public TimeSpan? MonteCarloTime { get; set; }

        public IList<DistributionParameter> DistributionParameters { get; } = new List<DistributionParameter>();

        public async Task ProcessAsync(Configuration configuration)
        {
            await Task.Run(() => Process(configuration));
        }

        public void Process(Configuration configuration)
        {
            lock (processLocker)
            {
                var univariate = ExpressionArgument.CreateDictionary(configuration.ExpressionArguments);
                var multivariate = MultivariateExpressionArgument.CreateDictionary(configuration.MultivariateExpressionArguments);

                if (multivariate.Count == 0)
                    multivariate = null;

                Stopwatch sw = Stopwatch.StartNew();

                if (configuration.EvaluateRandomAlgebra)
                {
                    RandomAlgebra = DistributionManager.RandomsAlgebraDistribution(
                        configuration.Expression,
                        univariate,
                        multivariate,
                        configuration.Samples);

                    sw.Stop();

                    RandomsAlgebraTime = sw.Elapsed;
                }
                else
                {
                    RandomAlgebra = null;
                    RandomsAlgebraTime = null;
                }

                if (configuration.EvaluateMonteCarlo)
                {
                    sw.Restart();

                    MonteCarlo = DistributionManager.MonteCarloDistribution(
                        configuration.Expression,
                        univariate,
                        multivariate,
                        configuration.Experiments,
                        configuration.Pockets);

                    sw.Stop();

                    MonteCarloTime = sw.Elapsed;
                }
                else
                {
                    MonteCarlo = null;
                    MonteCarloTime = null;
                }

                UpdateParameters(configuration.Probability);
            }
        }

        private void UpdateParameters(double p)
        {
            DistributionParameters.Clear();

            AddParameter(new DistributionParameter("t, ms", RandomsAlgebraTime?.TotalMilliseconds, MonteCarloTime?.TotalMilliseconds));
            AddParameter(new DistributionParameter("μ", RandomAlgebra?.Mean, MonteCarlo?.Mean));
            AddParameter(new DistributionParameter("σ", RandomAlgebra?.StandardDeviation, MonteCarlo?.StandardDeviation));
            AddParameter(new DistributionParameter("σ²", RandomAlgebra?.Variance, MonteCarlo?.Variance));
            AddParameter(new DistributionParameter("U⁺", RandomAlgebra?.QuantileUpper(p), MonteCarlo?.QuantileUpper(p)));
            AddParameter(new DistributionParameter("U⁻", RandomAlgebra?.QuantileLower(p), MonteCarlo?.QuantileLower(p)));
            AddParameter(new DistributionParameter("γ", RandomAlgebra?.Skewness, MonteCarlo?.Skewness));
            AddParameter(new DistributionParameter("U±", RandomAlgebra?.QuantileRange(p), MonteCarlo?.QuantileRange(p)));
        }

        private void AddParameter(DistributionParameter parameter)
        {
            DistributionParameters.Add(parameter);
        }
    }
}
