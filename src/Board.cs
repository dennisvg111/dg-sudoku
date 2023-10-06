using DG.Sudoku.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DG.Sudoku
{
    public class Board
    {
        public const int SideLength = CellDigit.MaxValue;
        public static readonly int RegionSize = (int)Math.Sqrt(SideLength);
        private const int BoardSize = SideLength * SideLength;
        private Cell[] _cells;

        public Board() : this(Enumerable.Range(0, BoardSize).Select(i => Cell.ForUnkown(i % SideLength, (int)Math.Floor(i / (double)SideLength))).ToArray()) { }

        private Board(Cell[] cells)
        {
            _cells = cells;
        }

        internal Board Copy()
        {
            return new Board(_cells.Select(c => c.Copy()).ToArray());
        }

        public Cell this[int x, int y] => _cells[y * SideLength + x];

        public Cell this[CellPosition i] => this[i.X, i.Y];

        public IEnumerable<Cell> GetCellsInColumn(Column x, params Cell[] exclude)
        {
            for (int y = 0; y < SideLength; y++)
            {
                if (exclude.Any(c => c.X == (int)x && c.Y == y))
                {
                    continue;
                }
                yield return this[(int)x, y];
            }
        }

        public IEnumerable<Cell> GetCellsInRow(Row y, params Cell[] exclude)
        {
            for (int x = 0; x < SideLength; x++)
            {
                if (exclude.Any(c => c.Y == (int)y && c.X == x))
                {
                    continue;
                }
                yield return this[x, (int)y];
            }
        }

        public IEnumerable<Cell> GetCellsInBox(Box region, params Cell[] exclude)
        {
            int offsetX = ((int)region % RegionSize) * RegionSize;
            int offsetY = ((int)region / RegionSize) * RegionSize;
            for (int y = offsetY; y < offsetY + RegionSize; y++)
            {
                for (int x = offsetX; x < offsetX + RegionSize; x++)
                {
                    if (exclude.Any(c => c.Y == y && c.X == x))
                    {
                        continue;
                    }
                    yield return this[x, y];
                }
            }
        }

        public IEnumerable<Cell> GetCellsInUnit(UnitType unit, int index, params Cell[] exclude)
        {
            switch (unit)
            {
                case UnitType.Box:
                    return GetCellsInBox((Box)index, exclude);
                case UnitType.Row:
                    return GetCellsInRow((Row)index, exclude);
                case UnitType.Column:
                    return GetCellsInColumn((Column)index, exclude);
                default:
                    throw new NotImplementedException("Cannot get cells in unit type " + unit + ".");
            }
        }

        public IEnumerable<Cell> GetInfluencedCells(Cell cell)
        {
            foreach (var otherCell in _cells)
            {
                if (otherCell.X == cell.X && otherCell.Y == cell.Y)
                {
                    continue;
                }
                if (otherCell.X == cell.X || otherCell.Y == cell.Y || otherCell.Box == cell.Box)
                {
                    yield return otherCell;
                }
            }
        }

        #region serialization
        public static bool TryParse(string s, out Board board)
        {
            if (string.IsNullOrEmpty(s) || s.Length != BoardSize)
            {
                board = null;
                return false;
            }
            Cell[] cells = new Cell[BoardSize];
            for (int y = 0; y < SideLength; y++)
            {
                for (int x = 0; x < SideLength; x++)
                {
                    int index = y * SideLength + x;
                    int value = s[index] - '0';
                    cells[index] = (value <= CellDigit.MaxValue && value > 0) ? Cell.ForKnown(x, y, value) : Cell.ForUnkown(x, y);
                }
            }
            board = new Board(cells);
            return true;
        }

        internal static Board Parse(string s)
        {
            Board board;
            if (TryParse(s, out board))
            {
                return board;
            }
            throw new FormatException($"String {s} is not a valid {nameof(Board)} format.");
        }

        public override string ToString()
        {
            return string.Concat(_cells.Select(c => c.Digit.IsKnown ? c.Digit.KnownValue.ToString() : "."));
        }
        #endregion

        #region samples
        public static Board Easy => Parse("...1.5...14....67..8...24...63.7..1.9.......3.1..9.52...72...8..26....35...4.9...");
        public static Board Gentle => Parse(".....4.284.6.....51...3.6.....3.1....87...14....7.9.....2.1...39.....5.767.4.....");
        public static Board Moderate => Parse("72..96..3...2.5....8...4.2........6.1.65.38.7.4........3.8...9....7.2...2..43..18");
        public static Board Tough => Parse("3.9...4..2..7.9....87......75..6.23.6..9.4..8.28.5..41......59....1.6..7..6...1.4");
        public static Board Diabolical => Parse("...7.4..5.2..1..7.....8...2.9...625.6...7...8.532...1.4...9.....3..6..9.2..4.7...");
        public static Board HiddenSingle => Parse("2...7..38.....6.7.3...4.6....8.2.7..1.......6..7.3.4....4.8...9.6.4.....91..6...2");
        public static Board NakedPair => Parse("4.....938.32.941...953..24.37.6.9..4529..16736.47.3.9.957..83....39..4..24..3.7.9");
        public static Board NakedTriple => Parse("294513..66..8423193..697254....56....4..8..6....47....73.164..59..735..14..928637");
        public static Board NakedQuad => Parse("....3..86....2..4..9..7852.3718562949..1423754..3976182..7.3859.392.54677..9.4132");
        public static Board HiddenQuad => Parse("9.15...46425.9..8186..1..2.5.2.......19...46.6.......2196.4.2532...6.817.....1694");
        public static Board PointingPair => Parse("93..5....2..63..95856..2.....318.57...5.2.98..8...5......8..1595.821...4...56...8");
        public static Board XWing => Parse("..54..6.2..6.2.15.293561784.523.48..3.12.64.5....5732..3..4256..24..59..5.7..924.");
        public static Board SinglesChain => Parse("..7.836...397.68..82641975364.19.387.8.367....73.48.6.39.87..267649..1382.863.97.");
        public static Board YWing => Parse("645.1.893738459621219638745597.6.184481975...3268415799.2.8..1.8.319....164.2.9.8");
        #endregion
    }
}
