﻿namespace DistributionsBlazor
{
    public class Double1DArrayBinding
    {
        private readonly double[] source;
        private readonly int index;

        public Double1DArrayBinding(double[] source, int index)
        {
            this.source = source;
            this.index = index;
        }

        public double Value
        {
            get => source[index];
            set => source[index] = value;
        }

        public static IList<Double1DArrayBinding[]> GetArrayBindings(double[] source)
        {
            Double1DArrayBinding[] item = new Double1DArrayBinding[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                item[i] = new Double1DArrayBinding(source, i);
            }

            return new List<Double1DArrayBinding[]> { item };
        }
    }

    public class Double2DArrayBinding
    {
        private readonly double[,] source;
        private readonly int x;
        private readonly int y;

        public Double2DArrayBinding(double[,] source, int x, int y)
        {
            this.source = source;
            this.x = x;
            this.y = y;
        }

        public double Value
        {
            get => source[x, y];
            set => source[x, y] = value;
        }

        public static IList<Double2DArrayBinding[]> GetArrayBindings(double[,] source)
        {
            List<Double2DArrayBinding[]> result = new List<Double2DArrayBinding[]>();

            for (int i = 0; i < source.GetLength(0); i++)
            {
                Double2DArrayBinding[] item = new Double2DArrayBinding[source.GetLength(1)];

                for (int j = 0; j < source.GetLength(1); j++)
                {
                    item[j] = new Double2DArrayBinding(source, i, j);
                }

                result.Add(item);
            }

            return result;
        }
    }
}