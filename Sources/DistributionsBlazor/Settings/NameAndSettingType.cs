using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class NameAndSettingType
    {
        private static readonly Dictionary<string, string> settingsNames = new Dictionary<string, string>
        {
            { "ArcsineDistributionSettings", "Arcsine" },
            { "BetaDistributionSettings", "Beta" },
            { "BivariateBasedNormalDistributionSettings", "Two dimensional normal" },
            { "ChiDistributionSettings", "Chi" },
            { "ChiSquaredDistributionSettings", "Chi-squared" },
            { "ExponentialDistributionSettings", "Exponential" },
            { "GammaDistributionSettings", "Gamma" },
            { "GeneralizedNormalDistributionSettings", "Generalized normal" },
            { "LognormalDistributionSettings", "Lognormal" },
            { "MultivariateBasedNormalDistributionSettings", "Sum of correlated normal" },
            { "MultivariateNormalDistributionSettings", "Normal" },
            { "MultivariateTDistributionSettings", "t-distribution" },
            { "NormalDistributionSettings", "Normal" },
            { "RayleighDistributionSettings", "Rayleigh" },
            { "StudentGeneralizedDistributionSettings", "t-distribution" },
            { "UniformDistributionSettings", "Uniform" }
        };

        public NameAndSettingType(Type settingType)
        {
            if (settingsNames.TryGetValue(settingType.Name, out string name))
            {
                Name = name;
            }
            else
            {
                Name = settingType.Name;
            }

            SettingsType = settingType;
        }

        static NameAndSettingType()
        {
            DisplayNames = new NameAndSettingType[]
                {
                    new NameAndSettingType(typeof(NormalDistributionSettings)),
                    new NameAndSettingType(typeof(UniformDistributionSettings)),
                    new NameAndSettingType(typeof(StudentGeneralizedDistributionSettings)),
                    new NameAndSettingType(typeof(ArcsineDistributionSettings)),
                    new NameAndSettingType(typeof(ExponentialDistributionSettings)),
                    new NameAndSettingType(typeof(BetaDistributionSettings)),
                    new NameAndSettingType(typeof(GammaDistributionSettings)),
                    new NameAndSettingType(typeof(LognormalDistributionSettings)),
                    new NameAndSettingType(typeof(GeneralizedNormalDistributionSettings)),
                    new NameAndSettingType(typeof(BivariateBasedNormalDistributionSettings)),
                    new NameAndSettingType(typeof(MultivariateBasedNormalDistributionSettings)),
                    new NameAndSettingType(typeof(ChiDistributionSettings)),
                    new NameAndSettingType(typeof(ChiSquaredDistributionSettings)),
                    new NameAndSettingType(typeof(RayleighDistributionSettings))
            };
        }

        public static NameAndSettingType[] DisplayNames { get; }

        public Type SettingsType { get; }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
