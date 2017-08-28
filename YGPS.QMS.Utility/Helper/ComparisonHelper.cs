using System;
using System.Collections.Generic;

namespace InstagramPhotos.Utility.Helper
{
    public static class ComparisonHelper<T>
    {
        public static IComparer<T> CreateComparer<V>(Func<T, V> keySelector)
        {
            return new CommonComparer<V>(keySelector);
        }
        public static IComparer<T> CreateComparer<V>(Func<T, V> keySelector, IComparer<V> comparer)
        {
            return new CommonComparer<V>(keySelector, comparer);
        }

        class CommonComparer<V> : IComparer<T>
        {
            private Func<T, V> keySelector;
            private IComparer<V> comparer;

            public CommonComparer(Func<T, V> keySelector, IComparer<V> comparer)
            {
                this.keySelector = keySelector;
                this.comparer = comparer;
            }
            public CommonComparer(Func<T, V> keySelector)
                : this(keySelector, Comparer<V>.Default)
            { }

            public int Compare(T x, T y)
            {
                return comparer.Compare(keySelector(x), keySelector(y));
            }
        }
    }
}