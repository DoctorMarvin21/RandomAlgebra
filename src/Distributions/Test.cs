using RandomsAlgebra.Distributions;
using RandomsAlgebra.Distributions.Settings;
using RandomsAlgebra.DistributionsEvaluation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distribuitons
{
    public static class Test
    {

        public static DataTable TestData()
        {
            DataTable results = new DataTable();
            results.Columns.Add(new DataColumn("R", typeof(double)));
            results.Columns.Add(new DataColumn("Sorg", typeof(double)));
            results.Columns.Add(new DataColumn("Smath", typeof(double)));
            results.Columns.Add(new DataColumn("Smc", typeof(double)));

            int experiments = 10;
            int samples = 1000;
            int randoms = (int)10e4;
            double tolerance = 0.0001;
            string formula = "A+B";
            int testType = 0;


            DistributionsEvaluator evaluator = new DistributionsEvaluator(formula);

            for (int i = 0; i < experiments; i++)
            {
                double s1 = 1;
                double s2 = InterpolateLiner(0.1, 10, i, experiments);


                double vOriginal
                var pair = GetPair(testType, 0, 0, s1, s2);
                 = GetResult(testType, 0, 0, s1, s2);

                var resultMath = evaluator.EvaluateDistributions(
                    new KeyValuePair<string, DistributionBase>("A", pair[0].GetDistribution(samples, tolerance, new Optimizations { UseContiniousConvolution = false, UseFFTConvolution = false })),
                    new KeyValuePair<string, DistributionBase>("B", pair[1].GetDistribution(samples, tolerance, new Optimizations { UseContiniousConvolution = false, UseFFTConvolution = false }))
                    );

                List<double> monteCarloQ = new List<double>();

                for (int j = 0; j < 10; j++)
                {
                    var resultMonteCarlo = new MonteCarloDistribution(evaluator, new Dictionary<string, DistributionSettings> { { "A", pair[0] }, { "B", pair[1] } }, randoms, 100);
                    monteCarloQ.Add(resultMonteCarlo.Variance);
                }
                double monteCarloQresult = Math.Sqrt(monteCarloQ.Sum(x => Math.Pow(x - vOriginal, 2)) / (monteCarloQ.Count - 1)) * 1.96;

                results.Rows.Add(s2, vOriginal, resultMath.Variance, vOriginal + monteCarloQresult);
            }

            return results;
        }

        private static DistributionSettings[] GetPair(int type, double m1, double m2, double s1, double s2, out double result)
        {
            if (type == 0 || type == 1 || type == 2)
            {
                result = Math.Pow(s1, 2) + Math.Pow(s2, 2);
            }
            else
            {
                result = 0;
            }
            if (type == 0)
            {
                return new DistributionSettings[] { new NormalDistributionSettings(m1, s1), new NormalDistributionSettings(m2, s2) };
            }
            else if (type == 1)
            {
                double a = s2 * Math.Sqrt(3);
                return new DistributionSettings[] { new NormalDistributionSettings(m1, s1), new UniformDistributionSettings(-a + m2, a + m2) };
            }
            else if (type == 2)
            {
                double a1 = s1 * Math.Sqrt(3);
                double a2 = s2 * Math.Sqrt(3);

                return new DistributionSettings[] { new UniformDistributionSettings(-a1 + m2, a1 + m2), new UniformDistributionSettings(-a2 + m2, a2 + m2) };
            }
            else
            {
                return null;
            }
        }

        private static double InterpolateLiner(double min, double max, int n, int count)
        {
            return min + (max - min) * (double)n / (count - 1);
        }
    }
}
