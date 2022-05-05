using MudBlazor;
using RandomAlgebra.Distributions.Settings;

namespace DistributionsBlazor
{
    public abstract class MultivariateSettingsSource
    {
        protected MultivariateSettingsSource(MultivariateDistributionSettings settings)
        {
            Settings = settings;
            UpdateBindings();
        }


        public MultivariateDistributionSettings Settings { get; set; }

        public int Dimension
        {
            get => Settings.Dimension;
            set
            {
                if (value != Settings.Dimension)
                {
                    UpdateDimensions(value);
                }
            }
        }

        public MudTable<OneDimensionalArrayBinding<double>[]> MeansTable { get; set; }

        public MudTable<TwoDimesionalArrayBinding<double>[]> CovarianceTable { get; set; }

        public IList<OneDimensionalArrayBinding<double>[]> MeanBindings { get; set; }

        public IList<TwoDimesionalArrayBinding<double>[]> CovarianceBindings { get; set; }

        protected void UpdateDimensions(int dimension)
        {
            int splitDimension = Math.Min(dimension, Settings.Dimension);

            var means = new double[dimension];
            var covarianceMatrix = new double[dimension, dimension];

            for (int i = 0; i < splitDimension; i++)
            {
                means[i] = Settings.Means[i];
            }

            for (int i = 0; i < splitDimension; i++)
            {
                for (int j = 0; j < splitDimension; j++)
                {
                    covarianceMatrix[i, j] = Settings.CovarianceMatrix[i, j];
                }
            }

            for (int i = splitDimension; i < dimension; i++)
            {
                covarianceMatrix[i, i] = 1;
            }

            UpdateSettings(means, covarianceMatrix);

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

        public void UpdateBindings()
        {
            MeanBindings = OneDimensionalArrayBinding<double>.GetArrayBindings(Settings.Means);
            CovarianceBindings = TwoDimesionalArrayBinding<double>.GetArrayBindings(Settings.CovarianceMatrix);
        }

        protected abstract void UpdateSettings(double[] means, double[,] covarianceMatrix);
    }

    public class MultivariateNormalSettingsSource : MultivariateSettingsSource
    {
        public MultivariateNormalSettingsSource(MultivariateNormalDistributionSettings settings)
            : base(settings)
        {
        }

        protected override void UpdateSettings(double[] means, double[,] covarianceMatrix)
        {
            Settings = new MultivariateNormalDistributionSettings(means, covarianceMatrix);
        }
    }

    public class MultivariateTSettingsSource : MultivariateSettingsSource
    {
        // TODO: make it neat
        private int degreesOfFreedom;

        public MultivariateTSettingsSource(MultivariateTDistributionSettings settings)
            : base(settings)
        {
        }

        public int DegreesOfFreedom
        {
            get => degreesOfFreedom;
            set
            {
                degreesOfFreedom = value;
                UpdateDimensions(Dimension);
            }
        }

        protected override void UpdateSettings(double[] means, double[,] covarianceMatrix)
        {
            Settings = new MultivariateTDistributionSettings(means, covarianceMatrix, DegreesOfFreedom);
        }
    }
}
