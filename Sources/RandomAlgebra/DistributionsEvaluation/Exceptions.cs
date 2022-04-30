using System;

namespace RandomAlgebra.DistributionsEvaluation
{
    internal enum DistributionsEvaluatorInvalidOperationExceptionType
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

    internal enum DistributionsEvaluatorArgumentExceptionType
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
    /// Invalid operation exception for expression evaluator.
    /// </summary>
    public class DistributionsEvaluatorInvalidOperationException : InvalidOperationException
    {
        internal DistributionsEvaluatorInvalidOperationException()
        {
        }

        internal DistributionsEvaluatorInvalidOperationException(DistributionsEvaluatorInvalidOperationExceptionType exceptionType)
            : base(Resources.GetMessage(exceptionType.ToString()))
        {
        }
    }

    /// <summary>
    /// Argument exception for expression evaluator.
    /// </summary>
    public class DistributionsEvaluatorArgumentException : ArgumentException
    {
        internal DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType exceptionType)
            : base(Resources.GetMessage(exceptionType.ToString()))
        {
        }

        internal DistributionsEvaluatorArgumentException(string message)
            : base(message)
        {
        }

        internal DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType exceptionType, params object[] arguments)
            : base(string.Format(Resources.GetMessage(exceptionType.ToString()), arguments))
        {
        }
    }
}
