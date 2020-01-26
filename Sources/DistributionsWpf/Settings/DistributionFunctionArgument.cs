using RandomAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DistributionsWpf
{
    public class DistributionFunctionArgument : INotifyPropertyChanged
    {
        private DisplayNameAndSettingType _settingsType;
        private DistributionSettings _distributionSettings;
        private string _argument;

        public DistributionFunctionArgument(string arg, Type settingsType)
        {
            Argument = arg;
            SettingsType = SettingTypes.FirstOrDefault(x => x.SettingsType == settingsType);
        }

        public DistributionFunctionArgument(string arg, DistributionSettings settings)
            : this(arg, settings.GetType())
        {
            DistributionSettings = settings;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string Argument
        {
            get
            {
                return _argument;
            }
            set
            {
                _argument = value;
                OnPropertyChanged(nameof(Argument));
            }
        }

        public DistributionSettings DistributionSettings
        {
            get
            {
                return _distributionSettings;
            }
            set
            {
                _distributionSettings = value;
                OnPropertyChanged(nameof(DistributionSettings));

                DistributionSettingsBindings.Update(this, DistributionSettings);
            }
        }

        public DisplayNameAndSettingType SettingsType
        {
            get
            {
                return _settingsType;
            }
            set
            {
                _settingsType = value;
                OnPropertyChanged(nameof(SettingsType));

                GenerateSettings();
            }
        }

        public DisplayNameAndSettingType[] SettingTypes
        {
            get
            {
                return DisplayNameAndSettingType.DisplayNames;
            }
        }

        public DistributionSettingsBindingCollection DistributionSettingsBindings { get; }
            = new DistributionSettingsBindingCollection();

        private void GenerateSettings()
        {
            DistributionSettings = (DistributionSettings)Activator.CreateInstance(SettingsType.SettingsType);
        }

        public void DistributionSettingsChanged()
        {
            OnPropertyChanged(nameof(DistributionSettings));
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static Dictionary<string, DistributionSettings> CreateDictionary(ICollection<DistributionFunctionArgument> functionArguments)
        {
            Dictionary<string, DistributionSettings> keyValuePairs = new Dictionary<string, DistributionSettings>();
            foreach (DistributionFunctionArgument arg in functionArguments)
            {
                keyValuePairs.Add(arg.Argument, arg.DistributionSettings);
            }
            return keyValuePairs;
        }
    }

    public class DisplayNameAndSettingType
    {

        public DisplayNameAndSettingType(Type settingType)
        {
            SettingsType = settingType;
            Name = Resources.GetMessage(settingType.Name);
        }

        static DisplayNameAndSettingType()
        {
            DisplayNames = new DisplayNameAndSettingType[]
                {
                    new DisplayNameAndSettingType(typeof(NormalDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(UniformDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(StudentGeneralizedDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(ArcsineDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(ExponentialDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(BetaDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(GammaDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(LognormalDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(GeneralizedNormalDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(BivariateBasedNormalDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(MultivariateBasedNormalDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(ChiDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(ChiSquaredDistributionSettings)),
                    new DisplayNameAndSettingType(typeof(RayleighDistributionSettings))
            };
        }

        public string Name
        {
            get;
            set;
        }
        public Type SettingsType
        {
            get;
            set;
        }

        public static DisplayNameAndSettingType[] DisplayNames
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
