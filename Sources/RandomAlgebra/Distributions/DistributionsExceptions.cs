using System;
using System.Reflection;
using System.Resources;

namespace RandomAlgebra
{
    internal static class ExceptionMessages
    {
        private static ResourceManager resourceManager
            = new ResourceManager("RandomAlgebra.Resources.ExceptionMessages",
                Assembly.GetAssembly(typeof(DistributionsArgumentException)));

        public static string GetExceptionMessage(string resourceKey)
        {
            return resourceManager.GetString(resourceKey);
        }
    }

    internal enum DistributionsInvalidOperationExceptionType
    {
        ImpossibeToUseRandomAlgebraParameterSetMoreThenOnce,
        ExpressionOpreatorsInconsistent,
        CorrelationParamtersIgnored,
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
        MissingExpression,
        UnknownSymbolInExpression,
        NegativeStep,
        ProbabilityMustBeInRangeFromZeroToOne,
        CovarianceMatrixMustBeSquare,
        CovarianceMatrixMustBeSymmetric,
        CovarianceMatrixMustBePositiveDefined,
        LengthOfArgumentsMustBeGreaterThenTwo,
        LengthOfValuesMustBeGreaterThenTwo,
        LengthOfArgumentsMustBeEqualToLengthOfValues,
        LengthOfArgumentsMustBeEqualToLengthOfParameters,
        VectorOfMeansMustBeEqualToDimension,
        VectorOfCoeffitientsMustBeEqualToDimension,
        ForCorrelationPairBothOfDistributionsMustBeContinuous

    }

    /// <summary>
    /// Invalid operation exception for distributions
    /// </summary>
    public class DistributionsInvalidOperationException : InvalidOperationException
    {

        internal DistributionsInvalidOperationException()
        {
        }

        internal DistributionsInvalidOperationException(DistributionsInvalidOperationExceptionType exceptionType)
            : base(ExceptionMessages.GetExceptionMessage(exceptionType.ToString()))
        {
        }
    }

    /// <summary>
    /// Argument exception for distributions.
    /// </summary>
    public class DistributionsArgumentException : ArgumentException
    {
        internal DistributionsArgumentException(DistributionsArgumentExceptionType exceptionType)
            : base(ExceptionMessages.GetExceptionMessage(exceptionType.ToString()))
        {
        }

        internal DistributionsArgumentException(DistributionsArgumentExceptionType exceptionType, params object[] arguments)
            : base(string.Format(ExceptionMessages.GetExceptionMessage(exceptionType.ToString()), arguments))
        {
        }
    }
}
