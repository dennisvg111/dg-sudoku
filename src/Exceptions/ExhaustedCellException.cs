using System;
using System.Runtime.Serialization;

namespace DG.Sudoku.Exceptions
{
    [Serializable]
    public class ExhaustedCellException : Exception
    {
        public ExhaustedCellException(string message) : base(message)
        {

        }

        protected ExhaustedCellException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }
    }
}
