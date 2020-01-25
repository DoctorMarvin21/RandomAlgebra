using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomAlgebra
{
    internal static class CommonExceptions
    {
        public static void ThrowCommonExcepton(CommonExceptionType exceptonType)
        {
            switch (exceptonType)
            {
                case CommonExceptionType.DivisionByZero:
                    throw new DistributionsInvalidOperationException("Division by zero", "Деление на 0");
                case CommonExceptionType.MultiplyRandomByZero:
                    throw new DistributionsInvalidOperationException("Multiplying a random value by zero", "Умножение случайной величины на 0");
                case CommonExceptionType.ExponentialOfRandomInZeroPower:
                    throw new DistributionsInvalidOperationException("Raising the random variable to the zero power", "Возведение случайной величины в степень 0");
                case CommonExceptionType.ExponentialOfNotPositiveRandomInIrrationalPower:
                    throw new DistributionsInvalidOperationException("Raising non-positive random variable to a non-natural power", "Возведение случайной величины, принимающей отрицательные значения, в не натуральную степень");
                case CommonExceptionType.ExponentialOfZeroCrossingRandomInNegativePower:
                    throw new DistributionsInvalidOperationException("Rasing random variable that takes zero to negative power", "Возведение случайной величины, принимающей значение 0, в отрицательную степень");
                case CommonExceptionType.ExponentialOfZeroInRandomPower:
                    throw new DistributionsInvalidOperationException("Rising zero to random power", "Возведение 0 в степень случайной величины");
                case CommonExceptionType.ExponentialOfOneInRandomPower:
                    throw new DistributionsInvalidOperationException("Rising one to random power", "Возведение 1 в степень случайной величины");
                case CommonExceptionType.ExponentialOfNegativeInRandomPower:
                    throw new DistributionsInvalidOperationException("Rising negative number to random power", "Возведение отрицательного числа в степень случайной величины");
                case CommonExceptionType.DivisionOfZero:
                    throw new DistributionsInvalidOperationException("Division of zero by random", "Деление 0 на случайную величину");
                case CommonExceptionType.DivisionByZeroCrossingRandom:
                    throw new DistributionsInvalidOperationException("Division by random that takes zero value", "Делении на случайную величину, принимающую значение 0");
                case CommonExceptionType.LogarithmWithZeroBase:
                    throw new DistributionsInvalidOperationException("Logarithm with zero base", "Логарифм по основанию 0");
                case CommonExceptionType.LogarithmWithOneBase:
                    throw new DistributionsInvalidOperationException("Logarithm with base one", "Логарифм по основанию 1");
                case CommonExceptionType.LogarithmWithNegativeBase:
                    throw new DistributionsInvalidOperationException("Logarithm with negative base", "Логарифм по отрицательному основанию");
                case CommonExceptionType.LogarithmOfNotPositiveRandom:
                    throw new DistributionsInvalidOperationException("Logarithm of non-positive or taking zero random value", "Логарифм от случайной величины, принимающей отрицательные значения или 0");
                case CommonExceptionType.LogarithmWithNotPositiveRandomBase:
                    throw new DistributionsInvalidOperationException("Logarithm with non-positive or taking zero random value base", "Логарифм по основанию случайной величины, принимающей отрицательные значения или 0");
                case CommonExceptionType.LogarithmWithOneCrossingRandomBase:
                    throw new DistributionsInvalidOperationException("Logarithm with taking one random value base", "Логарифм по основанию случайной величины, принимающей значение 1");
                case CommonExceptionType.LogarithmOfZeroValue:
                    throw new DistributionsInvalidOperationException("Logarithm of zero", "Логарифм от 0");
                case CommonExceptionType.LogarithmOfNegativeValue:
                    throw new DistributionsInvalidOperationException("Logarithm of a negative number", "Логарифм от отрицательного числа");
                case CommonExceptionType.TangentOfValueCrossingAsymptote:
                    throw new DistributionsInvalidOperationException("Tangent of random value crssing asymptote", "Тангенс от случайной величины пересекающей асимптоту");
            }
        }

        public static string Locale
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
        }
    }
    internal enum CommonExceptionType
    {
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

    /// <summary>
    /// Invalid operation exception for distributions
    /// </summary>
    public class DistributionsInvalidOperationException : InvalidOperationException
    {
        string _message = null;
        internal DistributionsInvalidOperationException()
        {
            _message = base.Message;
        }

        internal DistributionsInvalidOperationException(string eng, string rus)
        {
            if (CommonExceptions.Locale == "ru")
            {
                _message = rus;
            }
            else
            {
                _message = eng;
            }
        }

        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }

    /// <summary>
    /// Argument exception for distributions
    /// </summary>
    public class DistributionsArgumentException : ArgumentException
    {
        string _message = null;

        internal DistributionsArgumentException(string eng, string rus)
        {
            if (CommonExceptions.Locale == "ru")
            {
                _message = rus;
            }
            else
            {
                _message = eng;
            }
        }

        public override string Message
        {
            get
            {
                return _message;
            }
        }
    }
}
