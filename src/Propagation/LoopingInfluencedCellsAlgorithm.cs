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
        public IEnumerable<Cell> GetInfluencedCells(Board board, Cell cell)
        {
            for (int y = 0; y < Board.SideLength; y++)
            {
                for (int x = 0; x < Board.SideLength; x++)
                {
                    if (cell.Position.X == x && cell.Position.Y == y)
                    {
                        continue;
                    }

                    var otherCell = board[x, y];
                    if (x == cell.Position.X || y == cell.Position.Y || otherCell.Position.Box == cell.Position.Box)
                    {
                        yield return otherCell;
                    }
                }
            }
        }
    }
}
