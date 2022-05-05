namespace DistributionsBlazor
{
    public class OneDimensionalArrayBinding<T>
    {
        private readonly T[] source;
        private readonly int index;

        public OneDimensionalArrayBinding(T[] source, int index)
        {
            this.source = source;
            this.index = index;
        }

        public T Value
        {
            get => source[index];
            set => source[index] = value;
        }

        public static IList<OneDimensionalArrayBinding<T>[]> GetArrayBindings(T[] source)
        {
            OneDimensionalArrayBinding<T>[] item = new OneDimensionalArrayBinding<T>[source.Length];

            for (int i = 0; i < source.Length; i++)
            {
                item[i] = new OneDimensionalArrayBinding<T>(source, i);
            }

            return new List<OneDimensionalArrayBinding<T>[]> { item };
        }
    }

    public class TwoDimesionalArrayBinding<T>
    {
        private readonly T[,] source;
        private readonly int x;
        private readonly int y;

        public TwoDimesionalArrayBinding(T[,] source, int x, int y)
        {
            this.source = source;
            this.x = x;
            this.y = y;
        }

        public T Value
        {
            get => source[x, y];
            set => source[x, y] = value;
        }

        public static IList<TwoDimesionalArrayBinding<T>[]> GetArrayBindings(T[,] source)
        {
            List<TwoDimesionalArrayBinding<T>[]> result = new List<TwoDimesionalArrayBinding<T>[]>();

            for (int i = 0; i < source.GetLength(0); i++)
            {
                TwoDimesionalArrayBinding<T>[] item = new TwoDimesionalArrayBinding<T>[source.GetLength(1)];

                for (int j = 0; j < source.GetLength(1); j++)
                {
                    item[j] = new TwoDimesionalArrayBinding<T>(source, i, j);
                }

                result.Add(item);
            }

            return result;
        }
    }
}
