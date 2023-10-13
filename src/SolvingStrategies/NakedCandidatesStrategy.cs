using DG.Sudoku.SolvingStrategies.Data;
using System;
using System.Collections.Generic;

namespace DG.Sudoku.SolvingStrategies
{
    /// <summary>
    /// This strategy looks for a cluster of cells that can only contain the same number of candidates as there are cells, and removes candidates from other cells in that region.
    /// </summary>
    public class NakedCandidatesStrategy : ISolvingStrategy
    {
        private readonly RelationType _type;

        /// <summary>
        /// Initializes a new instance of <see cref="NakedCandidatesStrategy"/> for the given <see cref="RelationType"/>.
        /// </summary>
        /// <param name="type"></param>
        public NakedCandidatesStrategy(RelationType type)
        {
            _type = type;
        }

        /// <inheritdoc/>
        public string Name => "Naked " + _type.ToNameString();

        /// <inheritdoc/>
        public Difficulty Difficulty => _type == RelationType.Quad ? Difficulty.Basic : Difficulty.Easy;

        /// <inheritdoc/>
        public IEnumerable<Candidate> FindCandidatesToRemove(ISolvingBoard board)
        {
            throw new NotImplementedException();
        }
    }
}
