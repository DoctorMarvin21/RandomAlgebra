using Accord.Statistics.Distributions.Univariate;
using RandomsAlgebra.Distributions.SpecialDistributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RandomsAlgebra.Distributions
{
    internal static class ContinuousRandomsMath
    {
        public static ContinuousDistribution ConvolutionOfNormalAndNormal(ContinuousDistribution pdfX, ContinuousDistribution pdfY)
        {
            var norm1 = (NormalDistribution)pdfX.BaseDistribution;
            var norm2 = (NormalDistribution)pdfY.BaseDistribution;

            var v1 = norm1.Variance * Math.Pow(pdfX.Coefficient, 2);
            var m1 = norm1.Mean * pdfX.Coefficient + pdfX.Offset;
            var v2 = norm2.Variance * Math.Pow(pdfY.Coefficient, 2);
            var m2 = norm2.Mean * pdfY.Coefficient + pdfY.Offset;

            var newDistribution = new ContinuousDistribution(new NormalDistribution(m1 + m2, Math.Sqrt(v1 + v2)), pdfX.Samples);
            return newDistribution;
        }

        public static ContinuousDistribution ConvolutionOfUniformAndUniform(ContinuousDistribution pdfX, ContinuousDistribution pdfY)
        {
            var unif1 = (UniformContinuousDistribution)pdfX.BaseDistribution;
            var unif2 = (UniformContinuousDistribution)pdfY.BaseDistribution;

            double a1 = unif1.Support.Min * pdfX.Coefficient + pdfX.Offset;
            double b1 = unif1.Support.Max * pdfX.Coefficient + pdfX.Offset;

            if (a1 > b1)
            {
                double t = a1;
                a1 = b1;
                b1 = t;
            }

            double a2 = unif2.Support.Min * pdfY.Coefficient + pdfY.Offset;
            double b2 = unif2.Support.Max * pdfY.Coefficient + pdfY.Offset;

            if (a2 > b2)
            {
                double t = a2;
                a2 = b2;
                b2 = t;
            }

            double a = a1 + a2;
            double b = b1 + b2;

            double l1 = Math.Abs((b1 - a1) - (b2 - a2)) / 2d;
            double l2 = (b - a) / 2d;

            return new ContinuousDistribution(new IsoscelesTrapezoidalDistribution(a, b, l2 - l1), pdfX.Samples);
        }

        public static ContinuousDistribution ConvolutionOfNormalAndUniform(ContinuousDistribution pdfX, ContinuousDistribution pdfY)
        {
            var norm = (NormalDistribution)pdfX.BaseDistribution;
            var unif = (UniformContinuousDistribution)pdfY.BaseDistribution;

            double a = unif.Support.Min * pdfY.Coefficient + pdfY.Offset;
            double b = unif.Support.Max * pdfY.Coefficient + pdfY.Offset;
            var s = Math.Abs(norm.StandardDeviation * pdfX.Coefficient);
            var m = norm.Mean * pdfX.Coefficient + pdfX.Offset;

            return new ContinuousDistribution(new BhattacharjeeDistribution(a, b, m, s), pdfX.Samples);
        }

        public static ContinuousDistribution ConvolutionOfNormalAndBhattacharjee(ContinuousDistribution pdfX, ContinuousDistribution pdfY)
        {
            var norm = (NormalDistribution)pdfX.BaseDistribution;
            var bhat = (BhattacharjeeDistribution)pdfY.BaseDistribution;

            var v1 = norm.Variance * Math.Pow(pdfX.Coefficient, 2);
            var m1 = norm.Mean * pdfX.Coefficient + pdfX.Offset;
            var v2 = Math.Pow(bhat.NormalSigma * pdfY.Coefficient, 2);
            var m2 = bhat.NormalMean * pdfY.Coefficient + pdfY.Offset;

            var a = bhat.UniformMin * pdfY.Coefficient;
            var b = bhat.UniformMax * pdfY.Coefficient;

            var s = Math.Sqrt(v1 + v2);
            var m = m1 + m2;



            return new ContinuousDistribution(new BhattacharjeeDistribution(a, b, m, s), pdfX.Samples);
        }

        public static ContinuousDistribution ConvolutionOfStudentAndUniform(ContinuousDistribution pdfX, ContinuousDistribution pdfY)
        {
            var student = (StudentGeneralizedDistribution)pdfX.BaseDistribution;
            var unif = (UniformContinuousDistribution)pdfY.BaseDistribution;

            double a = unif.Support.Min * pdfY.Coefficient + pdfY.Offset;
            double b = unif.Support.Max * pdfY.Coefficient + pdfY.Offset;
            var s = Math.Abs(student.ScaleCoeffitient * pdfX.Coefficient);
            var m = student.Mean * pdfX.Coefficient + pdfX.Offset;

            return new ContinuousDistribution(new StudentUniformDistribution(a, b, m, s, student.DegreesOfFreedom), pdfX.Samples);
        }


        public static ContinuousDistribution Add(ContinuousDistribution pdf, double value)
        {
            return new ContinuousDistribution(pdf.BaseDistribution, pdf.Samples, pdf.Coefficient, pdf.Offset + value);
        }

        public static ContinuousDistribution Sub(ContinuousDistribution pdf, double value)
        {
            return new ContinuousDistribution(pdf.BaseDistribution, pdf.Samples, pdf.Coefficient, pdf.Offset - value);
        }

        public static ContinuousDistribution Sub(double value, ContinuousDistribution pdf)
        {
            return new ContinuousDistribution(pdf.BaseDistribution, pdf.Samples, -pdf.Coefficient, pdf.Offset + value);
        }

        public static ContinuousDistribution Multiply(ContinuousDistribution pdf, double value)
        {
            return new ContinuousDistribution(pdf.BaseDistribution, pdf.Samples, pdf.Coefficient * value, pdf.Offset * value);
        }

        public static ContinuousDistribution Divide(ContinuousDistribution pdf, double value)
        {
            return new ContinuousDistribution(pdf.BaseDistribution, pdf.Samples, pdf.Coefficient / value, pdf.Offset / value);
        }

        public static ContinuousDistribution Negate(ContinuousDistribution value)
        {
            return Multiply(value, -1);
        }
    }
}
