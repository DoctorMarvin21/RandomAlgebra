using MudBlazor;
using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public class MultivariateSettingsSource
    {
        private int dimension;

        public MultivariateSettingsSource(MultivariateDistributionSettings settings)
        {
            Func<string, bool> filter = (name) => name != nameof(NormalDistributionSettings.StandardDeviation) &&
                name != nameof(NormalDistributionSettings.Mean);

            BaseSettings = new DistributionSettingsSource(settings.BaseSettings, filter);
            dimension = settings.Dimension;
            Means = settings.Means;
            CovarianceMatrix = settings.CovarianceMatrix;

            UpdateBindings();
        }

        public DistributionSettingsSource BaseSettings { get; }

     
        public int Dimension
        {
            get => dimension;
            set
            {
                if (value != dimension)
                {
                    UpdateDimensions(value);
                }
            }
        }

        public MudTable<OneDimensionalArrayBinding<double>[]> MeansTable { get; set; }

        public MudTable<TwoDimesionalArrayBinding<double>[]> CovarianceTable { get; set; }

        public IList<OneDimensionalArrayBinding<double>[]> MeanBindings { get; set; }

        public IList<TwoDimesionalArrayBinding<double>[]> CovarianceBindings { get; set; }

        public double[] Means { get; set; }

        public double[,] CovarianceMatrix { get; set; }

        private void UpdateDimensions(int dimension)
        {
            int splitDimension = Math.Min(dimension, this.dimension);

            var means = new double[dimension];
            var covariance = new double[dimension, dimension];

            for (int i = 0; i < splitDimension; i++)
            {
                means[i] = Means[i];
            }

            for (int i = 0; i < splitDimension; i++)
            {
                for (int j = 0; j < splitDimension; j++)
                {
                    covariance[i, j] = CovarianceMatrix[i, j];
                }
            }

            for (int i = splitDimension; i < dimension; i++)
            {
                covariance[i, i] = 1;
            }

            this.dimension = dimension;
            Means = means;
            CovarianceMatrix = covariance;

            // Canceling editing
            try
            {
                MeansTable?.SetEditingItem(null);
                CovarianceTable?.SetEditingItem(null);
            }
            catch
            {
            }

            UpdateBindings();
        }

        protected void UpdateBindings()
        {
            MeanBindings = OneDimensionalArrayBinding<double>.GetArrayBindings(Means);
            CovarianceBindings = TwoDimesionalArrayBinding<double>.GetArrayBindings(CovarianceMatrix);
        }

        public MultivariateDistributionSettings GetSettings()
        {
            return new MultivariateDistributionSettings(Means, CovarianceMatrix, BaseSettings.Settings);
        }
    }
}
