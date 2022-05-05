using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class MultivariateExpressionArgument
    {
        private NameAndSettingType type;

        public MultivariateExpressionArgument(MultivariateDistributionSettings settings)
        {
            MultivariateDistributionSettings = settings;
        }

        public string[] Arguments { get; set; }

        public string JoinedArguments => string.Join("; ", Arguments);

        public IList<OneDimensionalArrayBinding<string>[]> ArgumentsBindings { get; set; }

        public NameAndSettingType Type
        {
            get => type;
            set
            {
                type = value;
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

        private static MultivariateDistributionSettings GetDistributionSettingsType()
        {
            throw new NotImplementedException();
        }
    }
}
