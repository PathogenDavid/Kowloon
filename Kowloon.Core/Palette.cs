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

                // We always return black for the 0th color
                // This is required because ApartmentManager assumes that the 0th color is black.
                // (It isn't actually possible for ApartmentManager to tell the difference between a black apartment and
                //  an apartment using the 0th palette color due to how apartment colors are stored.)
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
