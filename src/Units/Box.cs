namespace DG.Sudoku.Units
{
    /// <summary>
    /// Values of this enumeration represent a specific 3x3 box inside a sudoku.
    /// </summary>
    public enum Box
    {
        /// <summary>
        /// The 3x3 box in the top-left.
        /// </summary>
        TopLeft = 0,

        /// <summary>
        /// The 3x3 box in the center at the top of the sudoku. 
        /// </summary>
        TopCenter = 1,

        /// <summary>
        /// The 3x3 box in the top-right. 
        /// </summary>
        TopRight = 2,

        /// <summary>
        /// The 3x3 box left in the vertical center. 
        /// </summary>
        CenterLeft = 3,

        /// <summary>
        /// The 3x3 box in the center of the sudoku. 
        /// </summary>
        Center = 4,

        /// <summary>
        /// The 3x3 box right in the vertical center. 
        /// </summary>
        CenterRight = 5,

        /// <summary>
        /// The 3x3 box in the bottom-left. 
        /// </summary>
        BottomLeft = 6,

        /// <summary>
        /// The 3x3 box in the center at the bottom of the sudoku. 
        /// </summary>
        BottomCenter = 7,

        /// <summary>
        /// The 3x3 box in the bottom-right. 
        /// </summary>
        BottomRight = 8
    }
}
