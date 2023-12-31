﻿using System.Collections.Generic;
using DG.Sudoku.SolvingStrategies.Data;

namespace DG.Sudoku.Propagation
{
    /// <summary>
    /// Defines an algorithm to find all cells that share a row, column, or box with a given cell.
    /// </summary>
    public interface IInfluencedCellsAlgorithm
    {
        /// <summary>
        /// Returns all cells that share a row, column, or box with the given cell.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        IEnumerable<Cell> GetInfluencedCells(ISolvingBoard board, Cell cell);
    }
}
