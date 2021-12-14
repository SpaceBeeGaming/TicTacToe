using TicTacToe.Led;

namespace TicTacToe;

internal class Program
{
    public static Statistics Statistics { get; } = new();

    private static void Main()
    {
        // Initialize the LED System.
        LedManager.Init();

        // Run Tic-Tac-Toe in a loop.
        bool play;
        do
        {
            play = RunGame();

        } while (play);

        // Shutdown LED System.
        LedManager.Shutdown();

        // TODO: Move statistics printout here.


        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
    }

    private static bool RunGame()
    {
        // Game flow control.
        Game game = new(Statistics);
        game.Setup();
        game.Play();
        game.AnnounceWinner();

        Console.WriteLine("\nPress <Enter> to play again.");
        Console.WriteLine("Or anything else to exit.\n");

        // Determine if the player wants to play again.
        var key = Console.ReadKey(true).Key;
        LedManager.StopEffectOnKey(KeyNames.NUM_ENTER);
        if (key == ConsoleKey.Enter)
        {
            return true;
        }
        else
        {
            // TODO: Move to Main, since these make more sense there.
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

            return false;
        }
    }
}
