using DG.Sudoku.CellData;

namespace DG.Sudoku
{
    public class Cell
    {
        private readonly Position _position;
        private readonly CellDigit _digit;

        public Position Position => _position;
        public CellDigit Digit => _digit;


        private Cell(Position position, CellDigit digit)
        {
            _position = position;
            _digit = digit;
        }

        public Cell Copy()
        {
            //position is read-only so we don't need to copy it.
            return new Cell(_position, _digit.Copy());
        }

        public static Cell ForKnown(int x, int y, int digit)
        {
            return new Cell(Position.For(x, y), CellDigit.ForKnown(digit));
        }

        public static Cell ForUnkown(int x, int y)
        {
            return new Cell(Position.For(x, y), CellDigit.ForUnknown());
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Position.X + 1},{Position.Y + 1}: {(_digit.IsKnown ? _digit.KnownValue.ToString() : "?")}";
        }
    }
}
