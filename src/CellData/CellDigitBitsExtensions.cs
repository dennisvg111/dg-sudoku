using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DG.Sudoku.CellData
{
    /// <summary>
    /// Provides extension methods for the mask <see cref="short"/> in an instance of <see cref="CellDigit"/>.
    /// </summary>
    internal static class CellDigitBitsExtensions
    {
        private static readonly ConcurrentDictionary<short, int[]> possibleOptions = new ConcurrentDictionary<short, int[]>();

        /// <summary>
        /// Returns a value indicating if this mask <see cref="short"/> can represent the given digit.
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static bool CanBeDigit(this short bits, int digit)
        {
            return (bits & 1 << digit) != 0;
        }

        /// <summary>
        /// Returns a list of the digits 1 through 9 that can be represented by the given mask <see cref="short"/>.
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
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
