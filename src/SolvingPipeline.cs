using DG.Sudoku.SolvingStrategies;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DG.Sudoku
{
    public class SolvingPipeline : IEnumerable<ISolvingStrategy>
    {
        private readonly List<ISolvingStrategy> _strategies;

        public IReadOnlyList<ISolvingStrategy> StrategyPipeline => _strategies;

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

        public bool TryPipeline(Board board, out string usedStrategy, out IEnumerable<ValueInCell> valuesToRemove)
        {
            usedStrategy = string.Empty;
            foreach (var strategy in _strategies)
            {
                usedStrategy = strategy.Name;
                if (strategy.TryFindValuesToRemove(board, out valuesToRemove))
                {
                    return true;
                }
            }
            valuesToRemove = Enumerable.Empty<ValueInCell>();
            return false;
        }

        public static SolvingPipeline WithStartingStrategy(ISolvingStrategy strategy)
        {
            var pipeline = new SolvingPipeline();
            pipeline._strategies.Add(strategy);
            return pipeline;
        }

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
