using System.Runtime.InteropServices;

namespace TicTacToe.Led;

internal class LogiLED
{
    //LED SDK
    private const int LOGI_DEVICETYPE_MONOCHROME_ORD = 0;
    private const int LOGI_DEVICETYPE_RGB_ORD = 1;
    private const int LOGI_DEVICETYPE_PERKEY_RGB_ORD = 2;

    public const int LOGI_DEVICETYPE_MONOCHROME = 1 << LOGI_DEVICETYPE_MONOCHROME_ORD;
    public const int LOGI_DEVICETYPE_RGB = 1 << LOGI_DEVICETYPE_RGB_ORD;
    public const int LOGI_DEVICETYPE_PERKEY_RGB = 1 << LOGI_DEVICETYPE_PERKEY_RGB_ORD;
    public const int LOGI_DEVICETYPE_ALL = LOGI_DEVICETYPE_MONOCHROME | LOGI_DEVICETYPE_RGB | LOGI_DEVICETYPE_PERKEY_RGB;

    public const int LOGI_LED_BITMAP_WIDTH = 21;
    public const int LOGI_LED_BITMAP_HEIGHT = 6;
    public const int LOGI_LED_BITMAP_BYTES_PER_KEY = 4;

    public const int LOGI_LED_BITMAP_SIZE = LOGI_LED_BITMAP_WIDTH * LOGI_LED_BITMAP_HEIGHT * LOGI_LED_BITMAP_BYTES_PER_KEY;
    public const int LOGI_LED_DURATION_INFINITE = 0;

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedInit")]
    public static extern bool Init();

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedInitWithName", CharSet = CharSet.Unicode)]
    public static extern bool Init(string name);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSetTargetDevice")]
    public static extern bool SetTargetDevice(int targetDevice);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedGetSdkVersion")]
    public static extern bool GetSdkVersion(ref int majorNum, ref int minorNum, ref int buildNum);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSaveCurrentLighting")]
    public static extern bool SaveCurrentLighting();

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSetLighting")]
    public static extern bool SetLighting(int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedRestoreLighting")]
    public static extern bool RestoreLighting();

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedFlashLighting")]
    public static extern bool FlashLighting(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedPulseLighting")]
    public static extern bool PulseLighting(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedStopEffects")]
    public static extern bool StopEffects();

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedExcludeKeysFromBitmap")]
    public static extern bool ExcludeKeysFromBitmap(KeyNames[] keyList, int listCount);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSetLightingFromBitmap")]
    public static extern bool SetLightingFromBitmap(byte[] bitmap);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSetLightingForKeyWithScanCode")]
    public static extern bool SetLightingForKeyWithScanCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSetLightingForKeyWithHidCode")]
    public static extern bool SetLightingForKeyWithHidCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSetLightingForKeyWithQuartzCode")]
    public static extern bool SetLightingForKeyWithQuartzCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSetLightingForKeyWithKeyName")]
    public static extern bool SetLightingForKeyWithKeyName(KeyNames keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSaveLightingForKey")]
    public static extern bool SaveLightingForKey(KeyNames keyName);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedRestoreLightingForKey")]
    public static extern bool RestoreLightingForKey(KeyNames keyName);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedFlashSingleKey")]
    public static extern bool FlashSingleKey(KeyNames keyName, int redPercentage, int greenPercentage, int bluePercentage, int msDuration, int msInterval);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedPulseSingleKey")]
    public static extern bool PulseSingleKey(KeyNames keyName, int startRedPercentage, int startGreenPercentage, int startBluePercentage, int finishRedPercentage, int finishGreenPercentage, int finishBluePercentage, int msDuration, bool isInfinite);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedStopEffectsOnKey")]
    public static extern bool StopEffectsOnKey(KeyNames keyName);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedSetLightingForTargetZone")]
    public static extern bool SetLightingForTargetZone(DeviceTypes deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage);

    [DllImport("LogitechLedEnginesWrapper", CallingConvention = CallingConvention.Cdecl, EntryPoint = "LogiLedShutdown")]
    public static extern void Shutdown();
}
