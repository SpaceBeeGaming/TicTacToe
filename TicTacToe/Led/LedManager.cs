using System.Diagnostics;
using System.Drawing;

using TicTacToe.Board;

namespace TicTacToe.Led;

internal class LedManager
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
                LogiLED.FlashSingleKey(KeyNames.NUM_ENTER, Color.White, LogiLED.LOGI_LED_DURATION_INFINITE, 500);
                break;
            case GameOverType.X:
                LogiLED.FlashSingleKey(KeyNames.NUM_ENTER, Color.Blue, LogiLED.LOGI_LED_DURATION_INFINITE, 500);
                break;
            case GameOverType.O:
                LogiLED.FlashSingleKey(KeyNames.NUM_ENTER, Color.Red, LogiLED.LOGI_LED_DURATION_INFINITE, 500);
                break;
        }
    }

    [Conditional("WINDOWS")]
    public static void StopEffectOnKey(KeyNames key)
    {
        LogiLED.StopEffectsOnKey(key);
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
}
