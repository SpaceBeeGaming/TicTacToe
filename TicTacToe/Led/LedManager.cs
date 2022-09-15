using System.Diagnostics;
using System.Drawing;

using TicTacToe.Board;

namespace TicTacToe.Led;

internal static class LedManager
{
    [Conditional("WINDOWS")]
    public static void Init()
    {
        LogiLED.Init("TicTacToe");
        LogiLED.SetTargetDevice(LogiLED.LOGI_DEVICETYPE_PERKEY_RGB);
        LogiLED.SaveCurrentLighting();
    }

    [Conditional("WINDOWS")]
    public static void RestoreLighting()
    {
        LogiLED.RestoreLighting();
    }

    [Conditional("WINDOWS")]
    public static void SetDark()
    {
        LogiLED.SetLighting(0, 0, 0);
    }

    [Conditional("WINDOWS")]
    public static void FlashEnter(GameOverType gameOverType)
    {
        switch (gameOverType)
        {
            case GameOverType.Tie:
                FlashKey(KeyNames.NUM_ENTER, Color.White);
                break;
            case GameOverType.X:
                FlashKey(KeyNames.NUM_ENTER, Color.Blue);
                break;
            case GameOverType.O:
                FlashKey(KeyNames.NUM_ENTER, Color.Red);
                break;
        }
    }

    [Conditional("WINDOWS")]
    public static void StopEffects()
    {
        LogiLED.StopEffects();

    }

    [Conditional("WINDOWS")]
    public static void SetBox(Box box, GameBoard board)
    {
        var color = BoxStateToColorConverter.GetColorForBox(box);
        LogiLED.SetLightingForKeyWithKeyName(board.GetKeyNameFromBox(box), color);
    }

    [Conditional("WINDOWS")]
    public static void Shutdown()
    {
        LogiLED.RestoreLighting();
        LogiLED.Shutdown();
    }

    [Conditional("WINDOWS")]
    public static void FlashWinningLine(GameOverType winner, GameBoard board)
    {
        Players player = EnumConverters.GameOverTypeToPlayersConverter(winner);

        Line? gameoverLine = board.GetLines(player, 3, true).FirstOrDefault();
        if (gameoverLine is not null)
        {
            List<KeyNames> keys = new();
            foreach (Box box in gameoverLine.Boxes)
            {
                keys.Add(board.GetKeyNameFromBox(box));
            }

            Task.Run(() => FlashKeysAsync(keys, BoxStateToColorConverter.GetColorForBox(gameoverLine.Boxes.First()), 500));
        }
    }

    private static void FlashKey(KeyNames keyname, Color color, int intervalMS = 500) =>
        LogiLED.FlashSingleKey(keyname, color, LogiLED.LOGI_LED_DURATION_INFINITE, intervalMS);

    private static async Task FlashKeysAsync(IEnumerable<KeyNames> keys, Color color, int offsetMS = 0, int intervalMS = 500)
    {
        await Task.Delay(offsetMS);
        foreach (KeyNames keyname in keys)
        {
            FlashKey(keyname, color, intervalMS);
        }
    }
}
