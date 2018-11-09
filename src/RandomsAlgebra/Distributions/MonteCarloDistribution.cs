using Accord.Statistics.Distributions.Univariate;
using RandomsAlgebra.DistributionsEvaluation;
using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandomsAlgebra.Distributions
{
    /// <summary>
    /// Propagation of distributions using Monte-Carlo method
    /// </summary>
    public sealed class MonteCarloDistribution : DiscreteDistribution
    {
        static readonly ThreadLocal<Random> _rnd = new ThreadLocal<Random>(() => new Random(Environment.TickCount * Thread.CurrentThread.ManagedThreadId));
        readonly double[] _randomsSorted = null;
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
            _randomsSorted = basicData.RandomsSorted;
        }

        #region Generate and build coordinates
        private static BasicDistributionData GenerateBasicData(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions, int samples, int pockets)
        {
            if (evaluator == null)
                throw new ArgumentNullException(nameof(evaluator));

            if (univariateDistributions == null)
                throw new ArgumentNullException(nameof(univariateDistributions));

            if (samples < 3)
                throw new DistributionsArgumentException("Number of experiments must be greater then 2", "Число экспериментов должно быть больше 2");

            if (pockets < 3)
                throw new DistributionsArgumentException("Number of pockets must be greater then 2", "Число карманов должно быть больше 2");

            BasicDistributionData data = new BasicDistributionData();

            double[] randoms;

            if (multivariateDistributions == null)
            {
                randoms = GenerateRandoms(evaluator, univariateDistributions, samples);
            }
            else
            {
                randoms = GenerateRandoms(evaluator, univariateDistributions, multivariateDistributions, samples);
            }

            Array.Sort(randoms);
			double step;
            double[] xAxis = CommonMath.GenerateXAxis(randoms[0], randoms[samples - 1], pockets, out step);

            double[] cdf = GenerateCDF(xAxis, randoms);


            data.RandomsSorted = randoms;
            data.XAxis = xAxis;
            data.CDF = cdf;
            data.PDF = Derivate(cdf, xAxis[pockets - 1] - xAxis[0]);

            return data;
        }

        private static double[] GenerateRandoms(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> randomSource, int samples)
        {
            double[] randoms = new double[samples];
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
                    throw new DistributionsArgumentException($"Parameter value \"{arg}\" value is missing", $"Отсутствует значение параметра \"{arg}\"");
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

                randoms[i] = evaluator.EvaluateCompiled(args);
            });

            return randoms;
        }

        private static double[] GenerateRandoms(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions, int samples)
        {
            double[] randoms = new double[samples];
            var order = evaluator.Parameters;

            MultivariateGenerator generator = new MultivariateGenerator(order, univariateDistributions, multivariateDistributions);

            Parallel.For(0, samples, i =>
            {
                Random rnd = _rnd.Value;

                double[] args = generator.Generate(rnd);

                randoms[i] = evaluator.EvaluateCompiled(args);
            }
            );

            return randoms;
        }

        private static double[] GenerateCDF(double[] xCoordinates, double[] randomsSorted)
        {
            int length = xCoordinates.Length;
            int randomsLength = randomsSorted.Length;

            double[] yCoordinatesCDF = new double[length];

            double cStep = 1d / (randomsLength - 1);
            int d = 0;

            for (int i = 0; i < length; i++)
            {
                double max = xCoordinates[i];

                while (d < randomsLength && randomsSorted[d] <= max)
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

            double lowStep = xRange / (length - 1);//шаг по числу интервалов

            double highStep = xRange / (length + 1);//шаг по числу интервалов + 1

            derivated[0] = yValues[1] / highStep;

            for (int i = 1; i < length - 1; i++)
            {
                derivated[i] = (yValues[i + 1] - yValues[i - 1]) / (2d * lowStep);
            }

            derivated[length - 1] = (1 - yValues[length - 2]) / highStep;

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
                    _mean = _randomsSorted.Sum() / _randomsSorted.Length;
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

                    _variance = _randomsSorted.Sum(x => Math.Pow(x - mean, 2)) / (_randomsSorted.Length - 1);
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

                    double m = _randomsSorted.Sum(x => Math.Pow(x - mean, 3)) / (_randomsSorted.Length - 1);
                    double s = Math.Pow(InnerVariance, 3.0 / 2.0);

                    _skewness = m / s;
                }
                return _skewness.Value;
            }
        }

        internal override double InnerQuantile(double p)
        {
            double len = _randomsSorted.Length;
            double c = (len - 1) * p;
            return _randomsSorted[(int)c];
        }
        #endregion

        private class BasicDistributionData
        {
            public double[] RandomsSorted { get; set; }
            public double[] XAxis { get; set; }
            public double[] PDF { get; set; }
            public double[] CDF { get; set; }
        }
    }
}
