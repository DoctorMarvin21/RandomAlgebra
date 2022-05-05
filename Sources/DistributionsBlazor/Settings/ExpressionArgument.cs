using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class ExpressionArgument
    {
        private NameAndSettingType settingsType;

        public ExpressionArgument()
            : this("A", new NormalDistributionSettings())
        {
        }

        public ExpressionArgument(string arg, Type settingsType)
        {
            Argument = arg;
            SettingsType = NameAndSettingType.UnvariateSettingTypes.FirstOrDefault(x => x.SettingsType == settingsType);
        }

        public ExpressionArgument(string arg, DistributionSettings settings)
            : this(arg, settings.GetType())
        {
            Settings = GetSettingsSource(settings);
        }

        public string Argument { get; set; }

        public NameAndSettingType SettingsType
        {
            get => settingsType;
            set
            {
                settingsType = value;
                UpdateSettings();
            }
        }

        public DistributionSettingsSource Settings { get; set; }

        private static DistributionSettingsSource GetSettingsSource(DistributionSettings settings)
        {
            if (settings is MultivariateBasedNormalDistributionSettings multivariate)
            {
                return new MultivariateBasedNormalSettingsSource(multivariate);
            }
            else
            {
                return new DistributionSettingsSource(settings);
            }
        }

        private void UpdateSettings()
        {
            if (SettingsType.SettingsType == typeof(MultivariateBasedNormalDistributionSettings))
            {
                Settings = GetSettingsSource(new MultivariateBasedNormalDistributionSettings());
            }
            else
            {
                Settings = GetSettingsSource((DistributionSettings)Activator.CreateInstance(SettingsType.SettingsType));
            }
        }

        public static Dictionary<string, DistributionSettings> CreateDictionary(ICollection<ExpressionArgument> functionArguments)
        {
            var keyValuePairs = new Dictionary<string, DistributionSettings>();

            foreach (ExpressionArgument arg in functionArguments)
            {
                keyValuePairs.Add(arg.Argument, arg.Settings.Settings);
            }

            return keyValuePairs;
        }
    }
}
