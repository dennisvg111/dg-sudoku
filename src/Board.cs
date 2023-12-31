﻿using DG.Common.Exceptions;
using DG.Sudoku.CellData;
using DG.Sudoku.SolvingStrategies.Data;
using DG.Sudoku.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace DG.Sudoku
{
    /// <summary>
    /// This class represents the board of a sudoku puzzle.
    /// </summary>
    public class Board : ISolvingBoard
    {
        /// <summary>
        /// <para>The length of the sides of a sudoku.</para>
        /// <para>This also defines the amount of rows, columns, and boxes in this sudoku.</para>
        /// </summary>
        public const int SideLength = CellDigit.MaxValue;

        /// <summary>
        /// The length of the sides of a box inside a sudoku.
        /// </summary>
        public static readonly int BoxSize = (int)Math.Sqrt(SideLength);

        private const int BoardSize = SideLength * SideLength;

        /// <summary>
        /// The cells of this board.
        /// </summary>
        protected readonly Cell[] _cells;

        /// <summary>
        /// Initializes a new instance of <see cref="Board"/> where all cells are unknown.
        /// </summary>
        public Board() : this(Enumerable.Range(0, BoardSize).Select(i => Cell.ForUnkown(i % SideLength, (int)Math.Floor(i / (double)SideLength))).ToArray()) { }

        /// <summary>
        /// Initializes a new instance of <see cref="Board"/> with the given cells.
        /// </summary>
        /// <param name="cells"></param>
        protected Board(Cell[] cells)
        {
            ThrowIf.Collection(cells, nameof(cells)).IsEmpty();
            ThrowIf.Collection(cells, nameof(cells)).CountOtherThan(BoardSize);
            _cells = cells;
        }

        /// <inheritdoc/>
        public Cell this[int x, int y] => _cells[ConvertToArrayIndex(x, y)];

        /// <inheritdoc/>
        public IEnumerable<Cell> GetAllCells()
        {
            return _cells;
        }

        /// <inheritdoc/>
        public IEnumerable<Cell> GetCellsInColumn(Column column, params Cell[] exclude)
        {
            int x = (int)column;
            for (int y = 0; y < SideLength; y++)
            {
                if (Array.Exists(exclude, c => c.Position.Y == y && c.Position.X == x))
                {
                    continue;
                }
                yield return this[x, y];
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Cell> GetCellsInRow(Row row, params Cell[] exclude)
        {
            int y = (int)row;
            for (int x = 0; x < SideLength; x++)
            {
                if (Array.Exists(exclude, c => c.Position.Y == y && c.Position.X == x))
                {
                    continue;
                }
                yield return this[x, y];
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Cell> GetCellsInBox(Box box, params Cell[] exclude)
        {
            int offsetX = ((int)box % BoxSize) * BoxSize;
            int offsetY = ((int)box / BoxSize) * BoxSize;
            for (int y = offsetY; y < offsetY + BoxSize; y++)
            {
                for (int x = offsetX; x < offsetX + BoxSize; x++)
                {
                    if (Array.Exists(exclude, c => c.Position.Y == y && c.Position.X == x))
                    {
                        continue;
                    }
                    yield return this[x, y];
                }
            }
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// Replaces the cell at <see cref="Candidate.Position"/> with a cell without the given digit candidate.
        /// </summary>
        /// <param name="candidate"></param>
        public void RemoveCandidate(Candidate candidate)
        {
            var index = ConvertToArrayIndex(candidate.Position.X, candidate.Position.Y);
            var oldCell = _cells[index];
            var newDigit = oldCell.Digit.WithoutCandidate(candidate.Digit);
            _cells[index] = Cell.With(oldCell.Position, newDigit);
        }

        /// <summary>
        /// Checks to see if the cell at this position is not marked as known, but only has a single valid candidate. If this is true, replaces the cell with a guessed cell and returns <see langword="true"/>. otherwise returns <see langword="false"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool TrySolveCell(int x, int y)
        {
            var index = ConvertToArrayIndex(x, y);
            var oldCell = _cells[index];
            if (oldCell.Digit.IsKnown)
            {
                return false;
            }
            if (!oldCell.Digit.HasSingleCandidate(out int candidate))
            {
                return false;
            }
            _cells[index] = Cell.With(Position.For(x, y), CellDigit.ForGuessed(candidate));
            return true;
        }

        /// <summary>
        /// Converts the given x and y index to the corresponding index in <see cref="_cells"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int ConvertToArrayIndex(int x, int y)
        {
            return y * SideLength + x;
        }

        #region serialization
        /// <summary>
        /// Tries to parse the given string <paramref name="s"/> to a board, where each character in the string represents a cell and characters <c>'1'</c> through <c>'9'</c> get parsed as given digits.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="board"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Parses the given string <paramref name="s"/> to a board, where each character in the string represents a cell and characters <c>'1'</c> through <c>'9'</c> get parsed as given digits.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static Board Parse(string s)
        {
            if (TryParse(s, out Board board))
            {
                return board;
            }
            throw new FormatException($"String {s} is not a valid {nameof(Board)} format.");
        }

        /// <summary>
        /// <para>Renders this board as a string where each cell will be rendered as a single character (left to right, top to bottom).</para>
        /// <para>Unknown digits will be rendered as <c>'-'</c>.</para>
        /// <para>Note that this string can be parsed back using <see cref="TryParse(string, out Board)"/> or <see cref="Parse(string)"/>.</para>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Concat(_cells.Select(c => c.Digit.IsKnown ? c.Digit.KnownValue.ToString() : "-"));
        }
        #endregion

        #region samples

        //These properties are for testing purposes.
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Board Easy => Parse("---1-5---14----67--8---24---63-7--1-9-------3-1--9-52---72---8--26----35---4-9---");
        public static Board Gentle => Parse("-----4-284-6-----51---3-6-----3-1----87---14----7-9-----2-1---39-----5-767-4-----");
        public static Board Moderate => Parse("72--96--3---2-5----8---4-2--------6-1-65-38-7-4--------3-8---9----7-2---2--43--18");
        public static Board Tough => Parse("3-9---4--2--7-9----87------75--6-23-6--9-4--8-28-5--41------59----1-6--7--6---1-4");
        public static Board Diabolical => Parse("---7-4--5-2--1--7-----8---2-9---625-6---7---8-532---1-4---9-----3--6--9-2--4-7---");
        public static Board HiddenSingle => Parse("2---7--38-----6-7-3---4-6----8-2-7--1-------6--7-3-4----4-8---9-6-4-----91--6---2");
        public static Board NakedPair => Parse("4-----938-32-941---953--24-37-6-9--4529--16736-47-3-9-957--83----39--4--24--3-7-9");
        public static Board NakedTriple => Parse("294513--66--8423193--697254----56----4--8--6----47----73-164--59--735--14--928637");
        public static Board NakedQuad => Parse("----3--86----2--4--9--7852-3718562949--1423754--3976182--7-3859-392-54677--9-4132");
        public static Board HiddenQuad => Parse("9-15---46425-9--8186--1--2-5-2-------19---46-6-------2196-4-2532---6-817-----1694");
        public static Board PointingPair => Parse("93--5----2--63--95856--2-----318-57---5-2-98--8---5------8--1595-821---4---56---8");
        public static Board XWing => Parse("--54--6-2--6-2-15-293561784-523-48--3-12-64-5----5732--3--4256--24--59--5-7--924-");
        public static Board SinglesChain => Parse("--7-836---397-68--82641975364-19-387-8-367----73-48-6-39-87--267649--1382-863-97-");
        public static Board YWing => Parse("645-1-893738459621219638745597-6-184481975---3268415799-2-8--1-8-319----164-2-9-8");

#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

        #endregion
    }
}
