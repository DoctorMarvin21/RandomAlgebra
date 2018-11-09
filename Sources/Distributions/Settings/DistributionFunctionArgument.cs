using RandomAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Distributions
{
    public class DistributionFunctionArgument
    {
        private Type _settingType = typeof(NormalDistributionSettings);

        public DistributionFunctionArgument(string arg, Type settingsType)
        {
            Argument = arg;
            _settingType = settingsType;
            GenerateSettings();
        }

        public DistributionFunctionArgument(string arg, DistributionSettings settings)
        {
            Argument = arg;
            _settingType = settings.GetType();
            DistributionSettings = settings;
        }

        public string Argument
        {
            get;
            set;
        }
        public Type SettingsType
        {
            get
            {
                return _settingType;
            }
            set
            {
                if (_settingType != value)
                {
                    _settingType = value;
                    GenerateSettings();
                }
            }
        }

        private void GenerateSettings()
        {
            DistributionSettings = (DistributionSettings)Activator.CreateInstance(SettingsType);
        }

        public DistributionSettings DistributionSettings
        {
            get;
            private set;
        }

        public static Dictionary<string, DistributionSettings> CreateDictionary(List<DistributionFunctionArgument> functionArguments)
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
            Name = Languages.GetText(settingType.Name);
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
