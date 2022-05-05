using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class NameAndSettingType
    {
        private static readonly Dictionary<string, string> settingsNames = new Dictionary<string, string>
        {
            { nameof(ArcsineDistributionSettings), "Arcsine" },
            { nameof(BetaDistributionSettings), "Beta" },
            { nameof(BivariateBasedNormalDistributionSettings), "Two dimensional normal" },
            { nameof(ChiDistributionSettings), "Chi" },
            { nameof(ChiSquaredDistributionSettings), "Chi-squared" },
            { nameof(ExponentialDistributionSettings), "Exponential" },
            { nameof(GammaDistributionSettings), "Gamma" },
            { nameof(GeneralizedNormalDistributionSettings), "Generalized normal" },
            { nameof(LognormalDistributionSettings), "Lognormal" },
            { nameof(MultivariateBasedNormalDistributionSettings), "Sum of correlated normal" },
            { nameof(MultivariateNormalDistributionSettings), "Normal" },
            { nameof(MultivariateTDistributionSettings), "t-distribution" },
            { nameof(NormalDistributionSettings), "Normal" },
            { nameof(RayleighDistributionSettings), "Rayleigh" },
            { nameof(StudentGeneralizedDistributionSettings), "t-distribution" },
            { nameof(UniformDistributionSettings), "Uniform" }
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
            UnvariateSettingTypes = new NameAndSettingType[]
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

            MultivariateSettingTypes = new NameAndSettingType[]
            {
                new NameAndSettingType(typeof(MultivariateNormalDistributionSettings)),
                new NameAndSettingType(typeof(MultivariateTDistributionSettings))
            };
        }

        public static NameAndSettingType[] UnvariateSettingTypes { get; }

        public static NameAndSettingType[] MultivariateSettingTypes { get; }

        public Type SettingsType { get; }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
