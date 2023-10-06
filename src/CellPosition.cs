namespace DG.Sudoku
{
    public class CellPosition
    {
        private readonly int _x;
        private readonly int _y;
        private readonly int _box;

        /// <summary>
        /// The zero-indexed column this cell is located in.
        /// </summary>
        public int X => _x;

        /// <summary>
        /// The zero-indexed row this cell is located in.
        /// </summary>
        public int Y => _y;

        /// <summary>
        /// The zero-indexed box this cell is located in.
        /// </summary>
        public int Box => _box;

        public CellPosition(int x, int y)
        {
            _x = x;
            _y = y;
            _box = (x / 3) + ((y / 3) * 3);
        }

        public override string ToString()
        {
            return $"{_x},{_y}";
        }
    }
}
