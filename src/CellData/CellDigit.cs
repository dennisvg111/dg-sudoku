using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DG.Sudoku.CellData
{
    /// <summary>
    /// This class represents the possible candidates for a <see cref="Cell"/>.
    /// </summary>
    public readonly struct CellDigit
    {
        /// <summary>
        /// 9
        /// </summary>
        public const int MaxValue = 9;

        // Additional bits track state (lower-case to avoid conflicts)
        private const short _knownMask = 1 << 0;
        private const short _givenMask = 1 << 10;
        private const short _guessMask = 1 << 11;

        // Digit Unknown state could be any value
        private const int _unkownMask = 1 << 1 | 1 << 2 | 1 << 3 | 1 << 4 | 1 << 5 | 1 << 6 | 1 << 7 | 1 << 8 | 1 << 9;

        private readonly short _bits;

        /// <summary>
        /// A masked version of <see cref="_bits"/> to only contain bits that indicate the value.
        /// </summary>
        private readonly short _digitBits;

        /// <summary>
        /// Indicates if this value is known.
        /// </summary>
        public bool IsKnown => 0 != (_knownMask & _bits);

        /// <summary>
        /// Returns the known value of this cell, otherwise 0.
        /// </summary>
        public int KnownValue => IsKnown ? Log2n(_digitBits) : 0;

        /// <summary>
        /// Indicates this cell has no more values it could possibly be.
        /// </summary>
        public bool IsExhausted => _digitBits == 0;

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/>.
        /// </summary>
        /// <param name="bits"></param>
        private CellDigit(short bits)
        {
            _bits = bits;
            _digitBits = (short)(_bits & ~_knownMask & ~_givenMask & ~_guessMask);
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
        /// Excludes the given <paramref name="candidate"/> from the possible digits this cell can be.
        /// </summary>
        /// <param name="candidate"></param>
        public CellDigit WithoutCandidate(int candidate)
        {
            // Mask-out the excluded value
            var bitsWithoutCandidate = _bits & (short)~(1 << candidate);
            return new CellDigit((short)bitsWithoutCandidate);
        }

        /// <summary>
        /// Indicates what kind of digit this is.
        /// </summary>
        public DigitKnowledge Type
        {
            get
            {
                if (0 != (_givenMask & _bits))
                {
                    return DigitKnowledge.Given;
                }
                if (0 != (_guessMask & _bits))
                {
                    return DigitKnowledge.Guessed;
                }
                return DigitKnowledge.Unknown;
            }
        }

        /// <summary>
        /// Indicates if this digit could be equal to the given <paramref name="candidate"/>.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public bool CouldBe(int candidate)
        {
            return CouldBe(_bits, candidate);
        }

        private static readonly ConcurrentDictionary<short, IReadOnlyList<int>> cachedOptions = new ConcurrentDictionary<short, IReadOnlyList<int>>();
        /// <summary>
        /// Returns a list of the digits 1 through 9 that can be represented by the given mask <see cref="short"/>.
        /// </summary>
        /// <returns></returns>
        public IReadOnlyList<int> GetCandidates()
        {
            return cachedOptions.GetOrAdd(_digitBits, (b) =>
            {
                List<int> options = new List<int>();
                for (int i = 1; i <= MaxValue; i++)
                {
                    if (CouldBe(b, i))
                    {
                        options.Add(i);
                    }
                }
                return options;
            });
        }

        /// <summary>
        /// Returns a list of possible values this digit can have, checking only the values that match the given <paramref name="digitPredicate"/>.
        /// </summary>
        /// <param name="digitPredicate"></param>
        /// <returns></returns>
        public IReadOnlyList<int> GetCandidatesWhere(Func<int, bool> digitPredicate)
        {
            List<int> options = new List<int>();
            for (int i = 1; i <= MaxValue; i++)
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
        /// Indicates if this digit has only one possible value, and returns the possible value as <paramref name="candidate"/>.
        /// </summary>
        /// <param name="candidate"></param>
        /// <returns></returns>
        public bool HasSingleCandidate(out int candidate)
        {
            candidate = 0;
            var bitsToCheck = _digitBits;
            bool singleOption = bitsToCheck > 0 && (bitsToCheck & bitsToCheck - 1) == 0;
            if (!singleOption)
            {
                return false;
            }
            candidate = Log2n(bitsToCheck);
            return true;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/> for a specific digit.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CellDigit ForKnown(int value)
        {
            return new CellDigit((short)(1 << value | _knownMask | _givenMask));
        }

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/> for a specific digit.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static CellDigit ForGuessed(int value)
        {
            return new CellDigit((short)(1 << value | _knownMask | _guessMask));
        }

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/> where the underlying digit is not known.
        /// </summary>
        /// <returns></returns>
        public static CellDigit ForUnknown()
        {
            return new CellDigit(_unkownMask);
        }

        /// <summary>
        /// Creates a new instance of <see cref="CellDigit"/> where the underlying digit is not known, but can only be one of the given candidates.
        /// </summary>
        /// <param name="candidates"></param>
        /// <returns></returns>
        public static CellDigit WithCandidates(params int[] candidates)
        {
            short bits = 0;
            if (candidates != null)
            {
                foreach (var candidate in candidates)
                {
                    bits |= (short)(1 << candidate);
                }
            }
            return new CellDigit(bits);
        }

        /// <summary>
        /// Indicates if the given <paramref name="bits"/> has the bit set for a specific <paramref name="candidate"/>.
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="candidate"></param>
        /// <returns></returns>
        private static bool CouldBe(short bits, int candidate)
        {
            return (bits & 1 << candidate) != 0;
        }

        private static int Log2n(int n)
        {
            return n > 1 ? 1 + Log2n(n / 2) : 0;
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
