using Xunit;

namespace DG.Sudoku.Tests
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
        public void ForKnown_CouldBeAnything()
        {
            var value = CellDigit.ForKnown(7);

            Assert.False(value.CouldBe(3));
            Assert.False(value.CouldBe(9));
        }

        [Fact]
        public void Exclude_Works()
        {
            var value = CellDigit.ForUnknown();

            value.Exclude(1);
            value.Exclude(4);
            value.Exclude(9);

            Assert.False(value.CouldBe(4));
            Assert.False(value.CouldBe(9));
            Assert.False(value.CouldBe(1));
            Assert.True(value.CouldBe(2));
            Assert.True(value.CouldBe(3));
            Assert.True(value.CouldBe(7));

            Assert.False(value.HasSingleOption(out int _));

            value.Exclude(2);
            value.Exclude(3);
            value.Exclude(5);
            value.Exclude(6);
            value.Exclude(7);

            Assert.False(value.IsKnown);
            Assert.True(value.HasSingleOption(out int _));
        }

        [Fact]
        public void TrySetValue_MakesTypeGuess()
        {
            var value = CellDigit.ForUnknown();

            value.Exclude(1);
            value.Exclude(4);
            value.Exclude(9);
            value.TrySetValue(5);

            Assert.Equal(DigitKnowledge.Guessed, value.Type);
        }
    }
}
