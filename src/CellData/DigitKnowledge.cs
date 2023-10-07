namespace DG.Sudoku.CellData
{
    /// <summary>
    /// Indicates if this digit is an unknown digit, a guessed digit, or given by the puzzle.
    /// </summary>
    public enum DigitKnowledge
    {
        /// <summary>
        /// This digit is not yet known for sure.
        /// </summary>
        Unknown,

        /// <summary>
        /// This digit is given.
        /// </summary>
        Given,

        /// <summary>
        /// This digit is guessed.
        /// </summary>
        Guessed
    }
}
