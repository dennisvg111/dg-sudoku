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
        /// Executes the <see cref="ISolvingStrategy.FindCandidatesToRemove(Board)"/> method and returns a value indicating if any candidates have been found that haven't already been removed.
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="board"></param>
        /// <param name="candidatesToRemove"></param>
        /// <returns></returns>
        public static bool TryFindCandidatesToRemove(this ISolvingStrategy strategy, Board board, out IEnumerable<Candidate> candidatesToRemove)
        {
            var copy = board.Copy();
            var candidates = strategy.FindCandidatesToRemove(copy);
            candidates = candidates.Where(c => board[c.Position].Digit.CouldBe(c.Digit)).ToArray();
            if (!candidates.Any())
            {
                candidatesToRemove = Enumerable.Empty<Candidate>();
                return false;
            }
            candidatesToRemove = candidates;
            return true;
        }
    }
}
