using TicTacToe.Board;
using TicTacToe.Led;

namespace TicTacToe;

public class Game
{
    public Game(Statistics statistics)
    {
        Statistics = statistics;
    }

    public GameBoard Board { get; } = new GameBoard();

    public GameOverType Winner { get; private set; }
    public Statistics Statistics { get; }

    public void Setup()
    {
        Board.DrawBoard();
        LedManager.SetDark();
    }

    public void Play()
    {
        int turnCounter = Random.Shared.Next(0, 2);
        (bool gameOver, Players? winner) status;
        do
        {
            status = Board.CheckForWinner();
            if (status.gameOver)
            {
                SetGameOverType(status.winner);
                break;
            }

            if (turnCounter % 2 is 0)
            {
                PlayerTurn();
            }
            else
            {
                OpponentTurn();
            }

            turnCounter++;

        } while (true);
    }

    public void AnnounceWinner()
    {
        LedManager.FlashEnter(Winner);
        switch (Winner)
        {
            case GameOverType.Tie:
                Console.WriteLine("Tie!");
                Statistics.Ties++;
                break;
            case GameOverType.X:
                Console.WriteLine($"You win!");
                Statistics.Wins++;
                break;
            case GameOverType.O:
                Console.WriteLine("You lose!");
                Statistics.Losses++;
                break;
        }
    }

    private void PlayerTurn()
    {
        bool result;
        do
        {
            var key = Console.ReadKey(true).Key;

            result = key switch
            {
                ConsoleKey.NumPad7 or ConsoleKey.D1 => Board.DrawX(Boxes.B1),
                ConsoleKey.NumPad8 or ConsoleKey.D2 => Board.DrawX(Boxes.B2),
                ConsoleKey.NumPad9 or ConsoleKey.D3 => Board.DrawX(Boxes.B3),
                ConsoleKey.NumPad4 or ConsoleKey.D4 => Board.DrawX(Boxes.B4),
                ConsoleKey.NumPad5 or ConsoleKey.D5 => Board.DrawX(Boxes.B5),
                ConsoleKey.NumPad6 or ConsoleKey.D6 => Board.DrawX(Boxes.B6),
                ConsoleKey.NumPad1 or ConsoleKey.D7 => Board.DrawX(Boxes.B7),
                ConsoleKey.NumPad2 or ConsoleKey.D8 => Board.DrawX(Boxes.B8),
                ConsoleKey.NumPad3 or ConsoleKey.D9 => Board.DrawX(Boxes.B9),
                _ => false,
            };
        } while (result is false);
    }

    // Opponent is 'O'
    private void OpponentTurn()
    {
        Box? idealBox = null;
        var winningLine = Board.GetWinningLine(Players.O);
        if (winningLine is not null)
        {
            idealBox = winningLine.GetEmptyBoxes().FirstOrDefault();
        }

        if (idealBox is null)
        {
            var dangerousLines = Board.GetDangerousLines(Players.O);

            if (dangerousLines.Count is not 0)
            {
                List<Box> priorityBoxes = new();
                foreach (var line in dangerousLines)
                {
                    priorityBoxes.AddRange(line.GetEmptyBoxes());
                }

                var bestBoxes = priorityBoxes.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

                idealBox = bestBoxes.MaxBy(x => x.Value).Key;
            }
        }

        if (idealBox is null)
        {
            var boxes = Board.GetEmptyBoxes().ToList();
            idealBox = boxes[Random.Shared.Next(0, boxes.Count)];

        }

        Board.DrawO(idealBox);
    }

    private void SetGameOverType(Players? winner) =>
        Winner = winner switch
        {
            Players.X => GameOverType.X,
            Players.O => GameOverType.O,
            _ => GameOverType.Tie,
        };

    public static Players GetOpposingPlayer(Players player) =>
        player switch
        {
            Players.X => Players.O,
            Players.O => Players.X,
            _ => throw new NotImplementedException()
        };
}
