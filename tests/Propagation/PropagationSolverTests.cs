using DG.Sudoku.Propagation;
using System.Linq;
using Xunit;

namespace DG.Sudoku.Tests.Propagation
{
    public class PropagationSolverTests
    {
        private readonly TestingBoard _board = new TestingBoard();
        private readonly PropagationSolver _solver = PropagationSolver.Default;

        [Fact]
        public void FindCandidatesToRemove_ShouldNotContainSelf()
        {
            var initial = Position.For(1, 3);

            _board[initial.X, initial.Y] = Cell.ForKnown(initial.X, initial.Y, 7);

            var candidates = _solver.FindCandidatesToRemove(_board);

            Assert.DoesNotContain(candidates, c => c.Position == initial);
        }

        [Fact]
        public void FindCandidatesToRemove_ShouldNotOutsideBoundaryHaveSameDigit()
        {
            var initial = Position.For(1, 3);

            _board[initial.X, initial.Y] = Cell.ForKnown(initial.X, initial.Y, 7);

            var candidates = _solver.FindCandidatesToRemove(_board);

            Assert.All(candidates, c => Assert.True(c.Digit == 7));
        }

        [Fact]
        public void FindCandidatesToRemove_ShouldMatchAnyPositionProperty()
        {
            var initial = Position.For(1, 3);

            _board[initial.X, initial.Y] = Cell.ForKnown(initial.X, initial.Y, 7);

            var candidates = _solver.FindCandidatesToRemove(_board).Select(c => c.Position);

            Assert.All(candidates, p => Assert.True(p.X == initial.X || p.Y == initial.Y || p.Box == initial.Box));
        }

        [Fact]
        public void FindCandidatesToRemove_ReturnsCellsInRow()
        {
            var initial = Position.For(1, 3);
            _board[initial.X, initial.Y] = Cell.ForKnown(initial.X, initial.Y, 7);

            var candidates = _solver.FindCandidatesToRemove(_board).ToDictionary(c => c.Position, c => c.Digit);

            for (int x = 0; x < Board.SideLength; x++)
            {
                var position = Position.For(x, initial.Y);

                if (position == initial)
                {
                    continue;
                }

                Assert.True(candidates.ContainsKey(position), $"Candidates to remove should contain position {position.X}, {position.Y}");
            }
        }

        [Fact]
        public void FindCandidatesToRemove_ReturnsCellsInColumn()
        {
            var initial = Position.For(1, 3);
            _board[initial.X, initial.Y] = Cell.ForKnown(initial.X, initial.Y, 7);

            var candidates = _solver.FindCandidatesToRemove(_board).ToDictionary(c => c.Position, c => c.Digit);

            for (int y = 0; y < Board.SideLength; y++)
            {
                var position = Position.For(initial.X, y);

                if (position == initial)
                {
                    continue;
                }

                Assert.True(candidates.ContainsKey(position), $"Candidates to remove should contain position {position.X}, {position.Y}");
            }
        }

        [Fact]
        public void FindCandidatesToRemove_ReturnsCellsInBox()
        {
            var initial = Position.For(1, 3);
            int xOffsetBox = 0;
            int yOffsetBox = 3;
            _board[initial.X, initial.Y] = Cell.ForKnown(initial.X, initial.Y, 7);

            var candidates = _solver.FindCandidatesToRemove(_board).ToDictionary(c => c.Position, c => c.Digit);

            for (int y = 0; y < Board.BoxSize; y++)
            {
                for (int x = 0; x < Board.BoxSize; x++)
                {
                    var position = Position.For(x + xOffsetBox, y + yOffsetBox);

                    if (position == initial)
                    {
                        continue;
                    }

                    Assert.True(candidates.ContainsKey(position), $"Candidates to remove should contain position {position.X}, {position.Y}");
                }
            }
        }
    }
}
