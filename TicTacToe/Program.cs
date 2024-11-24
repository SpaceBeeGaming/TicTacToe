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
        ConsoleKeyInfo key = Console.ReadKey(true);
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
        ConsoleKey key = Console.ReadKey(true).Key;
        LedManager.StopEffects();
        return key is ConsoleKey.Enter;
    }

    private static void PrintStatistics()
    {
        Console.WriteLine($"""
        Games:  {Statistics.GamesPlayed}
        Wins:   {Statistics.Wins}
        Losses: {Statistics.Losses}
        Ties:   {Statistics.Ties}
        
        Win %:  {Statistics.WinPercent:0}%
        Loss %: {Statistics.LossPercent:0}%
        Tie %:  {Statistics.TiePercent:0}%
        
        Total Duration:   {Statistics.TotalGameDuration:m\:ss} min
        Average Duration: {Statistics.AverageGameDuration:s\:ff} s
        Average Turns:    {Statistics.AverageTurnCount}
        Your Starts:      {Statistics.XStartPercent:0}%
        """);
    }
}
