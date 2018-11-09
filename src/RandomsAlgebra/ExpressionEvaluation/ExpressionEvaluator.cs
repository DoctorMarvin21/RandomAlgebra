using RandomsAlgebra.Distributions;
using RandomsAlgebra.Distributions.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace RandomsAlgebra.DistributionsEvaluation
{
    /// <summary>
    /// Propagation of distributions by the method of algebra of random variables that is a combination of analytical and numberical (include numerical integration) methods
    /// </summary>
    public class DistributionsEvaluator
    {
        readonly Stack<NodeOperation> _nodeStack = new Stack<NodeOperation>();
        readonly Stack<Symbol> _operatorStack = new Stack<Symbol>();
        readonly List<NodeParameter> _nodeParameters = new List<NodeParameter>();
        readonly ParameterExpression _parameterExpression = Expression.Parameter(typeof(double[]), "args");
        readonly NodeOperation _parsed = null;
        readonly Func<double[], double> _compiled = null;
        const char _decimalSeparator = '.';

        /// <summary>
        /// Creates instance of distributions evaluatior
        /// </summary>
        /// <param name="modelExpression">Model expression, e.g. "A+B*3+1", where A and B, are parameters of function
        public DistributionsEvaluator(string modelExpression)
        {
            if (string.IsNullOrWhiteSpace(modelExpression))
                throw new DistributionsArgumentException("Missing expression", "Выражение не задано");

            ExpressionText = modelExpression;
            _parsed = Parse(modelExpression);

            _compiled = Compile(_parsed.ToExpression());

            Parameters = _nodeParameters.Select(x => x.Parameter).ToArray();
        }

        #region Calculations
        /// <summary>
        /// Performs propagation of distributions setted as arguments
        /// </summary>
        /// <param name="arguments">Dictionary of pairs of distributions and arguments</param>
        /// <returns>Propagation result</returns>
        public BaseDistribution EvaluateDistributions(Dictionary<string, BaseDistribution> arguments)
        {
            arguments = arguments ?? new Dictionary<string, BaseDistribution>();

            int length = _nodeParameters.Count;

            for (int i = 0; i < length; i++)
            {
                var parameter = _nodeParameters[i];

                string arg = parameter.Parameter;

                if (parameter.Count > 1)
                    throw new DistributionsInvalidOperationException($"It is impossible to perform the propagation by the method of algebra of random variables, since the parameter \"{ arg }\" occurs more than once ", $"Невозможно выполнить транформацию методом алгебры случайных величин, так как параметр \"{arg}\" встречается более одного раза");

				BaseDistribution value;
                if (arguments.TryGetValue(arg, out value))
                {
                    parameter.Value = value;
                }
                else
                {
                    throw new DistributionsArgumentException($"Parameter value \"{arg}\" value is missing", $"Отсутствует значение параметра \"{arg}\"");
                }

            }

            var result = _parsed.EvaluateExtended();

            return result;
        }

        internal double EvaluateCompiled(Dictionary<string, double> arguments)
        {
            arguments = arguments ?? new Dictionary<string, double>();


            int length = _nodeParameters.Count;
            double[] args = new double[length];

            for (int i = 0; i < length; i++)
            {
                var parameter = _nodeParameters[i];

                string arg = parameter.Parameter;
				double value;
                if (arguments.TryGetValue(arg, out value))
                {
                    args[i] = value;
                }
                else
                {
                    throw new DistributionsArgumentException($"Parameter value \"{arg}\" value is missing", $"Отсутствует значение параметра \"{arg}\"");
                }

            }

            return _compiled(args);
        }

        internal double EvaluateCompiled(double[] arguments)
        {
            if (arguments.Length != _nodeParameters.Count)
                throw new DistributionsArgumentException($"The number of arguments {arguments.Length} does not match the number of parameters {_nodeParameters.Count}", $"Число аргументов {arguments.Length} не соответствует числу параметров {_nodeParameters.Count}");

            return _compiled(arguments);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Parameters that must be setted as arguments
        /// </summary>
        public string[] Parameters { get; } = null;

        /// <summary>
        /// Model expression
        /// </summary>
        public string ExpressionText { get; } = null;

        internal NodeOperation Parsed
        {
            get
            {
                return _parsed;
            }
        }
        #endregion

        #region Parsing expression
        private NodeOperation Parse(string expression)
        {
            //simply useful 
            expression = expression.Replace(',', _decimalSeparator).Replace(" ", string.Empty);

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
                        _nodeStack.Push(ReadOperand(reader));
                        continue;
                    }

                    if (char.IsLetter(next))
                    {
                        PushParameter(reader);
                        continue;
                    }

                    if (Operator.IsDefined(next))
                    {
                        if (next == '-' && _nodeStack.Count == 0)
                        {
                            reader.Read();
                            _operatorStack.Push(Operator.UnaryMinus);
                            continue;
                        }

                        var currentOperation = ReadOperation(reader);

                        EvaluateWhile(() => _operatorStack.Count > 0 && _operatorStack.Peek() != Parentheses.Left &&
                            currentOperation.Precedence <= ((Operator)_operatorStack.Peek()).Precedence);

                        _operatorStack.Push(currentOperation);
                        continue;
                    }

                    if (next == '(')
                    {

                        reader.Read();

                        _operatorStack.Push(Parentheses.Left);

                        if (reader.Peek() == '-')
                        {
                            reader.Read();
                            _operatorStack.Push(Operator.UnaryMinus);
                        }

                        continue;
                    }

                    if (next == ')')
                    {
                        reader.Read();
                        EvaluateWhile(() => _operatorStack.Count > 0 && _operatorStack.Peek() != Parentheses.Left);
                        _operatorStack.Pop();
                        continue;
                    }


                    throw new DistributionsArgumentException($"Unknown symbol \"{next}\" in expression \"{expression}\"", $"Неизвестный символ \"{next}\" в выражении \"{expression}\"");
                }
            }

            EvaluateWhile(() => _operatorStack.Count > 0);

            return _nodeStack.Pop();
        }

        private void EvaluateWhile(Func<bool> condition)
        {
            try
            {
                while (condition())
                {
                    var operation = (Operator)_operatorStack.Pop();

                    var expressions = new NodeOperation[operation.NumberOfOperands];
                    for (var i = operation.NumberOfOperands - 1; i >= 0; i--)
                    {
                        expressions[i] = _nodeStack.Pop();
                    }

                    _nodeStack.Push(operation.Apply(expressions));
                }
            }
            catch
            {
                throw new DistributionsInvalidOperationException("Expression operators are inconsistent", "Операторы выражения несогласованы");
            }
        }

        private NodeOperation ReadOperand(TextReader reader)
        {
            var operand = string.Empty;

            int peek;

            while ((peek = reader.Peek()) > -1)
            {
                var next = (char)peek;

                if (char.IsDigit(next) || next == _decimalSeparator)
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

                if (char.IsLetter(next))
                {
                    reader.Read();
                    parameter += next;
                }
                else if (next == '(')
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
                _operatorStack.Push((Operator)parameter);
            }
            else
            {
                var found = _nodeParameters.FirstOrDefault(x => x.Parameter == parameter);

                if (found != null)
                {
                    found.Count++;
                    _nodeStack.Push(found);
                }
                else
                {
                    var nodeParameter = new NodeParameter(parameter, _nodeParameters.Count, _parameterExpression);
                    _nodeParameters.Add(nodeParameter);
                    _nodeStack.Push(nodeParameter);
                }
            }
        }
        #endregion

        #region Compiling
        private Func<double[], double> Compile(Expression expression)
        {
            var lambda = Expression.Lambda<Func<double[], double>>(expression, _parameterExpression);
            ;
            var compiled = lambda.Compile();
            return compiled;
        }
        #endregion
    }
}
