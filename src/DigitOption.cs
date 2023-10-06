using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DG.Sudoku
{
    public static class DigitOption
    {
        private static readonly ConcurrentDictionary<short, int[]> possibleOptions = new ConcurrentDictionary<short, int[]>();

        public static bool CanBeDigit(this short bits, int digit)
        {
            return (bits & (1 << digit)) != 0;
        }

        public static int[] GetOptions(this short bits)
        {
            return possibleOptions.GetOrAdd(bits, (b) =>
            {
                List<int> options = new List<int>();
                for (int i = 1; i <= CellDigit.MaxValue; i++)
                {
                    if (b.CanBeDigit(i))
                    {
                        options.Add(i);
                    }
                }
                return options.ToArray();
            });
        }
    }
}
