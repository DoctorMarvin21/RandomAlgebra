using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomAlgebra.Distributions.Settings
{
    /// <summary>
    /// Optimizations used during numerical integration, true by the default, use for tesing
    /// </summary>
    public static class Optimizations
    {

        /// <summary>
        /// If setted to true, on convolution of several distributions (eg. two normal distributions) the result of convolution will be derived analytically
        /// </summary>
        public static bool UseContinuousConvolution
        {
            get;
            set;
        } = true;

        /// <summary>
        /// Use FFT convolution (if possible) instead of discrete convolution, if would be faster (N log(N) against N^2), but, sometimes, less accurate
        /// </summary>
        public static bool UseFFTConvolution
        {
            get;
            set;
        } = true;
    }
}
