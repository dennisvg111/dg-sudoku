using System;
using System.Runtime.Serialization;

namespace DG.Sudoku.Exceptions
{

    [Serializable]
    public class ExhaustedCellException : Exception
    {
        public Cell Cell { get; set; }

        public ExhaustedCellException(Cell cell, string message) : base($"[Cell {cell.X + 1},{cell.Y + 1}] " + message)
        {

        }

        protected ExhaustedCellException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
