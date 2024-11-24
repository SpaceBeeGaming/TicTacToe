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
    public static void RestoreLighting() => LogiLED.RestoreLighting();

    [Conditional("WINDOWS")]
    public static void SetDark() => LogiLED.SetLighting(0, 0, 0);

    [Conditional("WINDOWS")]
    public static void FlashEnter(GameOverType gameOverType)
    {
        switch (gameOverType)
        {
            case GameOverType.Tie:
                FlashKey(KeyNames.NumEnter, Color.White);
                break;
            case GameOverType.X:
                FlashKey(KeyNames.NumEnter, Color.Blue);
                break;
            case GameOverType.O:
                FlashKey(KeyNames.NumEnter, Color.Red);
                break;
            default:
                throw new InvalidOperationException();
        }
    }

    [Conditional("WINDOWS")]
    public static void StopEffects() => LogiLED.StopEffects();

    [Conditional("WINDOWS")]
    public static void SetBox(Box box, GameBoard board)
    {
        Color color = BoxStateToColorConverter.GetColorForBox(box);
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

        Line? gameOverLine = board.GetLines(player, 3, true).FirstOrDefault();
        if (gameOverLine is not null)
        {
            List<KeyNames> keys = [];
            foreach (Box box in gameOverLine.Boxes)
            {
                keys.Add(board.GetKeyNameFromBox(box));
            }

            Task.Run(() => FlashKeysAsync(keys, BoxStateToColorConverter.GetColorForBox(gameOverLine.Boxes.First()), 500));
        }
    }

    private static void FlashKey(KeyNames keyName, Color color, int intervalMS = 500) =>
        LogiLED.FlashSingleKey(keyName, color, LogiLED.LOGI_LED_DURATION_INFINITE, intervalMS);

    private static async Task FlashKeysAsync(IEnumerable<KeyNames> keys, Color color, int offsetMS = 0, int intervalMS = 500)
    {
        await Task.Delay(offsetMS);
        foreach (KeyNames keyName in keys)
        {
            FlashKey(keyName, color, intervalMS);
        }
    }
}
