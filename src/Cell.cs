using DG.Sudoku.Units;
using System;

namespace DG.Sudoku
{
    public class Cell
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _box;
        private readonly CellDigit _digit;


        internal int X => _x;
        internal int Y => _y;
        internal int Box => _box;
        public CellDigit Digit => _digit;


        private Cell(int x, int y, CellDigit digit)
        {
            _x = x;
            _y = y;
            _box = (x / 3) + ((y / 3) * 3);
            _digit = digit;
        }

        public Cell Copy()
        {
            return new Cell(_x, _y, _digit);
        }

        public static Cell ForKnown(int x, int y, int digit)
        {
            return new Cell(x, y, CellDigit.ForKnown(digit));
        }

        public static Cell ForUnkown(int x, int y)
        {
            return new Cell(x, y, CellDigit.ForUnknown());
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

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{_x + 1},{_y + 1}: {(_digit.IsKnown ? _digit.KnownValue.ToString() : "?")}";
        }
    }
}
