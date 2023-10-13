namespace DG.Sudoku.SolvingStrategies.Data
{
    /// <summary>
    /// Provides extension methods for instances of <see cref="RelationType"/>.
    /// </summary>
    public static class RelationTypeExtensions
    {
        /// <summary>
        /// Converts this <see cref="RelationType"/> to a string representation, such as "Pairs", or "Triples".
        /// </summary>
        /// <param name="relationType"></param>
        /// <returns></returns>
        public static string ToNameString(this RelationType relationType)
        {
            return relationType + "s";
        }
    }
}
