using System.Collections.Generic;

namespace DG.Sudoku.SolvingStrategies
{
    public class PropagationSolver : ISolvingStrategy
    {
        public string Name => "Propagation";
        public Difficulty Difficulty => Difficulty.Easy;

        public IEnumerable<PossibleDigitInCell> FindValuesToRemove(Board board)
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
                            yield return PossibleDigitInCell.For(otherCell.Position, cell.Digit.KnownValue);
                        }
                    }
                }
            }
        }
    }
}
