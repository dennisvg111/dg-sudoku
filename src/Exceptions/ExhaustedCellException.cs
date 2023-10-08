using System;
using System.Runtime.Serialization;

namespace DG.Sudoku.Exceptions
{
    /// <summary>
    /// Represents errors that occur due to cells that have no more possible digits.
    /// </summary>
    [Serializable]
    public class ExhaustedCellException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ExhaustedCellException"/> for a cell at the given position.
        /// </summary>
        /// <param name="position"></param>
        public ExhaustedCellException(Position position) : base($"Cell at column {position.X + 1}, row {position.Y + 1} has no more remaining possible digits.")
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="ExhaustedCellException"/> with serialized data.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ExhaustedCellException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
