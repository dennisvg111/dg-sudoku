namespace DG.Sudoku
{
    /// <summary>
    /// This class represents a digit from 1 through 9 that could be located in a cell in a given <see cref="Position"/>.
    /// </summary>
    public class PossibleDigitInCell
    {
        private readonly Position _position;
        private readonly int _digit;

        /// <summary>
        /// The position of the cell this digit could be located in.
        /// </summary>
        public Position Position => _position;

        /// <summary>
        /// A digit from 1 through 9.
        /// </summary>
        public int Digit => _digit;

        /// <summary>
        /// Creates a new instance of <see cref="PossibleDigitInCell"/> with the given position and digit.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="digit"></param>
        public PossibleDigitInCell(Position position, int digit)
        {
            _position = position;
            _digit = digit;
        }

        /// <summary>
        /// Creates a new instance of <see cref="PossibleDigitInCell"/> with the given position and digit.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static PossibleDigitInCell For(Position position, int digit)
        {
            return new PossibleDigitInCell(position, digit);
        }

        /// <summary>
        /// Creates a new instance of <see cref="PossibleDigitInCell"/> with the given x and y index as position, and the given digit.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static PossibleDigitInCell For(int x, int y, int digit)
        {
            return new PossibleDigitInCell(Position.For(x, y), digit);
        }
    }
}
