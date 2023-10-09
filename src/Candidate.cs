namespace DG.Sudoku
{
    /// <summary>
    /// This class represents a digit from 1 through 9 that could be located in a cell in a given <see cref="Position"/>.
    /// </summary>
    public class Candidate
    {
        private readonly Position _position;
        private readonly int _digit;

        /// <summary>
        /// The position of the cell this digit could be located in.
        /// </summary>
        public Position Position => _position;

        /// <summary>
        /// A digit from 1 through 9.
        /// </summary>
        public int Digit => _digit;

        /// <summary>
        /// Creates a new instance of <see cref="Candidate"/> with the given position and digit.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="digit"></param>
        public Candidate(Position position, int digit)
        {
            _position = position;
            _digit = digit;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Candidate"/> with the cell and digit.
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static Candidate For(Cell cell, int digit)
        {
            return new Candidate(cell.Position, digit);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Candidate"/> with the given position and digit.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static Candidate For(Position position, int digit)
        {
            return new Candidate(position, digit);
        }

        /// <summary>
        /// Creates a new instance of <see cref="Candidate"/> with the given x and y index as position, and the given digit.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static Candidate For(int x, int y, int digit)
        {
            return new Candidate(Position.For(x, y), digit);
        }
    }
}
