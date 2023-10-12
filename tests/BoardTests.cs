using DG.Sudoku.CellData;
using Xunit;

namespace DG.Sudoku.Tests
{
    public class BoardTests
    {
        private readonly TestingBoard _board = new TestingBoard();

        [Fact]
        public void RemoveCandidate_ChangesCouldBe()
        {
            var candidate = Candidate.For(0, 0, 6);
            Assert.True(_board[0, 0].Digit.CouldBe(6));

            _board.RemoveCandidate(candidate);

            Assert.False(_board[0, 0].Digit.CouldBe(6));
        }

        [Fact]
        public void RemoveCandidate_ChangesHasSingleCandidate()
        {
            _board[0, 0] = Cell.With(Position.For(0, 0), CellDigit.WithCandidates(6, 7));
            var candidate = Candidate.For(0, 0, 6);
            Assert.False(_board[0, 0].Digit.HasSingleCandidate(out int _));

            _board.RemoveCandidate(candidate);

            Assert.True(_board[0, 0].Digit.HasSingleCandidate(out int lastCandidate), $"{nameof(CellDigit.HasSingleCandidate)} should return true after removing candidates.");
            Assert.Equal(7, lastCandidate);
        }

        [Fact]
        public void RemoveCandidate_DoesNotChangeIsKnown()
        {
            _board[0, 0] = Cell.With(Position.For(0, 0), CellDigit.WithCandidates(6, 7));
            var candidate = Candidate.For(0, 0, 6);
            Assert.False(_board[0, 0].Digit.HasSingleCandidate(out int _));

            _board.RemoveCandidate(candidate);

            Assert.False(_board[0, 0].Digit.IsKnown, $"{nameof(CellDigit.IsKnown)} should still return false after removing candidates.");
        }

        [Fact]
        public void TrySolveCell_MultipleOptions()
        {
            _board[0, 0] = Cell.With(Position.For(0, 0), CellDigit.WithCandidates(6, 7, 8));

            bool canSolve = _board.TrySolveCell(0, 0);

            Assert.False(canSolve, $"{nameof(Board.TrySolveCell)} should return false for multiple options.");
            Assert.False(_board[0, 0].Digit.IsKnown, $"Digit should not be known after {nameof(Board.TrySolveCell)}.");
        }

        [Fact]
        public void TrySolveCell_SingleOption()
        {
            _board[0, 0] = Cell.With(Position.For(0, 0), CellDigit.WithCandidates(6));

            bool canSolve = _board.TrySolveCell(0, 0);

            Assert.True(canSolve, $"{nameof(Board.TrySolveCell)} should return true for single option.");
            Assert.True(_board[0, 0].Digit.IsKnown, $"Digit should be known after {nameof(Board.TrySolveCell)}.");
        }

        [Fact]
        public void TrySolveCell_AlreadyKnown()
        {
            _board[0, 0] = Cell.ForKnown(0, 0, 6);

            bool canSolve = _board.TrySolveCell(0, 0);

            Assert.False(canSolve, $"{nameof(Board.TrySolveCell)} should return false if the cell is already known.");
            Assert.True(_board[0, 0].Digit.IsKnown, $"Digit should still be known after {nameof(Board.TrySolveCell)}.");
        }
    }
}
