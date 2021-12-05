using System.Drawing;

using TicTacToe.Board;

namespace TicTacToe.Led;

internal class LedManager
{
    public static bool IsWindows => Environment.OSVersion.Platform == PlatformID.Win32NT;

    public static void Init()
    {
        if (IsWindows)
        {
            LogiLED.Init("TicTacToe");
            LogiLED.SetTargetDevice(LogiLED.LOGI_DEVICETYPE_PERKEY_RGB);
            LogiLED.SaveCurrentLighting();
        }
    }

    public static void RestoreLighting()
    {
        if (IsWindows)
        {
            LogiLED.RestoreLighting();
        }
    }

    public static void SetDark()
    {
        if (IsWindows)
        {
            LogiLED.SetLighting(0, 0, 0);
        }
    }

    public static void FlashEnter(GameOverType gameOverType)
    {
        if (IsWindows)
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
    }

    public static void StopEffectOnKey(KeyNames key)
    {
        if (IsWindows)
        {
            LogiLED.StopEffectsOnKey(key);
        }
    }

    public static void SetBox(Box box, GameBoard board)
    {
        if (IsWindows)
        {
            var color = BoxStateToColorConverter.GetColorForBox(box);
            LogiLED.SetLightingForKeyWithKeyName(board.GetKeyNameFromBox(box), color);
        }
    }

    public static void Shutdown()
    {
        if (IsWindows)
        {
            LogiLED.RestoreLighting();
            LogiLED.Shutdown();
        }
    }
}
