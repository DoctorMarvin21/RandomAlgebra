using System;
using System.Collections.Generic;
using System.Linq;
using Accord.Math;
using Accord.Statistics.Distributions.Univariate;
using RandomAlgebra.Distributions.Settings;

namespace RandomAlgebra.Distributions
{
    internal class MultivariateGenerator
    {
        private readonly int length = 0;
        private readonly int[] indexesUnivariate;
        private readonly int[] indexesMultivariate;
        private readonly UnivariateContinuousDistribution[] univariate;
        private readonly MultivariateDistributionSettings[] multivariate;

        public MultivariateGenerator(string[] orderedArguments, Dictionary<string, DistributionSettings> univariateDistributions, Dictionary<string[], MultivariateDistributionSettings> multivariateDistributions)
        {
            length = orderedArguments.Length;

            foreach (var arg in univariateDistributions.Keys)
            {
                if (multivariateDistributions.Keys.Any(x => x.Contains(arg)))
                {
                    throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ArgumentSpecifiedSeveralTimes, arg);
                }
            }

            foreach (var args in multivariateDistributions.Keys)
            {
                foreach (string arg in args)
                {
                    if (args.Count(x => x == arg) > 1)
                    {
                        throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ArgumentSpecifiedSeveralTimes, arg);
                    }

                    if (multivariateDistributions.Keys.Where(x => x != args).Any(x => x.Contains(arg)))
                    {
                        throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ArgumentSpecifiedSeveralTimes, arg);
                    }
                }
            }

            univariate = univariateDistributions.Select(x => x.Value.GetUnivariateContinuousDistribution()).ToArray();
            multivariate = multivariateDistributions.Select(x => x.Value).ToArray();

            indexesUnivariate = GenerateIndexesUnivariate(orderedArguments, univariateDistributions);
            indexesMultivariate = GenerateIndexesMultivariate(orderedArguments, multivariateDistributions);

            for (int i = 0; i < length; i++)
            {
                string arg = orderedArguments[i];

                if (!(indexesUnivariate.Contains(i) || indexesMultivariate.Contains(i)))
                {
                    throw new DistributionsArgumentException(DistributionsArgumentExceptionType.ParameterValueIsMissing, arg);
                }
            }
        }

        public double[] Generate(Random rnd)
        {
            double[] generated = new double[length];
            for (int i = 0; i < univariate.Length; i++)
            {
                generated[indexesUnivariate[i]] = univariate[i].Generate(rnd);
            }

            int iter = 0;
            for (int i = 0; i < multivariate.Length; i++)
            {
                double[] mul = multivariate[i].GenerateRandom(rnd);

                for (int j = 0; j < mul.Length; j++)
                {
                    generated[indexesMultivariate[iter]] = mul[j];
                    iter++;
                }
            }

            return generated;
        }

        private int[] GenerateIndexesUnivariate(string[] orderedArguments, Dictionary<string, DistributionSettings> univariateDistributions)
        {
            int iterIndex = 0;
            int[] result = new int[univariateDistributions.Count];

            foreach (var distr in univariateDistributions)
            {
                var argIndex = orderedArguments.IndexOf(distr.Key);

                if (argIndex >= 0)
                {
                    result[iterIndex] = argIndex;
                }

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
                    {
                        result[iterIndex] = argIndex;
                    }

                    iterIndex++;
                }
            }

            return result;
        }
    }
}
