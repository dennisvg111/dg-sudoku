using System.Collections.Generic;

namespace DG.Sudoku.Units
{
    /// <summary>
    /// Values of this enumeration represent a type of unit inside a sudoku.
    /// </summary>
    public enum UnitType
    {
        /// <summary>
        /// A 3x3 box inside a sudoku.
        /// </summary>
        Box = 1,

        /// <summary>
        /// A horizontal row inside a sudoku.
        /// </summary>
        Row = 2,

        /// <summary>
        /// A vertical column inside a sudoku.
        /// </summary>
        Column = 4
    }

    /// <summary>
    /// Provides a way to get a list of all values of <see cref="UnitType"/>.
    /// </summary>
    public static class UnitTypes
    {
        private static readonly UnitType[] _all = new UnitType[] { UnitType.Row, UnitType.Column, UnitType.Box };

        /// <summary>
        /// A list of all values of <see cref="UnitType"/>.
        /// </summary>
        public static IReadOnlyList<UnitType> All => _all;
    }
}
