using MudBlazor;
using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class MultivariateExpressionArgument
    {
        private NameAndSettingType type;
        private MultivariateSettingsSource settings;

        public MultivariateExpressionArgument()
        {
            Settings = new MultivariateSettingsSource(new MultivariateDistributionSettings(2, new NormalDistributionSettings()));
            type = NameAndSettingType.MultivariateSettingTypes.First(x => x.SettingsType == typeof(NormalDistributionSettings));
        }

        public string[] Arguments { get; set; }

        public string JoinedArguments => string.Join("; ", Arguments);

        public MudTable<OneDimensionalArrayBinding<string>[]> ArgumentsTable { get; set; }

        public IList<OneDimensionalArrayBinding<string>[]> ArgumentsBindings { get; set; }

        public NameAndSettingType SettingsType
        {
            get => type;
            set
            {
                type = value;
                UpdateSettings();
            }
        }

        public MultivariateSettingsSource Settings
        {
            get => settings;
            set
            {
                settings = value;
                UpdateArguments();
            }
        }

        public int Dimension
        {
            get => Settings.Dimension;
            set
            {
                Settings.Dimension = value;
                UpdateArguments();
            }
        }

        public static Dictionary<string[], MultivariateDistributionSettings> CreateDictionary(ICollection<MultivariateExpressionArgument> functionArguments)
        {
            Dictionary<string[], MultivariateDistributionSettings> keyValuePairs = new Dictionary<string[], MultivariateDistributionSettings>();
            foreach (MultivariateExpressionArgument arg in functionArguments)
            {
                keyValuePairs.Add(arg.Arguments, arg.Settings.GetSettings());
            }
            return keyValuePairs;
        }

        private void UpdateSettings()
        {
            Settings = new MultivariateSettingsSource(new MultivariateDistributionSettings(Settings.Means, Settings.CovarianceMatrix, (DistributionSettings)Activator.CreateInstance(type.SettingsType)));
        }

        private void UpdateArguments()
        {
            var splitDimension = Math.Min(Arguments?.Length ?? 0, Settings.Dimension);

            var arguments = new string[Settings.Dimension];

            if (Arguments != null)
            {
                for (int i = 0; i < splitDimension; i++)
                {
                    arguments[i] = Arguments[i];
                }
            }

            char lastChar;

            if (splitDimension > 0 && Arguments?[splitDimension - 1]?.Length > 0)
            {
                lastChar = Arguments[splitDimension - 1][0];
            }
            else
            {
                lastChar = (char)('A' - 1);
            }

            for (int i = splitDimension; i < Settings.Dimension; i++)
            {
                arguments[i] = ((char)(i - splitDimension + lastChar + 1)).ToString();
            }

            Arguments = arguments;

            try
            {
                ArgumentsTable?.SetEditingItem(null);
            }
            catch
            {
            }

            ArgumentsBindings = OneDimensionalArrayBinding<string>.GetArrayBindings(Arguments);
        }
    }
}
