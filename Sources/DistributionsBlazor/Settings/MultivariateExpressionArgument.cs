using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public enum MultivariateType
    {
        Normal,
        TDistribution
    }

    public class MultivariateExpressionArgument
    {
        public MultivariateExpressionArgument(string[] arguments, MultivariateType type)
        {
            Arguments = arguments;
            //MultivariateDistributionSettings = settings;
        }

        public string JoinedArguments
        {
            get
            {
                if (Arguments == null)
                    return string.Empty;
                else
                {
                    return string.Join("; ", Arguments);
                }
            }
        }

        public string[] Arguments
        {
            get;
            set;
        }

        public string DistributionName
        {
            get
            {
                //Languages.GetText(MultivariateDistributionSettings.GetType().Name);
                return null;
            }
        }

        public MultivariateDistributionSettings MultivariateDistributionSettings { get; set; }

        public static Dictionary<string[], MultivariateDistributionSettings> CreateDictionary(ICollection<MultivariateExpressionArgument> functionArguments)
        {
            Dictionary<string[], MultivariateDistributionSettings> keyValuePairs = new Dictionary<string[], MultivariateDistributionSettings>();
            foreach (MultivariateExpressionArgument arg in functionArguments)
            {
                keyValuePairs.Add(arg.Arguments, arg.MultivariateDistributionSettings);
            }
            return keyValuePairs;
        }

        private static MultivariateDistributionSettings GetDistributionSettingsType(MultivariateType type)
        {
            throw new NotImplementedException();
        }
    }
}
