using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace RandomAlgebra.DistributionsEvaluation
{
    internal static class CustomExpression
    {
        public static Expression Abs(Expression x)
        {
            return Expression.Call(typeof(CustomActions).GetMethod("Abs"), x);
        }
        public static Expression Log(Expression left, Expression right)
        {
            return Expression.Call(typeof(CustomActions).GetMethod("Log"), left, right);
        }
        public static Expression Lg10(Expression x)
        {
            return Expression.Call(typeof(CustomActions).GetMethod("Lg10"), x);
        }
        public static Expression Ln(Expression x)
        {
            return Expression.Call(typeof(CustomActions).GetMethod("Ln"), x);
        }
        public static Expression Sin(Expression x)
        {
            return Expression.Call(typeof(CustomActions).GetMethod("Sin"), x);
        }
        public static Expression Cos(Expression x)
        {
            return Expression.Call(typeof(CustomActions).GetMethod("Cos"), x);
        }
        public static Expression Tan(Expression x)
        {
            return Expression.Call(typeof(CustomActions).GetMethod("Tan"), x);
        }
    }

    internal static class CustomActions
    {
        public static double Abs(double x)
        {
            return Math.Abs(x);
        }

        public static double Sin(double x)
        {
            return Math.Sin(x);
        }
        public static double Cos(double x)
        {
            return Math.Cos(x);
        }
        public static double Tan(double x)
        {
            return Math.Tan(x);
        }
        public static double Log(double x, double newBase)
        {
            return Math.Log(x, newBase);
        }
        public static double Lg10(double x)
        {
            return Math.Log10(x);
        }
        public static double Ln(double x)
        {
            return Math.Log(x);
        }
    }
}
