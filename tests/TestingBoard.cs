namespace DG.Sudoku.Tests
{
    public class TestingBoard : Board
    {
        public new Cell this[int x, int y]
        {
            get
            {
                return base[x, y];
            }
            set
            {
                var index = ConvertToArrayIndex(x, y);
                _cells[index] = value;
            }
        }
    }
}
