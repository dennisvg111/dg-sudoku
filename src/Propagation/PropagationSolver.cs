using DG.Sudoku.SolvingStrategies;
using System.Collections.Generic;

namespace DG.Sudoku.Propagation
{
    /// <summary>
    /// A solving strategy that works by simply removing candidates that already appear in a row, column, or box.
    /// </summary>
    public class PropagationSolver : ISolvingStrategy
    {
        private static readonly PropagationSolver _default = new PropagationSolver(LoopingInfluencedCellsAlgorithm.Instance);

        /// <summary>
        /// The default shared instance of <see cref="PropagationSolver"/>, using <see cref="LoopingInfluencedCellsAlgorithm"/>.
        /// </summary>
        public static PropagationSolver Default => _default;

        private readonly IInfluencedCellsAlgorithm _algorithm;

        /// <summary>
        /// Initializes a new instance of <see cref="PropagationSolver"/> with the given <see cref="IInfluencedCellsAlgorithm"/>.
        /// </summary>
        /// <param name="algorithm"></param>
        public PropagationSolver(IInfluencedCellsAlgorithm algorithm)
        {
            _algorithm = algorithm;
        }

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
                        var cells = _algorithm.GetInfluencedCells(board, cell);
                        foreach (var otherCell in cells)
                        {
                            if (!otherCell.Digit.CouldBe(cell.Digit.KnownValue))
                            {
                                continue;
                            }
                            yield return Candidate.For(otherCell, cell.Digit.KnownValue);
                        }
                    }
                }
            }
        }
    }
}
