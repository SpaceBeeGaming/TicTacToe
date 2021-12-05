using TicTacToe.Led;

namespace TicTacToe;

internal class Program
{
    public static Statistics Statistics { get; } = new();

    private static void Main()
    {

        LedManager.Init();

        bool play;
        do
        {
            play = RunGame();

        } while (play);

        LedManager.Shutdown();
        
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey(true);
    }

    private static bool RunGame()
    {
        Game game = new(Statistics);
        game.Setup();
        game.Play();
        game.AnnounceWinner();

        Console.WriteLine("\nPress <Enter> to play again.");
        Console.WriteLine("Or anything else to exit.\n");
        var key = Console.ReadKey(true).Key;
        LedManager.StopEffectOnKey(KeyNames.NUM_ENTER);
        if (key == ConsoleKey.Enter)
        {
            return true;
        }
        else
        {
            Console.WriteLine($"Games:  {Statistics.GamesPlayed}");
            Console.WriteLine($"Wins:   {Statistics.Wins}");
            Console.WriteLine($"L0sses: {Statistics.Losses}");
            Console.WriteLine($"Ties:   {Statistics.Ties}");
            Console.WriteLine($"Win %:  {Statistics.WinPercent}%");
            return false;
        }
    }
}
