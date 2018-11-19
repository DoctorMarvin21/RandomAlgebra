using RandomAlgebra.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomAlgebra.DistributionsEvaluation
{
    internal static class Evaluation
    {
        public static double EvaluateDoubleBinary(double left, double right, NodeOperationType operationType)
        {
            switch (operationType)
            {
                case NodeOperationType.Sum:
                    return left + right;
                case NodeOperationType.Substract:
                    return left - right;
                case NodeOperationType.Multiply:
                    return left * right;
                case NodeOperationType.Divide:
                    return left / right;
                case NodeOperationType.Power:
                    return Math.Pow(left, right);
                case NodeOperationType.Log:
                    return Math.Log(left, right);
                default:
                    throw new NotImplementedException();
            }
        }
        public static double EvaluateDoubleUnary(double value, NodeOperationType operationType)
        {
            switch (operationType)
            {
                case NodeOperationType.Negate:
                    return -value;
                case NodeOperationType.Sqrt:
                    return Math.Sqrt(value);
                case NodeOperationType.Abs:
                    return Math.Abs(value);
                case NodeOperationType.Lg10:
                    return Math.Log10(value);
                case NodeOperationType.Ln:
                    return Math.Log(value);
                case NodeOperationType.Sin:
                    return Math.Sin(value);
                default:
                    throw new NotImplementedException();
            }
        }

        public static BaseDistribution DistributionUnary(BaseDistribution value, NodeOperationType operationType)
        {
            if (value.InnerDistributionType == DistributionType.Number)
            {
                return EvaluateDoubleUnary((double)value, operationType);
            }
            else
            {
                switch (operationType)
                {
                    case NodeOperationType.Abs:
                        {
                            return value.InnerGetAbs();
                        }
                    case NodeOperationType.Negate:
                        {
                            return value.InnerGetNegate();
                        }
                    case NodeOperationType.Sqrt:
                        {
                            return value.InnerGetPower(0.5);
                        }
                    case NodeOperationType.Ln:
                        {
                            return value.InnerGetLog(Math.E);
                        }
                    case NodeOperationType.Lg10:
                        {
                            return value.InnerGetLog(10);
                        }
                    case NodeOperationType.Sin:
                        {
                            return CommonRandomMath.Sin(value);
                        }
                    case NodeOperationType.Cos:
                        {
                            return CommonRandomMath.Cos(value);
                        }
                    case NodeOperationType.Tan:
                        {
                            return CommonRandomMath.Tan(value);
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }

        public static BaseDistribution DistributionBinary(BaseDistribution left, BaseDistribution right, NodeOperationType operationType)
        {
            if (left.InnerDistributionType == DistributionType.Number && right.InnerDistributionType == DistributionType.Number)
            {
                return EvaluateDoubleBinary((double)left, (double)right, operationType);
            }
            else
            {
                switch (operationType)
                {
                    case NodeOperationType.Sum:
                        {

                            return left + right;
                        }
                    case NodeOperationType.Substract:
                        {
                            return left - right;
                        }
                    case NodeOperationType.Multiply:
                        {
                            return left * right;
                        }
                    case NodeOperationType.Divide:
                        {
                            return left / right;
                        }
                    case NodeOperationType.Power:
                        {
                            return left.InnerGetPower(right);
                        }
                    case NodeOperationType.Log:
                        {
                            return left.InnerGetLog(right);
                        }
                    default:
                        {
                            throw new NotImplementedException();
                        }
                }
            }
        }

        public static BaseDistribution DistributionBinaryCorrelated(BivariateContinuousDistribution bivariate, NodeOperationType operationType)
        {
            switch (operationType)
            {
                case NodeOperationType.Sum:
                    {
                        return bivariate.GetSum();
                    }
                case NodeOperationType.Substract:
                    {
                        return bivariate.GetDifference();
                    }
                case NodeOperationType.Multiply:
                    {
                        return bivariate.GetProduct();
                    }
                case NodeOperationType.Divide:
                    {
                        return bivariate.GetRatio();
                    }
                case NodeOperationType.Power:
                    {
                        return bivariate.GetPower();
                    }
                case NodeOperationType.Log:
                    {
                        return bivariate.GetLog();
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }
    }
}
