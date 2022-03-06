using System.Diagnostics;

using TicTacToe.Led;
using TicTacToe.Properties;

namespace TicTacToe;

internal class Program
{
    public static Statistics Statistics { get; } = new();
    public static Stopwatch Stopwatch { get; } = new();

    public static bool UseNumRow { get; private set; }
    public static bool AIMode { get; private set; }

    public static int AIRunsLeft { get; private set; }
    public static int ConsoleClearCount { get; private set; }

    private static void Main()
    {
        // Initialize the LED System.
        LedManager.Init();

        // Ask about AI mode
        Console.WriteLine("Press: <A> to run in AI vs AI mode.");
        var key = Console.ReadKey(true).Key;
        if (key is ConsoleKey.A)
        {
            AIMode = true;

            Console.Write("Number of Games: ");
            string? input;
            int result;
            do
            {
                input = Console.ReadLine();
            } while (int.TryParse(input, out result) is false);
            AIRunsLeft = result;

        }

        if (AIMode is false)
        {
            // Ask about alternate input method.
            Console.WriteLine("Press: <Backspace> to enable number row as input.");
            Console.WriteLine("This will disable numpad.");
            key = Console.ReadKey(true).Key;
            if (key is ConsoleKey.Backspace)
            {
                UseNumRow = true;
                Console.WriteLine("Set NumRow");
            }
        }

        // Run Tic-Tac-Toe in a loop.
        bool play;
        do
        {
            play = RunGame();

            if (AIMode)
            {
                // Failed AI Run
                if (play is false)
                {
                    play = true;
                    continue;
                }

                if (--AIRunsLeft is 0)
                {
                    break;
                }
            }
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
        try
        {
            game.Play();
        }
        // Catching exceptions when board goes outside buffer. Ending current game in a failure.
        catch (ArgumentOutOfRangeException ex)
        {
            if (ex.Source is "System.Console")
            {
                Console.Clear();
                Console.WriteLine($"Console Cleared! x{++ConsoleClearCount}");
                return false;
            }
        }

        game.AnnounceWinner();

        ConsoleKey key;
        if (AIMode is true)
        {
            key = ConsoleKey.Enter;
        }
        else
        {
            // Determine if the player wants to play again.
            key = Console.ReadKey(true).Key;
            Console.WriteLine();
            Console.WriteLine(Resources.PlayAgainPrompt);
        }

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

        Console.WriteLine($"Buffer Cleared: {ConsoleClearCount} times.");
    }
}
