namespace DistributionsBlazor
{
    public class DistributionParameter
    {
        public DistributionParameter(string name, double? randomsAlgebra, double? monteCarlo)
        {
            Name = name;
            RandomsAlgebra = randomsAlgebra;
            MonteCarlo = monteCarlo;
        }

        public string Name { get; }

        public double? RandomsAlgebra { get; }

        public double? MonteCarlo { get; }

        public double? PersentRatio => GetPersentRatio(RandomsAlgebra, MonteCarlo);

        private double? GetPersentRatio(double? v1, double? v2)
        {
            if (v1.HasValue && v2.HasValue)
            {
                double v1V = v1.Value;
                double v2V = v2.Value;
                return ((v1V - v2V) / v1V * 100d);
            }
            else
            {
                return null;
            }
        }
    }
}
