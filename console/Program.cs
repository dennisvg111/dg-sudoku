using DG.Sudoku.SolvingStrategies;
using System;
using System.Threading;
using Output = System.Console;

namespace DG.Sudoku.Console
{
    internal class Program
    {
        private static readonly ConsoleColor defaultColor = ConsoleColor.DarkGray;

        static void Main(string[] args)
        {
            Output.ForegroundColor = defaultColor;

            string input = "...1.5...14....67..8...24...63.7..1.9.......3.1..9.52...72...8..26....35...4.9...";
            if (!Board.TryParse(input, out Board board))
            {
                Output.WriteLine("Could not parse board from given input");
                Output.WriteLine("\"" + input + "\"");
            }

            Solver solver = new Solver(new PropagationSolver(), new SolvingPipeline());

            do
            {
                DrawDetailedBoard(board);
                Thread.Sleep(500);
            } while (solver.NextStep(board));
            Output.WriteLine("Could not find more solving steps"); ;
        }

        private static void DrawBoard(Board board)
        {
            Output.Clear();
            for (int y = 0; y < Board.SideLength; y++)
            {
                for (int x = 0; x < Board.SideLength; x++)
                {
                    var cell = board[x, y];
                    WriteToOutput(cell.Digit.IsKnown ? cell.Digit.KnownValue.ToString() : "?", cell.Digit.Type);
                    Output.Write("  ");
                }
                Output.WriteLine();
                Output.WriteLine();
            }
        }

        private static void DrawDetailedBoard(Board board)
        {
            Output.Clear();
            for (int y = 0; y < Board.SideLength; y++)
            {
                for (int offset = 0; offset < 3; offset++)
                {
                    for (int x = 0; x < Board.SideLength; x++)
                    {
                        var cell = board[x, y];
                        DrawDetailedCell(cell, offset);
                    }
                    Output.WriteLine();
                }
                Output.WriteLine();
            }
        }

        private static void DrawDetailedCell(Cell cell, int optionsOffset)
        {
            for (int digit = 1; digit <= 3; digit++)
            {
                var digitToCheck = digit + optionsOffset * 3;
                var isPossible = cell.Digit.CouldBe(digitToCheck);
                WriteToOutput(isPossible ? (digitToCheck + " ") : "  ", cell.Digit.Type);
            }
            Output.Write(" ");
        }

        private static void WriteToOutput(string output, DigitKnowledge digitType)
        {
            if (digitType == DigitKnowledge.Given)
            {
                Output.ForegroundColor = ConsoleColor.Blue;
            }
            if (digitType == DigitKnowledge.Guessed)
            {
                Output.ForegroundColor = ConsoleColor.White;
            }
            Output.Write(output);
            Output.ForegroundColor = defaultColor;
        }
    }
}
