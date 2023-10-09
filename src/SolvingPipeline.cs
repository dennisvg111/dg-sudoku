using DG.Sudoku.SolvingStrategies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DG.Sudoku
{
    /// <summary>
    /// A collection of implementations of <see cref="ISolvingStrategy"/> that will be tried in order to solve a sudoku.
    /// </summary>
    public class SolvingPipeline : IEnumerable<ISolvingStrategy>
    {
        private readonly List<ISolvingStrategy> _strategies;

        /// <summary>
        /// The list of strategies this pipeline has available.
        /// </summary>
        public IReadOnlyList<ISolvingStrategy> StrategyPipeline => _strategies;

        /// <summary>
        /// Initializes a new instance of <see cref="SolvingPipeline"/> without any strategies.
        /// </summary>
        public SolvingPipeline()
        {
            _strategies = new List<ISolvingStrategy>();
        }

        private SolvingPipeline(IEnumerable<ISolvingStrategy> strategies)
        {
            _strategies = strategies.ToList();
        }

        private SolvingPipeline Copy()
        {
            return new SolvingPipeline(_strategies);
        }

        /// <summary>
        /// Tries to find candidates to be removed from cells using the first possible implementation of <see cref="ISolvingStrategy"/>.
        /// </summary>
        /// <param name="board"></param>
        /// <param name="usedStrategy"></param>
        /// <param name="candidatesToRemove"></param>
        /// <returns></returns>
        public bool TryPipeline(Board board, out string usedStrategy, out IEnumerable<Candidate> candidatesToRemove)
        {
            usedStrategy = string.Empty;
            foreach (var strategy in _strategies)
            {
                usedStrategy = strategy.Name;
                if (strategy.TryFindCandidatesToRemove(board, out candidatesToRemove))
                {
                    return true;
                }
            }
            candidatesToRemove = Enumerable.Empty<Candidate>();
            return false;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SolvingPipeline"/> with the given strategy as the first to be tried.
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public static SolvingPipeline With(ISolvingStrategy strategy)
        {
            var pipeline = new SolvingPipeline();
            pipeline._strategies.Add(strategy);
            return pipeline;
        }

        /// <summary>
        /// Adds the given strategy at the back of the list of strategies to a new instance of <see cref="SolvingPipeline"/>, and returns this new pipeline.
        /// </summary>
        /// <param name="strategy"></param>
        /// <returns></returns>
        public SolvingPipeline ThenWith(ISolvingStrategy strategy)
        {
            var pipeline = Copy();
            pipeline._strategies.Add(strategy);
            return pipeline;
        }

        /// <inheritdoc/>
        public IEnumerator<ISolvingStrategy> GetEnumerator()
        {
            return StrategyPipeline.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
