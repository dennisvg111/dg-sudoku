using DG.Sudoku.Propagation;
using DG.Sudoku.SolvingStrategies;
using System.Collections.Generic;

namespace DG.Sudoku
{
    /// <summary>
    /// Solves sudoku puzzles by propagating solved cells, and stepping through a <see cref="SolvingPipeline"/>.
    /// </summary>
    public class Solver
    {
        private readonly PropagationSolver _propagation;
        private readonly SolvingPipeline _pipeline;

        /// <summary>
        /// Initializes a new instance of <see cref="Solver"/> with the given algorithm for finding influenced cells, and <see cref="SolvingPipeline"/> containing implementations of <see cref="ISolvingStrategy"/>.
        /// </summary>
        /// <param name="influencedCellsAlgorithm"></param>
        /// <param name="pipeline"></param>
        public Solver(IInfluencedCellsAlgorithm influencedCellsAlgorithm, SolvingPipeline pipeline)
        {
            _propagation = new PropagationSolver(influencedCellsAlgorithm);
            _pipeline = pipeline;
        }

        /// <summary>
        /// Executes the <see cref="NextStep(Board)"/> method <paramref name="n"/> amount of times.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="n"></param>
        public void NextSteps(Board board, int n)
        {
            for (int i = 0; i < n; i++)
            {
                NextStep(board);
            }
        }

        /// <summary>
        /// <para>Tries to remove candidates from cells on the given board, and returns a value indicating if this was successful.</para>
        /// <para>This always happens 1 step at a time. A step can be any of the following:
        /// <list type="number">
        /// <item>Finding one or multiple cells have been solved (only 1 possible candidate).</item>
        /// <item>Eliminating candidates by propagating solved cells using an instance of <see cref="PropagationSolver"/>.</item>
        /// <item>Eliminating candidates using a strategy in the <see cref="SolvingPipeline"/>.</item>
        /// </list>
        /// </para>
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public bool NextStep(Board board)
        {
            int found = CheckForSolved(board);
            if (found > 0)
            {
                return true;
            }

            if (_propagation.TryFindCandidatesToRemove(board, out IEnumerable<Candidate> candidatesToRemove))
            {
                RemoveCandidates(board, candidatesToRemove);
                return true;
            }

            string strategy;
            if (_pipeline.TryPipeline(board, out strategy, out candidatesToRemove))
            {
                RemoveCandidates(board, candidatesToRemove);
                return true;
            }

            return false;
        }

        private void RemoveCandidates(Board board, IEnumerable<Candidate> candidates)
        {

            foreach (var value in candidates)
            {
                board.RemoveCandidate(value);
            }
        }

        /// <summary>
        /// Check for cells that are not marked as solved, but can only contain a single value. This function returns the amount of cells found.
        /// </summary>
        /// <param name="board"></param>
        /// <returns>The amount of cells that have been newly solved.</returns>
        private int CheckForSolved(Board board)
        {
            int found = 0;
            for (int x = 0; x < Board.SideLength; x++)
            {
                for (int y = 0; y < Board.SideLength; y++)
                {
                    if (board.TrySolveCell(x, y))
                    {
                        found++;
                    }
                }
            }
            return found;
        }
    }
}
