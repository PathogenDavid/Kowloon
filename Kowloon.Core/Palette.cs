using System;
using System.Collections;
using System.Collections.Generic;

namespace Kowloon.Core
{
    public class Palette : IEnumerable<int>
    {
        public string Name { get; }
        private readonly List<int> Colors = new List<int>();
        public int Count => Colors.Count + 1; // +1 because there's always a dummy black color.

        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                { throw new IndexOutOfRangeException(); }

                if (index == 0)
                { return 0; }

                return Colors[index - 1];
            }
        }

        public Palette(string name)
            => Name = name;

        public void Add(int color)
            => Colors.Add(color & 0xFFFFFF);

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            { yield return this[i]; }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
