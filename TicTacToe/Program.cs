using System.Diagnostics;

using TicTacToe.Led;
using TicTacToe.Properties;

namespace TicTacToe;

internal static class Program
{
    public static Statistics Statistics { get; } = new();
    public static Stopwatch Stopwatch { get; } = new();

    public static bool UseNumRow { get; private set; }

    private static void Main()
    {
        // Initialize the LED System.
        LedManager.Init();

        // Ask about alternate input method.
        Console.WriteLine("Press: <Backspace> to enable number row as input.");
        Console.WriteLine("This will disable numpad.");
        var key = Console.ReadKey(true);
        if (key.Key is ConsoleKey.Backspace)
        {
            UseNumRow = true;
            Console.WriteLine("Set NumRow");
        }

        // Run Tic-Tac-Toe in a loop.
        bool play;
        do
        {
            play = RunGame();

        } while (play);

        // Shutdown LED System.
        LedManager.Shutdown();

        PrintStatistics();

        Console.WriteLine(Resources.ExitPrompt);
        Console.ReadKey(true);
    }

    private static bool RunGame()
    {
        // Game flow control.
        Game game = new(Statistics, Stopwatch);
        game.Setup();
        game.Play();
        game.AnnounceWinner();

        Console.WriteLine();
        Console.WriteLine(Resources.PlayAgainPrompt);

        // Determine if the player wants to play again.
        var key = Console.ReadKey(true).Key;
        LedManager.StopEffects();
        return key is ConsoleKey.Enter;
    }

    private static void PrintStatistics()
    {
        Console.WriteLine($"Games:  {Statistics.GamesPlayed}");
        Console.WriteLine($"Wins:   {Statistics.Wins}");
        Console.WriteLine($"Losses: {Statistics.Losses}");
        Console.WriteLine($"Ties:   {Statistics.Ties}");
        Console.WriteLine();
        Console.WriteLine($"Win %:  {Statistics.WinPercent:0}%");
        Console.WriteLine($"Loss %: {Statistics.LossPercent:0}%");
        Console.WriteLine($"Tie %:  {Statistics.TiePercent:0}%");
        Console.WriteLine();
        Console.WriteLine($"Total Duration:   {Statistics.TotalGameDuration:m\\:ss} min");
        Console.WriteLine($"Average Duration: {Statistics.AverageGameDuration:s\\:ff} s");
        Console.WriteLine($"Average Turns:    {Statistics.AverageTurnCount}");
        Console.WriteLine($"Your Starts:      {Statistics.XStartPercent:0}%");
    }
}
