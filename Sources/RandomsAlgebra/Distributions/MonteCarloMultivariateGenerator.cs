using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomsAlgebra.Distributions
{
    internal class MultivariateGenerator
    {
        readonly int _length = 0;
        readonly int[] _indexesUnivariate;
        readonly int[] _indexesMultivariate;
        readonly UnivariateContinuousDistribution[] _univariate;
        readonly MultivariateDistributionSettings[] _multivariate;

        public MultivariateGenerator(string[] orderedArguments, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions)
        {
            _length = orderedArguments.Length;

            foreach (var arg in univariateDistributions.Keys)
            {
                if (multivariateDistributions.Keys.Any(x => x.Contains(arg)))
                    throw new DistributionsArgumentException($"\"{arg}\" argument specified several times", $"Аргумент \"{arg}\" задан несколько раз");
            }

            foreach (var args in multivariateDistributions.Keys)
            {
                foreach (string arg in args)
                {
                    if (args.Count(x => x == arg) > 1)
                        throw new DistributionsArgumentException($"\"{arg}\" argument specified several times", $"Аргумент \"{arg}\" задан несколько раз");


                    if (multivariateDistributions.Keys.Where(x => x != args).Any(x => x.Contains(arg)))
                        throw new DistributionsArgumentException($"\"{arg}\" argument specified several times", $"Аргумент \"{arg}\" задан несколько раз");
                }
            }

            _univariate = univariateDistributions.Select(x => x.Value.GetUnivariateContinuoisDistribution()).ToArray();
            _multivariate = multivariateDistributions.Select(x => x.Value).ToArray();

            _indexesUnivariate = GenerateIndexesUnivariate(orderedArguments, univariateDistributions);
            _indexesMultivariate = GenerateIndexesMultivariate(orderedArguments, multivariateDistributions);


            for (int i = 0; i < _length; i++)
            {
                string arg = orderedArguments[i];

                if (!(_indexesUnivariate.Contains(i) || _indexesMultivariate.Contains(i)))
                {
                    throw new DistributionsArgumentException($"Parameter value \"{arg}\" is missing", $"Отсутствует значение параметра \"{arg}\"");
                }
            }
        }

        private int[] GenerateIndexesUnivariate(string[] orderedArguments, Dictionary<string, DistributionSettings> univariateDistributions)
        {
            int iterIndex = 0;
            int[] result = new int[univariateDistributions.Count];

            foreach (var distr in univariateDistributions)
            {
                var argIndex = orderedArguments.IndexOf(distr.Key);

                if (argIndex >= 0)
                    result[iterIndex] = argIndex;

                iterIndex++;
            }

            return result;
        }

        private int[] GenerateIndexesMultivariate(string[] orderedArguments, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions)
        {
            int iterIndex = 0;
            int[] result = new int[multivariateDistributions.Sum(x => x.Key.Count())];

            foreach (var distr in multivariateDistributions)
            {
                var keys = distr.Key;

                for (int i = 0; i < keys.Length; i++)
                {
                    var argIndex = orderedArguments.IndexOf(keys[i]);

                    if (argIndex >= 0)
                        result[iterIndex] = argIndex;

                    iterIndex++;
                }
            }

            return result;
        }

        public double[] Generate(Random rnd)
        {
            double[] generated = new double[_length];
            for (int i = 0; i < _univariate.Length; i++)
            {
                generated[_indexesUnivariate[i]] = _univariate[i].Generate(rnd);
            }

            int iter = 0;
            for (int i = 0; i < _multivariate.Length; i++)
            {
                double[] mul = _multivariate[i].GenerateRandoms(rnd);

                for (int j = 0; j < mul.Length; j++)
                {
                    generated[_indexesMultivariate[iter]] = mul[j];
                    iter++;
                }
            }

            return generated;
        }
    }
}
