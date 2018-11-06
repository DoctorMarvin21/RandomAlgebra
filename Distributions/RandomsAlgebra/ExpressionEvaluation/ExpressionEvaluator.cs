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
    /// Выполняет трансформацию распределений композиционным методом по заданной формуле модели
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
        /// Создает экземпляр класса трансформации по формуле модели
        /// </summary>
        /// <param name="expression">Формула модели, например A+B*3+1, где A и B, это аргументы функции
        public DistributionsEvaluator(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
                throw new ArgumentException("Выражение не задано");

            ExpressionText = expression;
            _parsed = Parse(expression);

            _compiled = Compile(_parsed.ToExpression());

            Parameters = _nodeParameters.Select(x => x.Parameter).ToArray();
        }

        #region Вычисление
        /// <summary>
        /// Выполняет трансформацию законов распределения, переданных в качестве аргуметов
        /// </summary>
        /// <param name="arguments">Пары значений агрумента формулы модели и распределения</param>
        /// <returns>Результат трансформации</returns>
        public BaseDistribution EvaluateDistributions(params KeyValuePair<string, BaseDistribution>[] arguments)
        {
            var dictionary = arguments.ToDictionary(x => x.Key, kvp => kvp.Value);
            return EvaluateDistributions(dictionary);
        }

        /// <summary>
        /// Выполняет трансформацию законов распределения, переданных в качестве аргуметов
        /// </summary>
        /// <param name="arguments">Словарь пар значений агрумента формулы и распределения</param>
        /// <returns>Результат трансформации</returns>
        public BaseDistribution EvaluateDistributions(Dictionary<string, BaseDistribution> arguments)
        {
            arguments = arguments ?? new Dictionary<string, BaseDistribution>();

            int length = _nodeParameters.Count;

            for (int i = 0; i < length; i++)
            {
                var parameter = _nodeParameters[i];

                string arg = parameter.Parameter;

                if (parameter.Count > 1)
                    throw new InvalidOperationException($"Невозможно выполнить рассчет композиционным методом, так как переменная \"{arg}\" встречается более одного раза");

				BaseDistribution value;
                if (arguments.TryGetValue(arg, out value))
                {
                    parameter.Value = value;
                }
                else
                {
                    throw new ArgumentException($"Отсутствует значение переменной \"{arg}\"");
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
                    throw new ArgumentException($"Отсутствует значение переменной \"{arg}\"");
                }

            }

            return _compiled(args);
        }

        internal double EvaluateCompiled(double[] arguments)
        {
            if (arguments.Length != _nodeParameters.Count)
                throw new ArgumentException($"Число агрументов {arguments.Length} не соответствует числу параметров {_nodeParameters.Count}");

            return _compiled(arguments);
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Переменные, которые необходимо задать в агрументах
        /// </summary>
        public string[] Parameters { get; } = null;

        /// <summary>
        /// Формула модели
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

        #region Компиляция
        private Func<double[], double> Compile(Expression expression)
        {
            var lambda = Expression.Lambda<Func<double[], double>>(expression, _parameterExpression);
            ;
            var compiled = lambda.Compile();
            return compiled;
        }

        private Func<double[], double> CompileToMethod(Expression expression)
        {
            var lambda = Expression.Lambda<Func<double[], double>>(expression, _parameterExpression);

            string assemblyName = "ExpressionCompliler";
            string className = "CompileExpression";
            string methodName = "EvaluateExpression";

            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(assemblyName), AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName);
            TypeBuilder bindingsClass = moduleBuilder.DefineType(className, TypeAttributes.Class | TypeAttributes.Public);

            var method = bindingsClass.DefineMethod(methodName, MethodAttributes.Public | MethodAttributes.Static, typeof(double), new Type[] { typeof(double[]) });
            lambda.CompileToMethod(method);
            bindingsClass.CreateType();

            var mi = bindingsClass.GetMethod(methodName);
            var func = Delegate.CreateDelegate(typeof(Func<double[], double>), mi);
            return (Func<double[], double>)func;
        }
        #endregion

        #region Парсинг выражения

        private NodeOperation Parse(string expression)
        {
            //просто необходимая вещь
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


                    throw new ArgumentException($"Неизвестный символ \"{next}\" в выражении \"{expression}\"");
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
                throw new Exception("Операторы выражения несогласованы");
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
            //Может, не делать проверку Parenthesis, также сделать проверку, куда уходят несогласованные параметры и что делать с прочей хуйней
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
    }
}
