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

            value.RemoveOption(1);
            value.RemoveOption(4);
            value.RemoveOption(9);

            Assert.False(value.CouldBe(4));
            Assert.False(value.CouldBe(9));
            Assert.False(value.CouldBe(1));
            Assert.True(value.CouldBe(2));
            Assert.True(value.CouldBe(3));
            Assert.True(value.CouldBe(7));

            Assert.False(value.HasSingleOption(out int _));

            value.RemoveOption(2);
            value.RemoveOption(3);
            value.RemoveOption(5);
            value.RemoveOption(6);
            value.RemoveOption(7);

            Assert.False(value.IsKnown);
            Assert.True(value.HasSingleOption(out int _));
        }

        [Fact]
        public void TrySetValue_MakesTypeGuess()
        {
            var value = CellDigit.ForUnknown();

            value.RemoveOption(1);
            value.RemoveOption(4);
            value.RemoveOption(9);
            value.TryGuessValue(5);

            Assert.Equal(DigitKnowledge.Guessed, value.Type);
        }
    }
}
