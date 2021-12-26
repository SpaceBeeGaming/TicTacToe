using TicTacToe.Board;
using TicTacToe.Led;
using TicTacToe.Properties;

namespace TicTacToe;

/// <summary>
/// Contains the logic for running a game of Tic-Tac-Toe.
/// </summary>
public class Game
{
    private readonly Statistics _statistics;
    private readonly GameBoard _board = new();

    public Players HumanPlayer { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    /// <param name="statistics">Instance of the <see cref="Statistics"/> class to store the results into.</param>
    public Game(Statistics statistics)
    {
        _statistics = statistics;
    }

    public GameOverType Winner { get; private set; }

    /// <summary>
    /// Sets the game up.
    /// </summary>
    public void Setup()
    {
        _board.DrawBoard();
        LedManager.SetDark();

        // Randomize which side the human player is.
        HumanPlayer = (Players)Random.Shared.Next(2);
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
            _statistics.AddXStart();
        }

        // Save the offset for later use.
        int turnCountOffset = turnCounter;

        do
        {
            // Get the current progress of the game.
            var status = _board.CheckForWinner();

            // If the game has ended we set the result and break.
            if (status.gameOver)
            {
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
        _statistics.AddTurnCount(turnCounter - turnCountOffset);
    }

    /// <summary>
    /// Announces the winner of the current game.
    /// </summary>
    public void AnnounceWinner()
    {
        LedManager.FlashEnter(Winner);
        if (Winner is GameOverType.Tie)
        {
            Console.WriteLine(Resources.Tie);
            _statistics.AddTie();
        }
        else if (PlayerIsWinner())
        {
            Console.WriteLine(Resources.PlayerWin);
            _statistics.AddWin();
        }
        else
        {
            Console.WriteLine(Resources.PlayerLoss);
            _statistics.AddLoss();
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
                    ConsoleKey.D1 => _board.DrawPlayer(Boxes.B1, player),
                    ConsoleKey.D2 => _board.DrawPlayer(Boxes.B2, player),
                    ConsoleKey.D3 => _board.DrawPlayer(Boxes.B3, player),
                    ConsoleKey.D4 => _board.DrawPlayer(Boxes.B4, player),
                    ConsoleKey.D5 => _board.DrawPlayer(Boxes.B5, player),
                    ConsoleKey.D6 => _board.DrawPlayer(Boxes.B6, player),
                    ConsoleKey.D7 => _board.DrawPlayer(Boxes.B7, player),
                    ConsoleKey.D8 => _board.DrawPlayer(Boxes.B8, player),
                    ConsoleKey.D9 => _board.DrawPlayer(Boxes.B9, player),
                    _ => false,
                },
                // Using Numpad as input.
                _ => key switch
                {
                    ConsoleKey.NumPad7 => _board.DrawPlayer(Boxes.B1, player),
                    ConsoleKey.NumPad8 => _board.DrawPlayer(Boxes.B2, player),
                    ConsoleKey.NumPad9 => _board.DrawPlayer(Boxes.B3, player),
                    ConsoleKey.NumPad4 => _board.DrawPlayer(Boxes.B4, player),
                    ConsoleKey.NumPad5 => _board.DrawPlayer(Boxes.B5, player),
                    ConsoleKey.NumPad6 => _board.DrawPlayer(Boxes.B6, player),
                    ConsoleKey.NumPad1 => _board.DrawPlayer(Boxes.B7, player),
                    ConsoleKey.NumPad2 => _board.DrawPlayer(Boxes.B8, player),
                    ConsoleKey.NumPad3 => _board.DrawPlayer(Boxes.B9, player),
                    _ => false,
                },
            };
        } while (result is false);
    }

    private void OpponentTurn(Players player)
    {
        Box? idealBox = null;

        // Check if we have a line which is one away from a win.
        var winningLine = _board.GetWinningLine(player);
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
            var dangerousLines = _board.GetDangerousLines(player);

            if (dangerousLines.Count is not 0)
            {
                List<Box> priorityBoxes = new();

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
            var boxes = _board.GetEmptyBoxes().ToList();

            // Select the target randomly.
            idealBox = boxes[Random.Shared.Next(boxes.Count)];

        }

        _board.DrawPlayer(idealBox, player);
    }

    private void SetGameOverType(Players? winner) =>
        Winner = winner switch
        {
            Players.X => GameOverType.X,
            Players.O => GameOverType.O,
            _ => GameOverType.Tie,
        };

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
