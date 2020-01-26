using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.DistributionsEvaluation;
using RandomAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomAlgebra.Distributions
{
    /// <summary>
    /// Propagation of distributions using Monte-Carlo method
    /// </summary>
    public sealed class MonteCarloDistribution : DiscreteDistribution
    {
        static readonly ThreadLocal<Random> _rnd = new ThreadLocal<Random>(() => new Random(Environment.TickCount * Thread.CurrentThread.ManagedThreadId));
        readonly double[] _randomSorted = null;
        double? _mean = null;
        double? _variance = null;
        double? _skewness = null;

        /// <summary>
        /// Creates instance of Monte-Calrlo distribution
        /// </summary>
        /// <param name="model">Model of propagation</param>
        /// <param name="univariateDistributions">Dictionary of univariate distribution settings and arguments</param>
        /// <param name="multivariateDistributions">Dictionary of multivariate distribution settings and arguments</param>
        /// <param name="samples">Number of experiments</param>
        /// <param name="pockets">Number of pockets for generating coodinates of PDF and CDF</param>
        public MonteCarloDistribution(string model, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions, int samples, int pockets) : this(GenerateBasicData(new DistributionsEvaluator(model), univariateDistributions, multivariateDistributions, samples, pockets))
        {
        }

        /// <summary>
        /// Creates instance of Monte-Calrlo distribution
        /// </summary>
        /// <param name="model">Model of propagation</param>
        /// <param name="randomSource">Dictionary of distribution settings and arguments</param>
        /// <param name="samples">Number of experiments</param>
        /// <param name="pockets">Number of pockets for generating coodinates of PDF and CDF</param>
        public MonteCarloDistribution(string model, Dictionary<string, DistributionSettings> randomSource, int samples, int pockets) : this(GenerateBasicData(new DistributionsEvaluator(model), randomSource, null, samples, pockets))
        {
        }

        private MonteCarloDistribution(BasicDistributionData basicData) : base(basicData.XAxis, basicData.PDF, basicData.CDF)
        {
            _randomSorted = basicData.RandomSorted;
        }

        #region Generate and build coordinates
        private static BasicDistributionData GenerateBasicData(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions, int samples, int pockets)
        {
            if (evaluator == null)
                throw new ArgumentNullException(nameof(evaluator));

            if (univariateDistributions == null)
                throw new ArgumentNullException(nameof(univariateDistributions));

            if (samples < 3)
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.NumberOfExperimentsMustBeGreaterThenTwo);

            if (pockets < 3)
                throw new DistributionsArgumentException(DistributionsArgumentExceptionType.NumberOfPocketsMustBeGreaterThenTwo);

            BasicDistributionData data = new BasicDistributionData();

            double[] random;

            if (multivariateDistributions == null)
            {
                random = GenerateRandom(evaluator, univariateDistributions, samples);
            }
            else
            {
                random = GenerateRandom(evaluator, univariateDistributions, multivariateDistributions, samples);
            }

            Array.Sort(random);
			double step;
            double[] xAxis = CommonRandomMath.GenerateXAxis(random[0], random[samples - 1], pockets, out step);

            double[] cdf = GenerateCDF(xAxis, random);


            data.RandomSorted = random;
            data.XAxis = xAxis;
            data.CDF = cdf;
            data.PDF = Derivate(cdf, xAxis[pockets - 1] - xAxis[0]);

            return data;
        }

        private static double[] GenerateRandom(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> randomSource, int samples)
        {
            double[] random = new double[samples];
            var order = evaluator.Parameters;

            int argsCount = order.Length;

            UnivariateContinuousDistribution[] orderedDistributions = new UnivariateContinuousDistribution[argsCount];

            ThreadLocal<double[]> argsLocal = new ThreadLocal<double[]>(() => new double[argsCount]);

            for (int i = 0; i < argsCount; i++)
            {
                string arg = order[i];
				DistributionSettings value;
                if (randomSource.TryGetValue(arg, out value))
                {
                    orderedDistributions[i] = value.GetUnivariateContinuoisDistribution();
                }
                else
                {
                    //TODO: check this out
                    //throw new DistributionsArgumentException($"Parameter value \"{arg}\" is missing", $"Отсутствует значение параметра \"{arg}\"");
                    throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ParameterValueIsMissing, arg);
                }
            }

            Parallel.For(0, samples, i =>
            {
                double[] args = argsLocal.Value;
                Random rnd = _rnd.Value;

                for (int j = 0; j < argsCount; j++)
                {
                    args[j] = orderedDistributions[j].Generate(rnd);
                }

                random[i] = evaluator.EvaluateCompiled(args);
            });

            return random;
        }

        private static double[] GenerateRandom(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions, int samples)
        {
            double[] random = new double[samples];
            var order = evaluator.Parameters;

            MultivariateGenerator generator = new MultivariateGenerator(order, univariateDistributions, multivariateDistributions);

            Parallel.For(0, samples, i =>
            {
                Random rnd = _rnd.Value;

                double[] args = generator.Generate(rnd);

                random[i] = evaluator.EvaluateCompiled(args);
            }
            );

            return random;
        }

        private static double[] GenerateCDF(double[] xCoordinates, double[] randomSorted)
        {
            int length = xCoordinates.Length;
            int randomLength = randomSorted.Length;

            double[] yCoordinatesCDF = new double[length];

            double cStep = 1d / (randomLength - 1);
            int d = 0;

            for (int i = 0; i < length; i++)
            {
                double max = xCoordinates[i];

                while (d < randomLength && randomSorted[d] <= max)
                {
                    d++;
                }

                yCoordinatesCDF[i] = d * cStep;

            }

            return yCoordinatesCDF;
        }

        private static double[] Derivate(double[] yValues, double xRange)
        {
            int length = yValues.Length;

            double[] derivated = new double[length];

            double step = xRange / (length - 1);//шаг по числу интервалов

            derivated[0] = (yValues[1] - yValues[0]) / step;

            for (int i = 1; i < length - 1; i++)
            {
                derivated[i] = (yValues[i + 1] - yValues[i - 1]) / (2d * step);
            }

            derivated[length - 1] = (yValues[length - 1] - yValues[length - 2]) / step;

            return derivated;
        }

        #endregion

        #region Overrides
        internal override double InnerMean
        {
            get
            {
                if (_mean == null)
                {
                    double sum = 0;

                    for (int i = 0; i < _randomSorted.Length; i++)
                    {
                        sum += _randomSorted[i];
                    }

                    _mean = sum / _randomSorted.Length;
                }
                return _mean.Value;
            }
        }

        internal override double InnerVariance
        {
            get
            {
                if (_variance == null)
                {
                    double mean = InnerMean;

                    double sum = 0;

                    for (int i = 0; i < _randomSorted.Length; i++)
                    {
                        sum += Math.Pow(_randomSorted[i] - mean, 2);
                    }

                    _variance = sum / (_randomSorted.Length - 1);
                }
                return _variance.Value;
            }
        }

        internal override double InnerSkewness
        {
            get
            {
                if (_skewness == null)
                {
                    double mean = InnerMean;


                    double sum = 0;

                    for (int i = 0; i < _randomSorted.Length; i++)
                    {
                        sum += Math.Pow(_randomSorted[i] - mean, 3);
                    }

                    double m = sum / (_randomSorted.Length - 1);

                    double s = Math.Pow(InnerVariance, 3.0 / 2.0);

                    _skewness = m / s;
                }
                return _skewness.Value;
            }
        }

        internal override double InnerQuantile(double p)
        {
            double len = _randomSorted.Length;
            double c = (len - 1) * p;
            return _randomSorted[(int)c];
        }
        #endregion

        private class BasicDistributionData
        {
            public double[] RandomSorted { get; set; }
            public double[] XAxis { get; set; }
            public double[] PDF { get; set; }
            public double[] CDF { get; set; }
        }
    }
}
