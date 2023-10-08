using System.Collections.Generic;

namespace DG.Sudoku.SolvingStrategies
{
    /// <summary>
    /// Defines a strategy used to remove possible digits from cells in a sudoku board.
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
        /// Finds the values to remove from the board. 
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        IEnumerable<PossibleDigitInCell> FindValuesToRemove(Board board);
    }
}
