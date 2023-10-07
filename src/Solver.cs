﻿using DG.Sudoku.SolvingStrategies;
using System.Collections.Generic;

namespace DG.Sudoku
{
    public class Solver
    {
        private ISolvingStrategy _propagation;
        private readonly SolvingPipeline _pipeline;

        public Solver(ISolvingStrategy propagation, SolvingPipeline pipeline)
        {
            _propagation = propagation;
            _pipeline = pipeline;
        }

        public void NextSteps(Board board, int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                NextStep(board);
            }
        }

        public bool NextStep(Board board)
        {
            int found = CheckForSolved(board);
            if (found > 0)
            {
                return true;
            }

            if (_propagation.TryFindValuesToRemove(board, out IEnumerable<ValueInCell> valuesToRemove))
            {
                RemoveOptions(board, valuesToRemove);
                return true;
            }

            string strategy;
            if (_pipeline.TryPipeline(board, out strategy, out valuesToRemove))
            {
                RemoveOptions(board, valuesToRemove);
                return true;
            }

            return false;
        }

        private void RemoveOptions(Board board, IEnumerable<ValueInCell> valuesToRemove)
        {

            foreach (var value in valuesToRemove)
            {
                board[value.Position].Digit.Exclude(value.Value);
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
                    var cell = board[x, y];
                    if (cell.Digit.IsKnown)
                    {
                        continue;
                    }
                    int option;
                    if (cell.Digit.HasSingleOption(out option))
                    {
                        cell.Digit.TrySetValue(option);
                        found++;
                    }
                }
            }
            return found;
        }
    }
}