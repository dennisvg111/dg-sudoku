using DG.Sudoku.CellData;
using DG.Sudoku.Propagation;
using DG.Sudoku.SolvingStrategies;
using System;
using System.Threading;
using Output = System.Console;

namespace DG.Sudoku.Console
{
    internal static class Program
    {
        private static readonly ConsoleColor defaultColor = ConsoleColor.DarkGray;

        static void Main(string[] args)
        {
            Output.ForegroundColor = defaultColor;

            string input = "2...7..38.....6.7.3...4.6....8.2.7..1.......6..7.3.4....4.8...9.6.4.....91..6...2";
            if (!Board.TryParse(input, out Board board))
            {
                Output.WriteLine("Could not parse board from given input");
                Output.WriteLine("\"" + input + "\"");
            }

            var pipeline = SolvingPipeline
                .With(new HiddenSingleStrategy());
            Solver solver = new Solver(LoopingInfluencedCellsAlgorithm.Instance, pipeline);

            do
            {
                DrawDetailedBoard(board);
                Thread.Sleep(500);
            } while (solver.NextStep(board));
            Output.WriteLine("Could not find more solving steps");
        }

        private static void DrawDetailedBoard(Board board)
        {
            Output.SetCursorPosition(0, 0);
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
            DrawLegend();
        }

        private static void DrawDetailedCell(Cell cell, int optionsOffset)
        {
            for (int digit = 1; digit <= 3; digit++)
            {
                var digitToCheck = digit + optionsOffset * 3;
                if (cell.Digit.IsKnown)
                {
                    if (digitToCheck == 5)
                    {
                        WriteToOutput(cell.Digit.KnownValue.ToString(), cell.Digit.Type);
                    }
                    else
                    {
                        Output.Write(" ");
                    }
                }
                else
                {
                    var isPossible = cell.Digit.CouldBe(digitToCheck);
                    WriteToOutput(isPossible ? digitToCheck.ToString() : " ", cell.Digit.Type);
                }
                Output.Write(" ");
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

        private static void DrawLegend()
        {
            WriteToOutput("[Given] ", DigitKnowledge.Given);
            WriteToOutput("[Guessed] ", DigitKnowledge.Guessed);
            WriteToOutput("[Unknown] ", DigitKnowledge.Unknown);
            Output.WriteLine();
        }
    }
}
