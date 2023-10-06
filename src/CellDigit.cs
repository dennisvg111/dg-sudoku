namespace DG.Sudoku
{
    public class CellDigit
    {
        /// <summary>
        /// 9
        /// </summary>
        public const int MaxValue = 9;

        // Additional bits track state (lower-case to avoid conflicts)
        internal const short KnownMask = (1 << 0);
        internal const short GivenMask = (1 << 10);
        internal const short GuessMask = (1 << 11);

        // Digit Unknown state could be any value
        private const int _unkownMask = (1 << 1) | (1 << 2) | (1 << 3) | (1 << 4) | (1 << 5) | (1 << 6) | (1 << 7) | (1 << 8) | (1 << 9);

        private short _bits;

        /// <summary>
        /// A masked version of <see cref="_bits"/> to only contain bits that indicate the value.
        /// </summary>
        internal short ValueBits => (short)(_bits & ~KnownMask & ~GivenMask & ~GuessMask);

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

        public CellDigit(short bits)
        {
            _bits = bits;
        }

        /// <summary>
        /// Excludes the given <paramref name="digit"/> from the possible digits this cell can be.
        /// </summary>
        /// <param name="digit"></param>
        public void Exclude(int digit)
        {
            // Mask-out the excluded value
            _bits &= (short)~(1 << digit);
        }

        public ValueKind DigitKind
        {
            get
            {
                if (0 != (GivenMask & _bits))
                {
                    return ValueKind.Given;
                }
                if (0 != (GuessMask & _bits))
                {
                    return ValueKind.Guess;
                }
                return ValueKind.Normal;
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

        public bool TrySetValue(int value, ValueKind kind)
        {
            if (IsKnown && (value != KnownValue))
            {
                return false;
            }
            // Set bits to reflect known value of specified kind
            _bits = (short)((1 << value) | KnownMask | (int)kind);
            return true;
        }

        public static CellDigit ForKnown(int value)
        {
            return new CellDigit((short)((1 << value) | KnownMask | GivenMask));
        }

        public static CellDigit ForUnknown()
        {
            return new CellDigit(_unkownMask);
        }

        private static int Log2n(int n)
        {
            return (n > 1) ? 1 + Log2n(n / 2) : 0;
        }
    }
}
