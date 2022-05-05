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
            Settings = new MultivariateNormalSettingsSource(new MultivariateNormalDistributionSettings());
        }

        public string[] Arguments { get; set; }

        public string JoinedArguments => string.Join("; ", Arguments);

        public MudTable<OneDimensionalArrayBinding<string>[]> MeansTable { get; set; }

        public IList<OneDimensionalArrayBinding<string>[]> ArgumentsBindings { get; set; }

        public NameAndSettingType Type
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
                if (settings != null)
                {
                    settings.DimensionUpdated -= SettingsDimensionUpdated;
                }

                settings = value;
                settings.DimensionUpdated += SettingsDimensionUpdated;

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
            switch (type.SettingsType.Name)
            {
                case nameof(MultivariateNormalDistributionSettings):
                    {
                        Settings = new MultivariateNormalSettingsSource(new MultivariateNormalDistributionSettings(Settings.Means, Settings.CovarianceMatrix));
                        break;
                    }
                case nameof(MultivariateTDistributionSettings):
                    {
                        Settings = new MultivariateTSettingsSource(new MultivariateTDistributionSettings(Settings.Means, Settings.CovarianceMatrix, 10));
                        break;
                    }
            }
        }

        private void SettingsDimensionUpdated(object sender, EventArgs e)
        {
            UpdateArguments();
        }

        private void UpdateArguments()
        {
            var splitDimension = Math.Min(Arguments?.Length ?? 0, Settings.Dimension);

            var arguments = new string[Settings.Dimension];

            for (int i = 0; i < splitDimension; i++)
            {

            }

            char lastChar;

            if (splitDimension > 0 && Arguments[splitDimension - 1].Length > 0)
            {
                lastChar = Arguments[splitDimension - 1][0];
            }
            else
            {
                lastChar = 'A';
            }

            for (int i = splitDimension; i < Settings.Dimension; i++)
            {
                arguments[i] = ((char)(i - splitDimension + lastChar + 1)).ToString();
            }

            Arguments = arguments;

            try
            {
                MeansTable?.SetEditingItem(null);
            }
            catch
            {
            }

            ArgumentsBindings = OneDimensionalArrayBinding<string>.GetArrayBindings(Arguments);
        }
    }
}
