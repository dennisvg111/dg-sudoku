namespace DG.Sudoku.SolvingStrategies.Data
{
    /// <summary>
    /// Represents the type of relation 2 or multiple cells have.
    /// </summary>
    public enum RelationType
    {
        /// <summary>
        /// Indicates two cells have some relation.
        /// </summary>
        Pair = 2,

        /// <summary>
        /// Indicates three cells have some relation.
        /// </summary>
        Triple = 3,

        /// <summary>
        /// Indicates four cells have some relation.
        /// </summary>
        Quad = 4
    }
}
