using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using RandomAlgebra.Distributions;

namespace RandomAlgebra.DistributionsEvaluation
{
    /// <summary>
    /// Propagation of distributions by the method of algebra of random variables that is
    /// a combination of analytical and numerical (include numerical integration) methods.
    /// </summary>
    public class DistributionsEvaluator
    {
        private const char ParenthesisLeft = ')';
        private const char ParenthesisRight = '(';
        private const char DecimalSeparator = '.';
        private const char Comma = ',';
        private const char Minus = '-';
        private const string PiConstant = "pi";
        private const string ExponentConstant = "exp";

        private readonly Stack<NodeOperation> nodeStack = new Stack<NodeOperation>();
        private readonly Stack<Symbol> operatorStack = new Stack<Symbol>();
        private readonly List<NodeParameter> nodeParameters = new List<NodeParameter>();
        private readonly ParameterExpression parameterExpression = Expression.Parameter(typeof(double[]), "args");
        private readonly Func<double[], double> compiled = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionsEvaluator"/> class.
        /// </summary>
        /// <param name="modelExpression">Model expression, e.g. "A+B*3+1", where A and B, are parameters of function.</param>
        public DistributionsEvaluator(string modelExpression)
        {
            if (string.IsNullOrWhiteSpace(modelExpression))
            {
                throw new DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType.MissingExpression);
            }

            ExpressionText = modelExpression;
            Parsed = Parse(modelExpression);

            compiled = Compile(Parsed.ToExpression());

            Parameters = nodeParameters.Select(x => x.Parameter).ToArray();
        }

        #region Properties

        /// <summary>
        /// Parameters that must be set as arguments.
        /// </summary>
        public string[] Parameters { get; }

        /// <summary>
        /// Model expression.
        /// </summary>
        public string ExpressionText { get; }

        internal NodeOperation Parsed { get; }

        #endregion

        #region Calculations

        /// <summary>
        /// Performs propagation of distributions set as arguments.
        /// </summary>
        /// <param name="arguments">Dictionary of pairs of distributions and arguments.</param>
        /// <param name="correlations">Correlation coefficients for distribution arguments.</param>
        /// <returns>Propagation result.</returns>
        public BaseDistribution EvaluateDistributions(Dictionary<string, BaseDistribution> arguments, List<CorrelatedPair> correlations)
        {
            correlations?.ForEach(x => x.Used = false);

            arguments = arguments ?? new Dictionary<string, BaseDistribution>();

            int length = nodeParameters.Count;

            for (int i = 0; i < length; i++)
            {
                NodeParameter parameter = nodeParameters[i];

                string arg = parameter.Parameter;

                if (parameter.Count > 1)
                {
                    throw new DistributionsEvaluatorInvalidOperationException(DistributionsEvaluatorInvalidOperationExceptionType.ImpossibeToUseRandomAlgebraParameterSetMoreThenOnce, arg);
                }

                if (arguments.TryGetValue(arg, out BaseDistribution value))
                {
                    parameter.Value = value;
                }
                else
                {
                    throw new DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType.ParameterValueIsMissing, arg);
                }
            }

            BaseDistribution result = Parsed.EvaluateExtended(correlations);

            if (correlations.Any(x => !x.Used))
            {
                throw new DistributionsEvaluatorInvalidOperationException(DistributionsEvaluatorInvalidOperationExceptionType.CorrelationParamtersIgnored);
            }

            return result;
        }

        internal double EvaluateCompiled(Dictionary<string, double> arguments)
        {
            arguments = arguments ?? new Dictionary<string, double>();

            int length = nodeParameters.Count;
            double[] args = new double[length];

            for (int i = 0; i < length; i++)
            {
                var parameter = nodeParameters[i];

                string arg = parameter.Parameter;

                if (arguments.TryGetValue(arg, out double value))
                {
                    args[i] = value;
                }
                else
                {
                    throw new DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType.ParameterValueIsMissing, arg);
                }
            }

            return compiled(args);
        }

        internal double EvaluateCompiled(double[] arguments)
        {
            if (arguments.Length != nodeParameters.Count)
            {
                throw new DistributionsEvaluatorArgumentException(
                    DistributionsEvaluatorArgumentExceptionType.LengthOfArgumentsMustBeEqualToLengthOfParameters,
                    arguments.Length,
                    nodeParameters.Count);
            }

            return compiled(arguments);
        }

        #endregion

        #region Parsing expression

        private NodeOperation Parse(string expression)
        {
            // Simply useful
            expression = expression.Replace(Comma, DecimalSeparator).Replace(" ", string.Empty);

            if (string.IsNullOrWhiteSpace(expression))
            {
                return new NodeConstant(0);
            }

            using (var reader = new StringReader(expression))
            {
                int peek;

                while ((peek = reader.Peek()) > -1)
                {
                    var next = (char)peek;

                    if (char.IsDigit(next))
                    {
                        nodeStack.Push(ReadOperand(reader));
                        continue;
                    }

                    if (char.IsLetter(next))
                    {
                        PushParameter(reader);
                        continue;
                    }

                    if (Operator.IsDefined(next))
                    {
                        if (next == Minus && nodeStack.Count == 0)
                        {
                            reader.Read();
                            operatorStack.Push(Operator.UnaryMinus);
                            continue;
                        }

                        var currentOperation = ReadOperation(reader);

                        EvaluateWhile(() => operatorStack.Count > 0 && operatorStack.Peek() != Parentheses.Left &&
                            currentOperation.Precedence <= ((Operator)operatorStack.Peek()).Precedence);

                        operatorStack.Push(currentOperation);
                        continue;
                    }

                    if (next == ParenthesisRight)
                    {
                        reader.Read();

                        operatorStack.Push(Parentheses.Left);

                        if (reader.Peek() == Minus)
                        {
                            reader.Read();
                            operatorStack.Push(Operator.UnaryMinus);
                        }

                        continue;
                    }

                    if (next == ParenthesisLeft)
                    {
                        reader.Read();
                        EvaluateWhile(() => operatorStack.Count > 0 && operatorStack.Peek() != Parentheses.Left);
                        operatorStack.Pop();
                        continue;
                    }

                    throw new DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType.UnknownSymbolInExpression, next, expression);
                }
            }

            EvaluateWhile(() => operatorStack.Count > 0);

            return nodeStack.Pop();
        }

        private void EvaluateWhile(Func<bool> condition)
        {
            try
            {
                while (condition())
                {
                    var operation = (Operator)operatorStack.Pop();

                    var expressions = new NodeOperation[operation.NumberOfOperands];
                    for (var i = operation.NumberOfOperands - 1; i >= 0; i--)
                    {
                        expressions[i] = nodeStack.Pop();
                    }

                    nodeStack.Push(operation.Apply(expressions));
                }
            }
            catch
            {
                throw new DistributionsEvaluatorInvalidOperationException(DistributionsEvaluatorInvalidOperationExceptionType.ExpressionOpreatorsInconsistent);
            }
        }

        private NodeOperation ReadOperand(TextReader reader)
        {
            var operand = string.Empty;

            int peek;

            while ((peek = reader.Peek()) > -1)
            {
                var next = (char)peek;

                if (char.IsDigit(next) || next == DecimalSeparator)
                {
                    reader.Read();
                    operand += next;
                }
                else
                {
                    break;
                }
            }

            return new NodeConstant(double.Parse(operand, NumberStyles.Any, CultureInfo.InvariantCulture));
        }

        private Operator ReadOperation(TextReader reader)
        {
            var operation = (char)reader.Read();
            return (Operator)operation;
        }

        private void PushParameter(TextReader reader)
        {
            var parameter = string.Empty;

            int peek;
            bool nextParenthesis = false;

            while ((peek = reader.Peek()) > -1)
            {
                var next = (char)peek;

                if (char.IsLetter(next) || (parameter.Length > 0 && char.IsDigit(next)))
                {
                    reader.Read();
                    parameter += next;
                }
                else if (next == ParenthesisRight)
                {
                    nextParenthesis = true;
                    break;
                }
                else
                {
                    break;
                }
            }
            if (nextParenthesis && Operator.IsDefinedFunction(parameter))
            {
                operatorStack.Push((Operator)parameter);
            }
            else
            {
                if (parameter == PiConstant)
                {
                    nodeStack.Push(new NodeConstant(Math.PI));
                }
                else if (parameter == ExponentConstant)
                {
                    nodeStack.Push(new NodeConstant(Math.E));
                }
                else
                {
                    var found = nodeParameters.FirstOrDefault(x => x.Parameter == parameter);

                    if (found != null)
                    {
                        found.Count++;
                        nodeStack.Push(found);
                    }
                    else
                    {
                        var nodeParameter = new NodeParameter(parameter, nodeParameters.Count, parameterExpression);
                        nodeParameters.Add(nodeParameter);
                        nodeStack.Push(nodeParameter);
                    }
                }
            }
        }

        #endregion

        #region Compiling

        private Func<double[], double> Compile(Expression expression)
        {
            var lambda = Expression.Lambda<Func<double[], double>>(expression, parameterExpression);

            var compiled = lambda.Compile();
            return compiled;
        }

        #endregion
    }
}
