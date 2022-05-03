using RandomAlgebra.Distributions.Settings;
using System.Reflection;

namespace DistributionsBlazor
{
    public class ExpressionArgument
    {
        private NameAndSettingType settingsType;
        private DistributionSettings distributionSettings;

        public ExpressionArgument(string arg, Type settingsType)
        {
            Argument = arg;
            SettingsType = SettingTypes.FirstOrDefault(x => x.SettingsType == settingsType);
        }

        public ExpressionArgument(string arg, DistributionSettings settings)
            : this(arg, settings.GetType())
        {
            DistributionSettings = settings;
        }

        public string Argument { get; set; }

        public DistributionSettings DistributionSettings
        {
            get => distributionSettings;
            set
            {
                distributionSettings = value;
                UpdateSettingsBindings(DistributionSettings);
            }
        }

        public NameAndSettingType SettingsType
        {
            get => settingsType;
            set
            {
                settingsType = value;
                GenerateSettings();
            }
        }

        public NameAndSettingType[] SettingTypes => NameAndSettingType.DisplayNames;

        public IList<DistributionSettingsBinding> DistributionSettingsBindings { get; }
            = new List<DistributionSettingsBinding>();

        private void GenerateSettings()
        {
            DistributionSettings = (DistributionSettings)Activator.CreateInstance(SettingsType.SettingsType);
        }

        private void UpdateSettingsBindings(DistributionSettings settings)
        {
            DistributionSettingsBindings.Clear();

            var properties = settings.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanWrite && x.CanRead);

            foreach (var property in properties)
            {
                DistributionSettingsBindings.Add(new DistributionSettingsBinding(settings, property));
            }
        }

        public static Dictionary<string, DistributionSettings> CreateDictionary(ICollection<ExpressionArgument> functionArguments)
        {
            Dictionary<string, DistributionSettings> keyValuePairs = new Dictionary<string, DistributionSettings>();
            foreach (ExpressionArgument arg in functionArguments)
            {
                keyValuePairs.Add(arg.Argument, arg.DistributionSettings);
            }
            return keyValuePairs;
        }
    }
}
