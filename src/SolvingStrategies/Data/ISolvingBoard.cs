using DG.Sudoku.Units;
using System.Collections.Generic;

namespace DG.Sudoku.SolvingStrategies.Data
{
    /// <summary>
    /// Defines methods of retrieving cells to be used by an implementation of <see cref="ISolvingStrategy"/>
    /// </summary>
    public interface ISolvingBoard
    {
        /// <summary>
        /// Provides a way to iterate over all cells in the board.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Cell> GetAllCells();

        /// <summary>
        /// Returns the cell at the given zero-indexed x and y coordinate.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        Cell this[int x, int y] { get; }

        /// <summary>
        /// Returns all cells in the given column.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        IEnumerable<Cell> GetCellsInColumn(Column column, params Cell[] exclude);

        /// <summary>
        /// Returns all cells in the given row.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        IEnumerable<Cell> GetCellsInRow(Row row, params Cell[] exclude);

        /// <summary>
        /// Returns all cells in the given box.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        IEnumerable<Cell> GetCellsInBox(Box box, params Cell[] exclude);

        /// <summary>
        /// Returns all cells in the unit type specified by <paramref name="unit"/>, with the given zero-based index.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="index"></param>
        /// <param name="exclude"></param>
        /// <returns></returns>
        IEnumerable<Cell> GetCellsInUnit(UnitType unit, int index, params Cell[] exclude);
    }
}