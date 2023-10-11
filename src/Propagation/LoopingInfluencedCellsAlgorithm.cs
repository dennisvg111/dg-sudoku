using DG.Sudoku.SolvingStrategies.Data;
using System.Collections.Generic;

namespace DG.Sudoku.Propagation
{
    /// <summary>
    /// The default implementation for <see cref="IInfluencedCellsAlgorithm"/>, which simply loops over all positions on the board.
    /// </summary>
    public class LoopingInfluencedCellsAlgorithm : IInfluencedCellsAlgorithm
    {
        private static readonly LoopingInfluencedCellsAlgorithm _instance = new LoopingInfluencedCellsAlgorithm();

        /// <summary>
        /// The default shared instance of <see cref="LoopingInfluencedCellsAlgorithm"/>.
        /// </summary>
        public static LoopingInfluencedCellsAlgorithm Instance => _instance;

        /// <inheritdoc/>
        public IEnumerable<Cell> GetInfluencedCells(ISolvingBoard board, Cell cell)
        {
            foreach (var otherCell in board.GetAllCells())
            {
                var otherPosition = otherCell.Position;
                if (cell.Position == otherPosition)
                {
                    continue;
                }

                if (otherPosition.X == cell.Position.X || otherPosition.Y == cell.Position.Y || otherPosition.Box == cell.Position.Box)
                {
                    yield return otherCell;
                }
            }
        }
    }
}
