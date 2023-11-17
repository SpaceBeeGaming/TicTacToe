using System.Diagnostics;

using TicTacToe.Board;
using TicTacToe.Led;
using TicTacToe.Properties;

namespace TicTacToe;

/// <summary>
/// Contains the logic for running a game of Tic-Tac-Toe.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="Game"/> class.
/// </remarks>
/// <param name="statistics">Instance of the <see cref="Statistics"/> class to store the results into.</param>
/// <param name="stopwatch">Instance of the <see cref="Stopwatch"/> class to clock the game runs.</param>
public sealed class Game(Statistics statistics, Stopwatch stopwatch)
{
    private readonly Statistics statistics = statistics;
    private readonly Stopwatch stopwatch = stopwatch;
    private readonly GameBoard board = new();

    public Players HumanPlayer { get; private set; }

    public GameOverType Winner { get; private set; }

    /// <summary>
    /// Sets the game up.
    /// </summary>
    public void Setup()
    {
        board.DrawBoard();
        LedManager.SetDark();

        // Randomize which side the human player is.
        HumanPlayer = (Players)Random.Shared.Next(1, 3);
        Console.WriteLine($"You are: {HumanPlayer}");
    }

    /// <summary>
    /// Executes the game loop.
    /// </summary>
    public void Play()
    {
        int turnCounter = 0;

        // Offset the turn counter if human player is supposed to go second.
        if (HumanPlayer is Players.O)
        {
            turnCounter++;
        }
        else
        {
            statistics.AddXStart();
        }

        // Save the offset for later use.
        int turnCountOffset = turnCounter;

        stopwatch.Restart();
        do
        {
            // Get the current progress of the game.
            var status = board.CheckForWinner();

            // If the game has ended we set the result and break.
            if (status.gameOver)
            {
                stopwatch.Stop();
                SetGameOverType(status.winner);
                break;
            }

            // Alternate between the two players.
            if (turnCounter % 2 is 0)
            {
                PlayerTurn(HumanPlayer);
            }
            else
            {
                OpponentTurn(GetOpposingPlayer(HumanPlayer));
            }

            turnCounter++;

        } while (true);

        // Remove the added offset before submitting the turn count.
        statistics.AddTurnCount(turnCounter - turnCountOffset);
        statistics.AddGameDuration(stopwatch.ElapsedMilliseconds);
    }

    /// <summary>
    /// Announces the winner of the current game.
    /// </summary>
    public void AnnounceWinner()
    {
        if (Winner is not GameOverType.Tie)
        {
            LedManager.FlashWinningLine(Winner, board);
        }

        LedManager.FlashEnter(Winner);
        if (Winner is GameOverType.Tie)
        {
            Console.WriteLine(Resources.Tie);
            statistics.AddTie();
        }
        else if (PlayerIsWinner())
        {
            Console.WriteLine(Resources.PlayerWin);
            statistics.AddWin();
        }
        else
        {
            Console.WriteLine(Resources.PlayerLoss);
            statistics.AddLoss();
        }

        bool PlayerIsWinner() => (Winner is GameOverType.X && HumanPlayer is Players.X) || (Winner is GameOverType.O && HumanPlayer is Players.O);
    }

    private void PlayerTurn(Players player)
    {
        bool result;
        do
        {
            ConsoleKey key = Console.ReadKey(true).Key;

            result = Program.UseNumRow switch
            {
                // Using Number row as input. Assuming that the user doesn't have a Numpad.
                true => key switch
                {
                    ConsoleKey.D1 => board.DrawPlayer(Boxes.B1, player),
                    ConsoleKey.D2 => board.DrawPlayer(Boxes.B2, player),
                    ConsoleKey.D3 => board.DrawPlayer(Boxes.B3, player),
                    ConsoleKey.D4 => board.DrawPlayer(Boxes.B4, player),
                    ConsoleKey.D5 => board.DrawPlayer(Boxes.B5, player),
                    ConsoleKey.D6 => board.DrawPlayer(Boxes.B6, player),
                    ConsoleKey.D7 => board.DrawPlayer(Boxes.B7, player),
                    ConsoleKey.D8 => board.DrawPlayer(Boxes.B8, player),
                    ConsoleKey.D9 => board.DrawPlayer(Boxes.B9, player),
                    _ => false,
                },
                // Using Numpad as input.
                _ => key switch
                {
                    ConsoleKey.NumPad7 => board.DrawPlayer(Boxes.B1, player),
                    ConsoleKey.NumPad8 => board.DrawPlayer(Boxes.B2, player),
                    ConsoleKey.NumPad9 => board.DrawPlayer(Boxes.B3, player),
                    ConsoleKey.NumPad4 => board.DrawPlayer(Boxes.B4, player),
                    ConsoleKey.NumPad5 => board.DrawPlayer(Boxes.B5, player),
                    ConsoleKey.NumPad6 => board.DrawPlayer(Boxes.B6, player),
                    ConsoleKey.NumPad1 => board.DrawPlayer(Boxes.B7, player),
                    ConsoleKey.NumPad2 => board.DrawPlayer(Boxes.B8, player),
                    ConsoleKey.NumPad3 => board.DrawPlayer(Boxes.B9, player),
                    _ => false,
                },
            };
        } while (result is false);
    }

    private void OpponentTurn(Players player)
    {
        Box? idealBox = null;

        // Check if we have a line which is one away from a win.
        var winningLine = board.GetWinningLine(player);
        if (winningLine is not null)
        {
            // Set the missing box as the ideal target.
            idealBox = winningLine.GetEmptyBoxes().FirstOrDefault();
        }

        // If we previously didn't find a suitable box we proceed.
        // If we did, we skip.
        if (idealBox is null)
        {
            // Check if we have any lines where the opponent is one away from winning.
            var dangerousLines = board.GetDangerousLines(player);

            if (dangerousLines.Count is not 0)
            {
                List<Box> priorityBoxes = [];

                // Iterate over all the lines founds to be dangerous.
                foreach (var line in dangerousLines)
                {
                    // Add the empty boxes from these dangerous lines into a list.
                    priorityBoxes.AddRange(line.GetEmptyBoxes());
                }

                // Group the boxes into a dictionary by the number of times they appear.
                var bestBoxes = priorityBoxes.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

                // Select the box which is contained in largest number of lines.
                idealBox = bestBoxes.MaxBy(x => x.Value).Key;
            }
        }

        // If we previously didn't find a suitable box we proceed.
        // If we did, we skip.
        if (idealBox is null)
        {
            // Get a list of all empty boxes.
            var boxes = board.GetEmptyBoxes().ToList();

            // Select the target randomly.
            idealBox = boxes[Random.Shared.Next(boxes.Count)];

        }

        board.DrawPlayer(idealBox, player);
    }

    private void SetGameOverType(Players winner) =>
        Winner = EnumConverters.PlayersToGameOverTypeConverter(winner);

    /// <summary>
    /// Returns the opposing player.
    /// </summary>
    /// <param name="player">An instance of <see cref="Players"/>.</param>
    /// <returns>The opposing player.</returns>
    /// <exception cref="ArgumentException">Thrown if input is unexpected.</exception>
    public static Players GetOpposingPlayer(Players player) =>
        player switch
        {
            Players.X => Players.O,
            Players.O => Players.X,
            _ => throw new ArgumentException("Invalid Player", nameof(player))
        };
}
