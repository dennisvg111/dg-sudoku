using System.Collections.Generic;
using System.Linq;

namespace DG.Sudoku.SolvingStrategies
{
    public static class ISolvingStrategyExtensions
    {
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
