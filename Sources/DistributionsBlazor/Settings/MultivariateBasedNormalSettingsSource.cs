using MudBlazor;
using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class MultivariateBasedNormalSettingsSource : DistributionSettingsSource
    {
        private readonly MultivariateBasedNormalDistributionSettings multivariateSettings;

        public MultivariateBasedNormalSettingsSource(MultivariateBasedNormalDistributionSettings settings)
            : base(settings)
        {
            multivariateSettings = settings;
            MultivariateSettingsSource = new MultivariateNormalSettingsSource(settings.MultivariateNormalDistributionSettings);
            UpdateBindings();
        }

        public MultivariateNormalSettingsSource MultivariateSettingsSource { get; set; }

        public int Dimension
        {
            get => multivariateSettings.MultivariateNormalDistributionSettings.Dimension;
            set
            {
                if (value != MultivariateSettingsSource.Dimension)
                {
                    UpdateDimensions(value, multivariateSettings);
                }
            }
        }

        public MudTable<OneDimensionalArrayBinding<double>[]> CoefficientsTable { get; set; }
        public IList<OneDimensionalArrayBinding<double>[]> CoefficientBindings { get; set; }

        private void UpdateDimensions(int dimension, MultivariateBasedNormalDistributionSettings settings)
        {
            int splitDimension = Math.Min(dimension, settings.MultivariateNormalDistributionSettings.Dimension);

            var coefficients = new double[dimension];

            for (int i = 0; i < splitDimension; i++)
            {
                coefficients[i] = settings.Coefficients[i];
            }

            for (int i = splitDimension; i < dimension; i++)
            {
                coefficients[i] = 1;
            }

            settings.Coefficients = coefficients;
            MultivariateSettingsSource.Dimension = dimension;

            settings.MultivariateNormalDistributionSettings = (MultivariateNormalDistributionSettings)MultivariateSettingsSource.Settings;

            // Canceling editing
            try
            {
                CoefficientsTable?.SetEditingItem(null);
            }
            catch
            {
            }

            UpdateBindings();
        }

        protected override void UpdateBindings()
        {
            if (multivariateSettings != null)
            {
                CoefficientBindings = OneDimensionalArrayBinding<double>.GetArrayBindings(multivariateSettings.Coefficients);
            }
        }
    }
}
