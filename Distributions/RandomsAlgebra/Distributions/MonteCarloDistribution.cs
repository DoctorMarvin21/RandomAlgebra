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
    /// Класс трансформации распределений методом Монте-Карло
    /// </summary>
    public sealed class MonteCarloDistribution : DiscreteDistribution
    {
        static readonly ThreadLocal<Random> _rnd = new ThreadLocal<Random>(() => new Random(Environment.TickCount * Thread.CurrentThread.ManagedThreadId));
        readonly double[] _randomsSorted = null;
        double? _mean = null;
        double? _variance = null;
        double? _skewness = null;

        /// <summary>
        /// Создает экземпляр класса для транформации распределений методом Монте-Карло
        /// </summary>
        /// <param name="evaluator">Калькулятор</param>
        /// <param name="univariateDistributions">Словарь одномерных случайных величин</param>
        /// <param name="multivariateDistributions">Словарь многомерных случайных величин</param>
        /// <param name="samples">Число генерируемых случайных величин, минимальное значение: 3</param>
        /// <param name="pockets">Количество отсчетов в дискретных координатах функции распределения и плотности вероятности, минимальное значение: 3</param>
        public MonteCarloDistribution(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions, int samples, int pockets) : this(GenerateBasicData(evaluator, univariateDistributions, multivariateDistributions, samples, pockets))
        {
        }

        /// <summary>
        /// Создает экземпляр класса для транформации распределений методом Монте-Карло
        /// </summary>
        /// <param name="evaluator">Калькулятор</param>
        /// <param name="randomSource">Словарь случайных величин</param>
        /// <param name="samples">Число генерируемых случайных величин, минимальное значение: 3</param>
        /// <param name="pockets">Количество отсчетов в дискретных координатах функции распределения и плотности вероятности, минимальное значение: 3</param>
        public MonteCarloDistribution(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> randomSource, int samples, int pockets) : this(GenerateBasicData(evaluator, randomSource, null, samples, pockets))
        {
        }

        private MonteCarloDistribution(BasicDistributionData basicData) : base(basicData.XAxis, basicData.PDF, basicData.CDF)
        {
            _randomsSorted = basicData.RandomsSorted;
        }

        #region Генерация случайных величин и построение графиков
        private static BasicDistributionData GenerateBasicData(DistributionsEvaluator evaluator, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions, int samples, int pockets)
        {
            if (evaluator == null)
                throw new ArgumentException("Не задан калькулятор");

            if (univariateDistributions == null)
                throw new ArgumentNullException("Словарь случайных величин не задан");

            if (samples < 3)
                throw new ArgumentException($"Число случайных величин {samples} меньше минимально допустимого значения 3");

            if (pockets < 3)
                throw new ArgumentException($"Число карманов {pockets} меньше минимально допустимого значения 3");

            if (pockets > samples)
                throw new ArgumentException($"Число карманов {pockets} больше числа случайных величин {samples}");

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
                    throw new ArgumentException($"Отсутствует значение переменной \"{arg}\"");
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
            //TODO: узнать, зачем я так сделал
            for (int i = 0; i < samples; i++)
            //Parallel.For(0, samples, i =>
            {
                Random rnd = _rnd.Value;

                double[] args = generator.Generate(rnd);

                randoms[i] = evaluator.EvaluateCompiled(args);
            }
            //);

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

        #region Переопределение параметров распределения
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
            double c = len * p;
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
