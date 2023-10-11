using DG.Sudoku.CellData;
using Xunit;

namespace DG.Sudoku.Tests.CellData
{
    public class CellDigitTests
    {
        [Fact]
        public void ForUnknown_CouldBeAnything()
        {
            var value = CellDigit.ForUnknown();

            for (int i = 1; i <= CellDigit.MaxValue; i++)
            {
                var canBeI = value.CouldBe(i);
                Assert.True(canBeI);
            }
        }

        [Fact]
        public void ForKnown_CanOnlyBeValue()
        {
            var value = CellDigit.ForKnown(7);

            Assert.False(value.CouldBe(3));
            Assert.False(value.CouldBe(9));
            Assert.True(value.CouldBe(7));
        }

        [Fact]
        public void WithoutCandidate_Works()
        {
            var value = CellDigit.ForUnknown()
                .WithoutCandidate(1)
                .WithoutCandidate(4)
                .WithoutCandidate(9);

            Assert.False(value.CouldBe(4));
            Assert.False(value.CouldBe(9));
            Assert.False(value.CouldBe(1));
            Assert.True(value.CouldBe(2));
            Assert.True(value.CouldBe(3));
            Assert.True(value.CouldBe(7));

            Assert.False(value.HasSingleCandidate(out int _));

            value = value.WithoutCandidate(2)
            .WithoutCandidate(3)
            .WithoutCandidate(5)
            .WithoutCandidate(6)
            .WithoutCandidate(7);

            Assert.False(value.IsKnown);
            Assert.True(value.HasSingleCandidate(out int _));
        }

        [Fact]
        public void WithCandidates_Works()
        {
            var value = CellDigit.WithCandidates(1, 5, 9);

            for (int i = 2; i < 5; i++)
            {
                Assert.False(value.CouldBe(i));
            }
            for (int i = 6; i < 9; i++)
            {
                Assert.False(value.CouldBe(i));
            }

            Assert.True(value.CouldBe(1));
            Assert.True(value.CouldBe(5));
            Assert.True(value.CouldBe(9));
        }
    }
}
