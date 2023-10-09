namespace DG.Sudoku
{
    /// <summary>
    /// The difficulty of a sudoku strategy or board.
    /// </summary>
    public enum Difficulty
    {
        /// <summary>
        /// Easy, anyone with a basic understanding of sudoku should be able to solve this.
        /// </summary>
        Easy,

        /// <summary>
        /// Basic difficulty, such as pointing pairs.
        /// </summary>
        Basic,

        /// <summary>
        /// Tough difficulty, such as x-wings.
        /// </summary>
        Tough,

        /// <summary>
        /// Diabolical difficulty, such as xy-chains.
        /// </summary>
        Diabolical,

        /// <summary>
        /// Extreme difficulty.
        /// </summary>
        Extreme,

        /// <summary>
        /// Unfair difficulty.
        /// </summary>
        Unfair
    }
}
