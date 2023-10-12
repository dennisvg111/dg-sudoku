using DG.Sudoku.SolvingStrategies;
using DG.Sudoku.Units;
using System.Linq;
using Xunit;

namespace DG.Sudoku.Tests.SolvingStrategies
{
    public class HiddenSingleStrategyTests
    {
        private readonly TestingBoard _board = new TestingBoard();
        private readonly HiddenSingleStrategy _strategy = new HiddenSingleStrategy();

        [Fact]
        public void FindCandidatesToRemove_IsAllInSameCell()
        {
            CreateHiddenSingle(UnitType.Box, 0, Position.For(1, 2), 7);

            var candidates = _strategy.FindCandidatesToRemove(_board);

            Assert.All(candidates, c => Assert.True(c.Position == Position.For(1, 2)));
        }

        [Fact]
        public void FindCandidatesToRemove_AllOtherDigits()
        {
            CreateHiddenSingle(UnitType.Row, 4, Position.For(3, 4), 7);

            var candidates = _strategy.FindCandidatesToRemove(_board).Select(c => c.Digit);

            var digits = Enumerable.Range(1, 9).Where(d => d != 7);
            Assert.All(digits, d => Assert.Contains(candidates, c => c == d));
        }

        private void CreateHiddenSingle(UnitType unit, int unitIndex, Position positionOfSingle, int digit)
        {
            var cells = _board.GetCellsInUnit(unit, unitIndex).ToArray();

            for (int i = 0; i < cells.Length; i++)
            {
                var position = cells[i].Position;
                if (positionOfSingle == position)
                {
                    continue;
                }

                _board[position.X, position.Y] = Cell.With(position, cells[i].Digit.WithoutCandidate(digit));
            }
        }
    }
}
