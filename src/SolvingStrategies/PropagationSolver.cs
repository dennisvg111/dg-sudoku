using System.Collections.Generic;

namespace DG.Sudoku.SolvingStrategies
{
    /// <summary>
    /// A solving strategy that works by simply removing candidates that already appear in a row, column, or box.
    /// </summary>
    public class PropagationSolver : ISolvingStrategy
    {
        /// <inheritdoc/>
        public string Name => "Propagation";

        /// <inheritdoc/>
        public Difficulty Difficulty => Difficulty.Easy;

        /// <inheritdoc/>
        public IEnumerable<Candidate> FindCandidatesToRemove(Board board)
        {
            for (int x = 0; x < Board.SideLength; x++)
            {
                for (int y = 0; y < Board.SideLength; y++)
                {
                    if (board[x, y].Digit.IsKnown)
                    {
                        var cell = board[x, y];
                        var cells = board.GetInfluencedCells(cell);
                        foreach (var otherCell in cells)
                        {
                            yield return Candidate.For(otherCell, cell.Digit.KnownValue);
                        }
                    }
                }
            }
        }
    }
}
