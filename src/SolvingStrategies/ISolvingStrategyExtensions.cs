using System.Collections.Generic;
using System.Linq;

namespace DG.Sudoku.SolvingStrategies
{
    /// <summary>
    /// Provides extension methods for classes implementing <see cref="ISolvingStrategy"/>.
    /// </summary>
    public static class ISolvingStrategyExtensions
    {
        /// <summary>
        /// Executes the <see cref="ISolvingStrategy.FindValuesToRemove(Board)"/> method and returns a value indicating if any digits have been found that haven't already been removed.
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="board"></param>
        /// <param name="valuesToRemove"></param>
        /// <returns></returns>
        public static bool TryFindValuesToRemove(this ISolvingStrategy strategy, Board board, out IEnumerable<PossibleDigitInCell> valuesToRemove)
        {
            var copy = board.Copy();
            var removables = strategy.FindValuesToRemove(copy);
            removables = removables.Where(vc => board[vc.Position].Digit.CouldBe(vc.Digit)).ToArray();
            if (!removables.Any())
            {
                valuesToRemove = Enumerable.Empty<PossibleDigitInCell>();
                return false;
            }
            valuesToRemove = removables;
            return true;
        }
    }
}
