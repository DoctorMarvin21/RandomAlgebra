namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Optimizations used during numerical integration, true by the default, use for testing.
    /// </summary>
    public static class Optimizations
    {
        /// <summary>
        /// If set to true, on convolution of several distributions (e.g. two normal distributions)
        /// the result of convolution will be derived analytically.
        /// </summary>
        public static bool UseAnalyticalConvolution
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Use FFT convolution (if possible) instead of discrete convolution, if would be faster (N log(N) against N^2),
        /// but, sometimes, less accurate.
        /// </summary>
        public static bool UseFftConvolution
        {
            get;
            set;
        } = false;
    }
}
