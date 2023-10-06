namespace DG.Sudoku
{
    public class ValueInCell
    {
        private readonly Position _position;
        private readonly int _value;

        public Position Position => _position;
        public int Value => _value;

        public ValueInCell(Position position, int value)
        {
            _position = position;
            _value = value;
        }

        public static ValueInCell For(Position position, int value)
        {
            return new ValueInCell(position, value);
        }

        public static ValueInCell For(int x, int y, int value)
        {
            return new ValueInCell(Position.For(x, y), value);
        }
    }
}
