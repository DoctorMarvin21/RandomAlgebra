﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RandomAlgebra.Distributions;

namespace RandomAlgebra.DistributionsEvaluation
{
    internal enum NodeOperationType
    {
        Constant,
        Parameter,
        Sum,
        Substract,
        Multiply,
        Divide,
        Negate,
        Power,
        Log,
        Lg10,
        Ln,
        Sqrt,
        Abs,
        Sin,
        Cos,
        Tan
    }

    internal class NodeOperation
    {
        public NodeOperation()
        {
        }

        public NodeOperation(NodeOperation left, NodeOperationType operationType)
        {
            Left = left;
            OperationType = operationType;
        }

        public NodeOperation(NodeOperation left, NodeOperation right, NodeOperationType operationType)
        {
            Left = left;
            Right = right;
            OperationType = operationType;
        }

        public bool IsUnary
        {
            get
            {
                return Right == null;
            }
        }

        public NodeOperationType OperationType
        {
            get;
            protected set;
        }

        public NodeOperation Left
        {
            get;
            set;
        }

        public NodeOperation Right
        {
            get;
            set;
        }

        public virtual double Evaluate()
        {
            if (IsUnary)
            {
                return Evaluation.EvaluateDoubleUnary(Left.Evaluate(), OperationType);
            }
            else
            {
                return Evaluation.EvaluateDoubleBinary(Left.Evaluate(), Right.Evaluate(), OperationType);
            }
        }

        public virtual BaseDistribution EvaluateExtended(List<CorrelatedPair> correlations)
        {
            if (IsUnary)
            {
                var left = Left.EvaluateExtended(correlations);

                var result = Evaluation.DistributionUnary(left, OperationType);
                return result;
            }
            else
            {
                var left = Left.EvaluateExtended(correlations);
                var right = Right.EvaluateExtended(correlations);

                var baseCorrelation = correlations?.Find(x => (x.BaseLeft.BaseDistribution == (left as ContinuousDistribution)?.BaseDistribution
                    && x.BaseRight.BaseDistribution == (right as ContinuousDistribution)?.BaseDistribution) ||
                    (x.BaseLeft.BaseDistribution == (right as ContinuousDistribution)?.BaseDistribution
                    && x.BaseRight.BaseDistribution == (left as ContinuousDistribution)?.BaseDistribution));

                if (baseCorrelation != null)
                {
                    baseCorrelation.Used = true;
                    var correlation = new CorrelatedPair(left, right, baseCorrelation.Correlation);
                    return Evaluation.DistributionBinaryCorrelated(correlation.GetBivariate(), OperationType);
                }
                else
                {
                    var result = Evaluation.DistributionBinary(left, right, OperationType);
                    return result;
                }
            }
        }

        public virtual Expression ToExpression()
        {
            switch (OperationType)
            {
                case NodeOperationType.Sum:
                    {
                        return Expression.Add(Left.ToExpression(), Right.ToExpression());
                    }
                case NodeOperationType.Substract:
                    {
                        return Expression.Subtract(Left.ToExpression(), Right.ToExpression());
                    }
                case NodeOperationType.Divide:
                    {
                        return Expression.Divide(Left.ToExpression(), Right.ToExpression());
                    }
                case NodeOperationType.Multiply:
                    {
                        return Expression.Multiply(Left.ToExpression(), Right.ToExpression());
                    }
                case NodeOperationType.Power:
                    {
                        return Expression.Power(Left.ToExpression(), Right.ToExpression());
                    }
                case NodeOperationType.Log:
                    {
                        return CustomExpression.Log(Left.ToExpression(), Right.ToExpression());
                    }
                case NodeOperationType.Negate:
                    {
                        return Expression.Negate(Left.ToExpression());
                    }
                case NodeOperationType.Sqrt:
                    {
                        return Expression.Power(Left.ToExpression(), Expression.Constant(0.5d));
                    }
                case NodeOperationType.Abs:
                    {
                        return CustomExpression.Abs(Left.ToExpression());
                    }
                case NodeOperationType.Lg10:
                    {
                        return CustomExpression.Lg10(Left.ToExpression());
                    }
                case NodeOperationType.Ln:
                    {
                        return CustomExpression.Ln(Left.ToExpression());
                    }
                case NodeOperationType.Sin:
                    {
                        return CustomExpression.Sin(Left.ToExpression());
                    }
                case NodeOperationType.Cos:
                    {
                        return CustomExpression.Cos(Left.ToExpression());
                    }
                case NodeOperationType.Tan:
                    {
                        return CustomExpression.Tan(Left.ToExpression());
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
    }

    internal class NodeConstant : NodeOperation
    {
        public NodeConstant(double value)
        {
            Value = value;
            OperationType = NodeOperationType.Constant;
        }

        public double Value
        {
            get;
            set;
        }

        public override Expression ToExpression()
        {
            return Expression.Constant(Value);
        }

        public override BaseDistribution EvaluateExtended(List<CorrelatedPair> correlations)
        {
            return Value;
        }

        public override double Evaluate()
        {
            return Value;
        }
    }

    internal class NodeParameter : NodeOperation
    {
        public NodeParameter(string parameter, int index, ParameterExpression expression)
        {
            Parameter = parameter;
            OperationType = NodeOperationType.Parameter;
            ParameterIndex = index;
            ParameterExpression = expression;
        }

        public string Parameter
        {
            get;
            set;
        }

        public ParameterExpression ParameterExpression
        {
            get;
            set;
        }

        public int ParameterIndex
        {
            get;
            set;
        }

        public BaseDistribution Value
        {
            get;
            set;
        }

        public int Count
        {
            get;
            set;
        } = 1;

        public override BaseDistribution EvaluateExtended(List<CorrelatedPair> correlations)
        {
            if (Value == null)
            {
                throw new DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType.ParameterValueIsMissing, Parameter);
            }

            return Value;
        }

        public override Expression ToExpression()
        {
            return Expression.ArrayIndex(ParameterExpression, Expression.Constant(ParameterIndex));
        }

        public override double Evaluate()
        {
            if (Value == null)
            {
                throw new DistributionsEvaluatorArgumentException(DistributionsEvaluatorArgumentExceptionType.ParameterValueIsMissing, Parameter);
            }

            return (double)Value;
        }
    }
}
