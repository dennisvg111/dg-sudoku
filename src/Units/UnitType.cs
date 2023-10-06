using System.Collections.Generic;

namespace DG.Sudoku.Units
{
    public enum UnitType
    {
        Box = 1,
        Row = 2,
        Column = 4
    }

    public static class UnitTypes
    {
        private static readonly UnitType[] _all = new UnitType[] { UnitType.Row, UnitType.Column, UnitType.Box };


        public static IReadOnlyList<UnitType> All => _all;
    }
}
