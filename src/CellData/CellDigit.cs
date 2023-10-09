using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DG.Sudoku.CellData
{
    /// <summary>
    /// This class represents the digit possibilities for a <see cref="Cell"/>.
    /// </summary>
    public sealed class CellDigit : IEquatable<CellDigit>
    {
        /// <summary>
        /// 9
        /// </summary>
        public const int MaxValue = 9;

        // Additional bits track state (lower-case to avoid conflicts)
        private const short KnownMask = 1 << 0;
        private const short GivenMask = 1 << 10;
        private const short GuessMask = 1 << 11;

        // Digit Unknown state could be any value
        private const int _unkownMask = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5 | 1 << 6 | 1 << 7 | 1 << 8 | 1 << 9;

        private short _bits;

        /// <summary>
        /// A masked version of <see cref="_bits"/> to only contain bits that indicate the value.
        /// </summary>
        private short ValueBits => (short)(_bits & ~KnownMask & ~GivenMask & ~GuessMask);

        /// <summary>
        /// Indicates if this value is known.
        /// </summary>
        public bool IsKnown => 0 != (KnownMask & _bits);

        /// <summary>
        /// Returns the known value of this cell, otherwise 0.
        /// </summary>
        public int KnownValue => IsKnown ? Log2n(ValueBits) : 0;

        /// <summary>
        /// Indicates this cell has no more values it could possibly be.
        /// </summary>
        public bool IsExhausted => ValueBits == 0;

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/>.
        /// </summary>
        /// <param name="bits"></param>
        private CellDigit(short bits)
        {
            _bits = bits;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/> with the same digit knowledge.
        /// </summary>
        /// <returns></returns>
        public CellDigit Copy()
        {
            return new CellDigit(_bits);
        }

        /// <summary>
        /// Excludes the given <paramref name="digit"/> from the possible digits this cell can be.
        /// </summary>
        /// <param name="digit"></param>
        public void RemoveOption(int digit)
        {
            // Mask-out the excluded value
            _bits &= (short)~(1 << digit);
        }

        /// <summary>
        /// Indicates what kind of digit this is.
        /// </summary>
        public DigitKnowledge Type
        {
            get
            {
                if (0 != (GivenMask & _bits))
                {
                    return DigitKnowledge.Given;
                }
                if (0 != (GuessMask & _bits))
                {
                    return DigitKnowledge.Guessed;
                }
                return DigitKnowledge.Unknown;
            }
        }

        /// <summary>
        /// Indicates if this digit could be equal to the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool CouldBe(int value)
        {
            return (_bits & 1 << value) != 0;
        }

        private static readonly ConcurrentDictionary<short, IReadOnlyList<int>> cachedOptions = new ConcurrentDictionary<short, IReadOnlyList<int>>();
        /// <summary>
        /// Returns a list of the digits 1 through 9 that can be represented by the given mask <see cref="short"/>.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<int> GetOptions()
        {
            return cachedOptions.GetOrAdd(ValueBits, (b) =>
            {
                return GetOptionsWhere(d => true);
            });
        }

        /// <summary>
        /// Returns a list of possible values this digit can have, checking only the values that match the given <paramref name="digitPredicate"/>.
        /// </summary>
        /// <param name="digitPredicate"></param>
        /// <returns></returns>
        public IReadOnlyList<int> GetOptionsWhere(Func<int, bool> digitPredicate)
        {
            List<int> options = new List<int>();
            for (int i = 1; i <= CellDigit.MaxValue; i++)
            {
                if (!digitPredicate(i))
                {
                    continue;
                }
                if (CouldBe(i))
                {
                    options.Add(i);
                }
            }
            return options;
        }

        /// <summary>
        /// Indicates if this digit has only one possible value, and returns the possible value as <paramref name="optionFound"/>.
        /// </summary>
        /// <param name="optionFound"></param>
        /// <returns></returns>
        public bool HasSingleOption(out int optionFound)
        {
            optionFound = 0;
            var bitsToCheck = ValueBits;
            bool singleOption = bitsToCheck > 0 && (bitsToCheck & bitsToCheck - 1) == 0;
            if (!singleOption)
            {
                return false;
            }
            optionFound = Log2n(bitsToCheck);
            return true;
        }

        /// <summary>
        /// Sets this digit to the given value (if <see cref="IsKnown"/> is currently false), and changes <see cref="Type"/> to <see cref="DigitKnowledge.Guessed"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGuessValue(int value)
        {
            if (IsKnown && value != KnownValue)
            {
                return false;
            }
            // Set bits to reflect known value of specified kind
            _bits = (short)(1 << value | KnownMask | GuessMask);
            return true;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/> for a specific digit.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CellDigit ForKnown(int value)
        {
            return new CellDigit((short)(1 << value | KnownMask | GivenMask));
        }

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/> where the underlying digit is not known.
        /// </summary>
        /// <returns></returns>
        public static CellDigit ForUnknown()
        {
            return new CellDigit(_unkownMask);
        }

        private static int Log2n(int n)
        {
            return n > 1 ? 1 + Log2n(n / 2) : 0;
        }

        /// <inheritdoc/>
        public bool Equals(CellDigit other)
        {
            return _bits == other._bits;
        }

        /// <summary>
        /// Returns a string that represents the <see cref="KnownValue"/>, or a question mark.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return IsKnown ? KnownValue.ToString() : "?";
        }
    }
}
