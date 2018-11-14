using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

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


        public static void Save(PropagationData data)
        {
            string xml = data.ConvertToXml();
            
        }

        public static XmlRootAttribute Root
        {
            get;
        } = new XmlRootAttribute("SavedData");

        private static Dictionary<Type, XmlSerializer> _serializers = new Dictionary<Type, XmlSerializer>();

        private static XmlSerializer GetSerializer(Type t)
        {
            if (_serializers.TryGetValue(t, out XmlSerializer value))
            {
                return value;
            }
            else
            {
                var serial = new XmlSerializer(t, Root);
                _serializers.Add(t, serial);
                return serial;

            }
        }


        public static string ConvertToXml<T>(this T a)
        {
            return ConvertToXml(a, typeof(T));
        }

        public static string ConvertToXml(object a, Type t)
        {
            var serializer = GetSerializer(t);

            using (var stream = new StringWriter())
            {
                serializer.Serialize(stream, a);
                return stream.ToString();
            }
        }

        public static T ConvertFromXml<T>(string xml)
        {
            return (T)ConvertFromXml(xml, typeof(T));
        }

        public static object ConvertFromXml(string xml, Type t)
        {
            var serializer = GetSerializer(t);

            try
            {
                using (StringReader reader = new StringReader(xml))
                {
                    var result = serializer.Deserialize(reader);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }

        public static T DeepClone<T>(this T a)
        {
            var serializer = GetSerializer(typeof(T));

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, a);
                stream.Seek(0, SeekOrigin.Begin);
                return (T)serializer.Deserialize(stream);
            }
        }
    }

    public class PropagationData
    {
        public string Model
        {
            get;
            set;
        }

        public List<DistributionFunctionArgument> UnivariateDIstributions
        {
            get;
            set;
        }

        public List<MultivariateDistributionFunctionArgument> MultivariateDistributions
        {
            get;
            set;
        }
    }
}
