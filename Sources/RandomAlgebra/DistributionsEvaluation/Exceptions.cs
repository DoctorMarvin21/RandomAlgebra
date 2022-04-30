using System;

namespace RandomAlgebra.DistributionsEvaluation
{
    internal enum DistributionsEvaluatorInvalidOperationExceptionType
    {
        ImpossibeToUseRandomAlgebraParameterSetMoreThenOnce,
        ExpressionOpreatorsInconsistent,
        CorrelationParamtersIgnored
    }

    internal enum DistributionsEvaluatorArgumentExceptionType
    {
        ParameterValueIsMissing,
        MissingExpression,
        UnknownSymbolInExpression,
        LengthOfArgumentsMustBeEqualToLengthOfParameters
    }

    /// <summary>
    /// Invalid operation exception for expression evaluator.
    /// </summary>
    public class DistributionsEvaluatorInvalidOperationException : InvalidOperationException
    {
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

        internal DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType exceptionType, params object[] arguments)
            : base(string.Format(Resources.GetMessage(exceptionType.ToString()), arguments))
        {
        }
    }
}
