using System.Collections.Generic;

namespace DG.Sudoku.SolvingStrategies
{
    public interface ISolvingStrategy
    {
        string Name { get; }
        Difficulty Difficulty { get; }
        IEnumerable<ValueInCell> FindValuesToRemove(Board board);
    }
}
