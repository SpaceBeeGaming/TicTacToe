using System.Diagnostics;

using TicTacToe.Led;

namespace TicTacToe;

internal class Program
{
    public static Statistics Statistics { get; } = new();
    public static Stopwatch Stopwatch { get; } = new();

    private static void Main()
    {

        LedManager.Init();

        bool play;
        do
        {
            play = RunGame();

        } while (play);

        LedManager.Shutdown();

        PrintStatistics();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
    }

    private static bool RunGame()
    {
        Game game = new(Statistics, Stopwatch);
        game.Setup();
        game.Play();
        game.AnnounceWinner();

        Console.WriteLine("\nPress <Enter> to play again.");
        Console.WriteLine("Or anything else to exit.\n");
        var key = Console.ReadKey(true).Key;
        LedManager.StopEffectOnKey(KeyNames.NUM_ENTER);

        return key == ConsoleKey.Enter;
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
