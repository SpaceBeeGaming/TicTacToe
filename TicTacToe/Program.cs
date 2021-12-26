using TicTacToe.Led;
using TicTacToe.Properties;

namespace TicTacToe;

internal class Program
{
    public static Statistics Statistics { get; } = new();

    public static bool UseNumRow { get; private set; }

    private static void Main()
    {
        // Initialize the LED System.
        LedManager.Init();

        // Ask about alternate input method.
        Console.WriteLine("Press: <Ctrl + Enter> to enable number row as input.");
        Console.WriteLine("This will disable numpad.");
        var key = Console.ReadKey(true);
        if (key.Key is ConsoleKey.Enter && key.Modifiers is ConsoleModifiers.Control)
        {
            UseNumRow = true;
        }

        // Run Tic-Tac-Toe in a loop.
        bool play;
        do
        {
            play = RunGame();

        } while (play);

        // Shutdown LED System.
        LedManager.Shutdown();

        // Prints statistics about the session.
        Console.WriteLine($"Games:  {Statistics.GamesPlayed}");
        Console.WriteLine($"Wins:   {Statistics.Wins}");
        Console.WriteLine($"Losses: {Statistics.Losses}");
        Console.WriteLine($"Ties:   {Statistics.Ties}");
        Console.WriteLine();
        Console.WriteLine($"Win %:  {Statistics.WinPercent:0}%");
        Console.WriteLine($"Loss %: {Statistics.LossPercent:0}%");
        Console.WriteLine($"Tie %:  {Statistics.TiePercent:0}%");
        Console.WriteLine();
        Console.WriteLine($"Average turns: {Statistics.AverageTurnCount}");
        Console.WriteLine($"Your starts:   {Statistics.XStartPercent:0}%");

        Console.WriteLine(Resources.ExitPrompt);
        Console.ReadKey(true);
    }

    private static bool RunGame()
    {
        // Game flow control.
        Game game = new(Statistics);
        game.Setup();
        game.Play();
        game.AnnounceWinner();

        Console.WriteLine();
        Console.WriteLine(Resources.PlayAgainPrompt);

        // Determine if the player wants to play again.
        var key = Console.ReadKey(true).Key;
        LedManager.StopEffectOnKey(KeyNames.NUM_ENTER);
        return key is ConsoleKey.Enter;
    }
}
