using DG.Sudoku.Exceptions;
using DG.Sudoku.Units;
using System;
using System.Collections.Generic;

namespace DG.Sudoku
{
    public class Cell
    {

        public const int MaxValue = 9;
        public enum Kind { Normal = 0, Given = GivenMask, Guess = GuessMask };
        // Digit Unknown state could be any value
        public const int UnkownMask = (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9);

        // Additional bits track state (lower-case to avoid conflicts)
        private const int KnownMask = (1 << 0);
        private const int GivenMask = (1 << 10);
        private const int GuessMask = (1 << 11);

        private static readonly ConcurrentDictionary<int, int[]> possibleOptions = new ConcurrentDictionary<int, int[]>();

        private int _x;
        private int _y;

        /// <summary>
        /// A masked version of <see cref="_bits"/> to only contain bits that indicate the value.
        /// </summary>
        internal int ValueBits => _bits & ~KnownMask & ~GivenMask & ~GuessMask;

        internal int X => _x;
        internal int Y => _y;
        internal int Box => (_x / 3) + ((_y / 3) * 3);

        internal int[] Options => possibleOptions.GetOrAdd(ValueBits, (bits) =>
        {
            List<int> options = new List<int>();
            for (int i = 1; i <= MaxValue; i++)
            {
                if (IsValueBitSet(bits, i))
                {
                    options.Add(i);
                }
            }
            return options.ToArray();
        });

        private int _bits;

        private Cell(int x, int y, int bits)
        {
            _x = x;
            _y = y;
            _bits = bits;
        }

        public Cell Copy()
        {
            return new Cell(_x, _y, _bits);
        }

        public static Cell ForKnown(int x, int y, int value)
        {
            return new Cell(x, y, (1 << value) | KnownMask | GivenMask);
        }

        public static Cell ForUnkown(int x, int y)
        {
            return new Cell(x, y, UnkownMask);
        }

        public void SetValue(int value, Kind kind)
        {
            if (HasKnownValue && (value != KnownValue))
            {
                throw new Exception("Cannot change the value of a known digit (but can change its kind).");
            }
            // Set bits to reflect known value of specified kind
            _bits = (1 << value) | KnownMask | (int)kind;
        }

        public Kind DigitKind
        {
            get
            {
                // The kind of digit
                Kind kind;
                if (0 != (GivenMask & _bits))
                {
                    kind = Kind.Given;
                }
                else if (0 != (GuessMask & _bits))
                {
                    kind = Kind.Guess;
                }
                else
                {
                    kind = Kind.Normal;
                }
                return kind;
            }
        }

        public bool HasKnownValue => 0 != (KnownMask & _bits);

        public int KnownValue => HasKnownValue ? Log2n(ValueBits) : 0;

        public void Exclude(int value)
        {
            // Mask-out the excluded value
            _bits &= ~(1 << value);
            // Invalid digit if no more values are possible
            if (ValueBits == 0)
            {
                throw new ExhaustedCellException(this, $"No more values are possible after removing {value}.");
            }
        }

        public bool CouldBe(int value)
        {
            return IsValueBitSet(_bits, value);
        }

        public bool HasSingleOption(out int optionFound)
        {
            optionFound = 0;
            var bitsToCheck = ValueBits;
            bool singleOption = bitsToCheck > 0 && ((bitsToCheck & (bitsToCheck - 1)) == 0);
            if (!singleOption)
            {
                return false;
            }
            optionFound = Log2n(bitsToCheck);
            return true;
        }

        internal int GetIndex(UnitType unit)
        {
            switch (unit)
            {
                case UnitType.Box:
                    return Box;
                case UnitType.Row:
                    return Y;
                case UnitType.Column:
                    return X;
                default:
                    throw new NotImplementedException($"Method {nameof(GetIndex)} is not implemented for unit type {unit}.");
            }
        }

        private static bool IsValueBitSet(int bits, int value)
        {
            return (bits & (1 << value)) != 0;
        }

        private static int Log2n(int n)
        {
            return (n > 1) ? 1 + Log2n(n / 2) : 0;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{_x + 1},{_y + 1}: {(HasKnownValue ? KnownValue.ToString() : "?")}";
        }
    }
}
