using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class ExpressionArgument
    {
        private NameAndSettingType settingsType;

        public ExpressionArgument(string arg, Type settingsType)
        {
            Argument = arg;
            SettingsType = SettingTypes.FirstOrDefault(x => x.SettingsType == settingsType);
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

        public NameAndSettingType[] SettingTypes => NameAndSettingType.DisplayNames;

        private DistributionSettingsSource GetSettingsSource(DistributionSettings settings)
        {
            if (settings.GetType() == typeof(MultivariateBasedNormalDistributionSettings))
            {
                return new MultivariateSettingsSource(new MultivariateBasedNormalDistributionSettings());
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
