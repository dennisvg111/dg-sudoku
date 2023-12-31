﻿using System.Collections.Generic;
using DG.Sudoku.SolvingStrategies.Data;

namespace DG.Sudoku.SolvingStrategies
{
    /// <summary>
    /// Defines a strategy used to remove candidates from cells in a sudoku board.
    /// </summary>
    public interface ISolvingStrategy
    {
        /// <summary>
        /// The name of this strategy.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// How difficult this strategy would be to use.
        /// </summary>
        Difficulty Difficulty { get; }

        /// <summary>
        /// Finds the candidates to remove from the board. 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        IEnumerable<Candidate> FindCandidatesToRemove(ISolvingBoard board);
    }
}
