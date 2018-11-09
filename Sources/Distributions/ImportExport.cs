using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Distributions
{
    public static class ImportExport
    {
        const string _t = "\t";

        public static string ExportToText(DistributionsPair pair)
        {
            StringBuilder sb = new StringBuilder();
            if (pair.RandomsAlgebra != null)
            {
                sb.AppendLine(Languages.GetText("RandomsAlgebra"));
                AppendDistribution(sb, pair.RandomsAlgebra);
            }

            if (pair.MonteCarlo != null)
            {
                sb.AppendLine(Languages.GetText("MonteCarlo"));
                AppendDistribution(sb, pair.MonteCarlo);
            }

            return sb.ToString();
        }

        private static void AppendDistribution(StringBuilder sb, RandomAlgebra.Distributions.BaseDistribution distribution)
        {
            double step = distribution.Step;
            var inv = System.Globalization.CultureInfo.CurrentCulture;


            AppendTableLine(sb, Languages.GetText("Argument"), Languages.GetText("PDFTitle"), Languages.GetText("CDFTitle"));

            for (double x = distribution.MinX; x < distribution.MaxX; x += step)
            {
                AppendTableLine(sb, x.ToString(inv), distribution.ProbabilityDensityFunction(x).ToString(inv), distribution.DistributionFunction(x).ToString(inv));
            }
        }



        private static void AppendTableLine(StringBuilder sb, params string[] args)
        {
            int l = args.Length;

            for (int i = 0; i < l; i++)
            {
                sb.Append(args[i]);

                if (i == l - 1)
                {
                    sb.Append(Environment.NewLine);
                }
                else
                {
                    sb.Append(_t);
                }
            }
        }
    }
}
