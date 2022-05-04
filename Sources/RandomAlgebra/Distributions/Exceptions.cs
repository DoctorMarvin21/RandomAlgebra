using System;

namespace RandomAlgebra.Distributions
{
    internal enum DistributionsInvalidOperationExceptionType
    {
        ForCorrelationPairMultivariateDistributionMustBeTwoDimensional,
        DivisionByZero,
        MultiplyRandomByZero,
        ExponentialOfRandomInZeroPower,
        ExponentialOfNotPositiveRandomInIrrationalPower,
        ExponentialOfZeroCrossingRandomInNegativePower,
        ExponentialOfZeroInRandomPower,
        ExponentialOfOneInRandomPower,
        ExponentialOfNegativeInRandomPower,
        DivisionOfZero,
        DivisionByZeroCrossingRandom,
        LogarithmWithZeroBase,
        LogarithmWithOneBase,
        LogarithmWithNegativeBase,
        LogarithmOfNotPositiveRandom,
        LogarithmWithNotPositiveRandomBase,
        LogarithmWithOneCrossingRandomBase,
        LogarithmOfZeroValue,
        LogarithmOfNegativeValue,
        TangentOfValueCrossingAsymptote
    }

    internal enum DistributionsArgumentExceptionType
    {
        SamplesNumberMustBeGreaterThenTwo,
        LowerBoundIsGreaterThenUpperBound,
        StandardDeviationMustBeGreaterThenZero,
        StandardDeviationOfFirstDistributionMustBeGreaterThenZero,
        StandardDeviationOfSecondDistributionMustBeGreaterThenZero,
        ShapeParameterMustBeGreaterThenZero,
        ShapeParameterAMustBeGreaterThenZero,
        ShapeParameterBMustBeGreaterThenZero,
        ScaleParameterMustBeGreaterThenZero,
        RateParameterMustBeGreaterThenZero,
        CorrelationMustBeInRangeFromMinusOneToOne,
        DegreesOfFreedomMustNotBeLessThenOne,
        DegreesOfFreedomMustBeGreaterThenZero,
        NumberOfExperimentsMustBeGreaterThenTwo,
        NumberOfPocketsMustBeGreaterThenTwo,
        ParameterValueIsMissing,
        ArgumentSpecifiedSeveralTimes,
        NegativeStep,
        ProbabilityMustBeInRangeFromZeroToOne,
        CovarianceMatrixMustBeSquare,
        CovarianceMatrixMustBeSymmetric,
        CovarianceMatrixMustBePositiveDefined,
        LengthOfArgumentsMustBeGreaterThenTwo,
        LengthOfValuesMustBeGreaterThenTwo,
        LengthOfArgumentsMustBeEqualToLengthOfValues,
        VectorOfMeansMustBeEqualToDimension,
        VectorOfCoeffitientsMustBeEqualToDimension,
        ForCorrelationPairBothOfDistributionsMustBeContinuous,
        BivariateTDistributionMustHaveSameDegreesOfFreedom
    }

    /// <summary>
    /// Invalid operation exception for distributions.
    /// </summary>
    public class DistributionsInvalidOperationException : InvalidOperationException
    {
        internal DistributionsInvalidOperationException()
        {
        }

        internal DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType exceptionType)
            : base(Resources.GetMessage(exceptionType.ToString()))
        {
        }
    }

    /// <summary>
    /// Argument exception for distributions.
    /// </summary>
    public class DistributionsArgumentException : ArgumentException
    {
        internal DistributionsArgumentException(DistributionsArgumentExceptionType exceptionType)
            : base(Resources.GetMessage(exceptionType.ToString()))
        {
        }

        internal DistributionsArgumentException(string message)
            : base(message)
        {
        }

        internal DistributionsArgumentException(DistributionsArgumentExceptionType exceptionType, params object[] arguments)
            : base(string.Format(Resources.GetMessage(exceptionType.ToString()), arguments))
        {
        }
    }
}
