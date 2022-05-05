using MudBlazor;
using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class MultivariateSettingsSource : DistributionSettingsSource
    {
        private readonly MultivariateBasedNormalDistributionSettings multivariateSettings;

        public MultivariateSettingsSource(MultivariateBasedNormalDistributionSettings settings)
                    : base(settings)
        {
            multivariateSettings = settings;
            UpdateBindings();
        }

        public int Dimension
        {
            get => multivariateSettings.MultivariateNormalDistributionSettings.Dimension;
            set
            {
                if (value != multivariateSettings.MultivariateNormalDistributionSettings.Dimension)
                {
                    UpdateDimensions(value, multivariateSettings);
                }
            }
        }

        public MudTable<OneDimensionalArrayBinding<double>[]> CoefficientsTable { get; set; }

        public MudTable<OneDimensionalArrayBinding<double>[]> MeansTable { get; set; }

        public MudTable<TwoDimesionalArrayBinding<double>[]> CovarianceTable { get; set; }

        public IList<OneDimensionalArrayBinding<double>[]> MeanBindings { get; set; }

        public IList<OneDimensionalArrayBinding<double>[]> CoefficientBindings { get; set; }

        public IList<TwoDimesionalArrayBinding<double>[]> CovarianceBindings { get; set; }

        private void UpdateDimensions(int dimension, MultivariateBasedNormalDistributionSettings settings)
        {
            int splitDimension = Math.Min(dimension, settings.MultivariateNormalDistributionSettings.Dimension);

            var coefficients = new double[dimension];
            var means = new double[dimension];
            var covarianceMatrix = new double[dimension, dimension];

            for (int i = 0; i < splitDimension; i++)
            {
                coefficients[i] = settings.Coefficients[i];
                means[i] = settings.MultivariateNormalDistributionSettings.Means[i];
            }

            for (int i = 0; i < splitDimension; i++)
            {
                for (int j = 0; j < splitDimension; j++)
                {
                    covarianceMatrix[i, j] = settings.MultivariateNormalDistributionSettings.CovarianceMatrix[i, j];
                }
            }

            for (int i = splitDimension; i < dimension; i++)
            {
                coefficients[i] = 1;
                covarianceMatrix[i, i] = 1;
            }

            settings.Coefficients = coefficients;
            settings.MultivariateNormalDistributionSettings = new MultivariateNormalDistributionSettings(means, covarianceMatrix);

            // Cancelling editing
            try
            {
                CoefficientsTable?.SetEditingItem(null);
                MeansTable?.SetEditingItem(null);
                CovarianceTable?.SetEditingItem(null);
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
                MeanBindings = OneDimensionalArrayBinding<double>.GetArrayBindings(multivariateSettings.MultivariateNormalDistributionSettings.Means);
                CovarianceBindings = TwoDimesionalArrayBinding<double>.GetArrayBindings(multivariateSettings.MultivariateNormalDistributionSettings.CovarianceMatrix);
            }
        }
    }
}
