namespace DG.Sudoku
{
    /// <summary>
    /// Indicates if this digit is a regular digit, a guessed digit, or given by the puzzle.
    /// </summary>
    public enum DigitKnowledge
    {
        Normal,
        Given,
        Guess
    }
}
