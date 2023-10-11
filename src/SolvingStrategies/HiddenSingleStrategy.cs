using DG.Sudoku.CellData;
using DG.Sudoku.SolvingStrategies.Data;
using DG.Sudoku.Units;
using System.Collections.Generic;
using System.Linq;

namespace DG.Sudoku.SolvingStrategies
{
    /// <summary>
    /// Looks at units where only one cell in the unit can be a specific digit.
    /// </summary>
    public class HiddenSingleStrategy : ISolvingStrategy
    {
        /// <inheritdoc/>
        public string Name => "Hidden Singles";

        /// <inheritdoc/>
        public Difficulty Difficulty => Difficulty.Easy;

        /// <inheritdoc/>
        public IEnumerable<Candidate> FindCandidatesToRemove(ISolvingBoard board)
        {
            List<Candidate> valuesToRemove = new List<Candidate>();
            foreach (var unit in UnitTypes.All)
            {
                for (int i = 0; i < Board.SideLength; i++)
                {
                    var unitCells = board.GetCellsInUnit(unit, i);
                    valuesToRemove.AddRange(FindHiddenSingle(unitCells));
                }
            }
            return valuesToRemove;
        }

        private IEnumerable<Candidate> FindHiddenSingle(IEnumerable<Cell> unitCells)
        {
            for (int value = 1; value <= CellDigit.MaxValue; value++)
            {
                var cellsWithValue = unitCells.Where(c => c.Digit.CouldBe(value));
                if (cellsWithValue.Count() == 1)
                {
                    var cell = cellsWithValue.Single();
                    if (cell.Digit.IsKnown)
                    {
                        continue;
                    }
                    foreach (var option in cell.Digit.GetCandidatesWhere(o => o != value))
                    {
                        yield return Candidate.For(cell.Position, option);
                    }
                }
            }
        }
    }
}
