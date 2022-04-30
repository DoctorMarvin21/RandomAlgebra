using Avalonia.Threading;
using RandomAlgebra.Distributions;
using RandomAlgebra.Distributions.Settings;
using RandomAlgebra.DistributionsEvaluation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DistributionsAvalonia
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

    public class DistributionParameter
    {
        public DistributionParameter(string name, double? randomsAlgebra, double? monteCarlo)
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
        private readonly object processLocker = new object();

        public DistributionsPair()
        {
        }

        public BaseDistribution RandomAlgebra { get; set; }

        public BaseDistribution MonteCarlo { get; set; }

        public TimeSpan? RandomsAlgebraTime { get; set; }

        public TimeSpan? MonteCarloTime { get; set; }

        public ObservableCollection<DistributionParameter> DistributionParameters { get; } = new ObservableCollection<DistributionParameter>();

        public ObservableCollection<string> Warnings { get; } = new ObservableCollection<string>();



        public async Task ProcessAsync(Configuration configuration)
        {
            await Task.Run(() => Process(configuration));
        }

        public void Process(Configuration configuration)
        {
            lock (processLocker)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Warnings.Clear();
                });

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
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                DistributionParameters.Clear();
            });

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
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                DistributionParameters.Add(parameter);
            });
        }
    }
}
