namespace DG.Sudoku
{
    public enum ValueKind : short
    {
        Normal = 0,
        Given = CellDigit.GivenMask,
        Guess = CellDigit.GuessMask
    }
}
