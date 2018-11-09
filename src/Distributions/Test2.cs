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
            results.Columns.Add(new DataColumn("Vorg", typeof(double)));
            results.Columns.Add(new DataColumn("Vmath", typeof(double)));
            results.Columns.Add(new DataColumn("Vmc", typeof(double)));


            int experiments = 10;
            int samples = 1000;
            int randoms = (int)10e4;
            double tolerance = 0.0001;
            int testType = 3;
            double m1 = 5;
            double m2 = 5;

            for (int i = 0; i < experiments; i++)
            {
                double s1 = 1;
                double s2 = InterpolateLiner(0.1, 10, i, experiments);

                var pair = GetData(testType, m1, m2, s1, s2, out double vOriginal, out string formula);


                DistributionsEvaluator evaluator = new DistributionsEvaluator(formula);

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

        private static DistributionSettings[] GetData(int type, double m1, double m2, double s1, double s2, out double result, out string formula)
        {
            if (type == 0 || type == 1 || type == 2)
            {
                result = Math.Pow(s1, 2) + Math.Pow(s2, 2);
                formula = "A+B";
            }
            else if (type == 3 || type == 4 || type == 5)
            {
                result = Math.Pow(s1 * m2, 2) + Math.Pow(s2 * m1, 2) + Math.Pow(s1 * s2, 2);
                formula = "A*B";
            }
            else
            {
                formula = string.Empty;
                result = 0;
            }
            if (type == 0 || type == 3)
            {
                return new DistributionSettings[] { new NormalDistributionSettings(m1, s1), new NormalDistributionSettings(m2, s2) };
            }
            else if (type == 1 || type == 4)
            {
                double a = s2 * Math.Sqrt(3);
                return new DistributionSettings[] { new NormalDistributionSettings(m1, s1), new UniformDistributionSettings(-a + m2, a + m2) };
            }
            else if (type == 2 || type == 5)
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
