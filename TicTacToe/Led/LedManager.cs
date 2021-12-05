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
