using DG.Sudoku.CellData;

namespace DG.Sudoku
{
    /// <summary>
    /// Represents a cell in a sudoku board.
    /// </summary>
    public readonly struct Cell
    {
        private readonly Position _position;
        private readonly CellDigit _digit;

        /// <summary>
        /// The position of this cell.
        /// </summary>
        public Position Position => _position;

        /// <summary>
        /// The digit or possible candidates of this cell.
        /// </summary>
        public CellDigit Digit => _digit;


        private Cell(Position position, CellDigit digit)
        {
            _position = position;
            _digit = digit;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Cell"/>, with the given position and digit, and with <see cref="DigitKnowledge.Given"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static Cell ForKnown(int x, int y, int digit)
        {
            return new Cell(Position.For(x, y), CellDigit.ForKnown(digit));
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Cell"/> with all candidates possible and with <see cref="DigitKnowledge.Unknown"/>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static Cell ForUnkown(int x, int y)
        {
            return new Cell(Position.For(x, y), CellDigit.ForUnknown());
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Cell"/>, with the given position and digit.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static Cell With(Position position, CellDigit digit)
        {
            return new Cell(position, digit);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Position.X + 1},{Position.Y + 1}: {(_digit.IsKnown ? _digit.KnownValue.ToString() : "?")}";
        }
    }
}
