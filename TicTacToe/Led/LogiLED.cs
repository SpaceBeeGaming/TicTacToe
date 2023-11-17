using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace TicTacToe.Led;

internal static partial class LogiLED
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

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedInit")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool Init();

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedInitWithName", StringMarshalling = StringMarshalling.Utf16)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool Init(string name);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSetTargetDevice")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetTargetDevice(int targetDevice);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedGetSdkVersion")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool GetSdkVersion(ref int majorNum, ref int minorNum, ref int buildNum);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSaveCurrentLighting")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SaveCurrentLighting();

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSetLighting")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetLighting(int redPercentage, int greenPercentage, int bluePercentage);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedRestoreLighting")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RestoreLighting();

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedFlashLighting")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool FlashLighting(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedPulseLighting")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool PulseLighting(int redPercentage, int greenPercentage, int bluePercentage, int milliSecondsDuration, int milliSecondsInterval);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedStopEffects")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool StopEffects();

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedExcludeKeysFromBitmap")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ExcludeKeysFromBitmap(in KeyNames[] keyList, int listCount);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSetLightingFromBitmap")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetLightingFromBitmap(in byte[] bitmap);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSetLightingForKeyWithScanCode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetLightingForKeyWithScanCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSetLightingForKeyWithHidCode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetLightingForKeyWithHidCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSetLightingForKeyWithQuartzCode")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetLightingForKeyWithQuartzCode(int keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSetLightingForKeyWithKeyName")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetLightingForKeyWithKeyName(KeyNames keyCode, int redPercentage, int greenPercentage, int bluePercentage);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSaveLightingForKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SaveLightingForKey(KeyNames keyName);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedRestoreLightingForKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RestoreLightingForKey(KeyNames keyName);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedFlashSingleKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool FlashSingleKey(KeyNames keyName, int redPercentage, int greenPercentage, int bluePercentage, int msDuration, int msInterval);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedPulseSingleKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool PulseSingleKey(KeyNames keyName, int startRedPercentage, int startGreenPercentage, int startBluePercentage, int finishRedPercentage, int finishGreenPercentage, int finishBluePercentage, int msDuration, [MarshalAs(UnmanagedType.Bool)] bool isInfinite);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedStopEffectsOnKey")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool StopEffectsOnKey(KeyNames keyName);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedSetLightingForTargetZone")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool SetLightingForTargetZone(DeviceTypes deviceType, int zone, int redPercentage, int greenPercentage, int bluePercentage);

    [LibraryImport("LogitechLedEnginesWrapper", EntryPoint = "LogiLedShutdown")]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    public static partial void Shutdown();

    // Custom Overloads.
    public static bool SetLightingForKeyWithKeyName(KeyNames keyCode, Color color) =>
        SetLightingForKeyWithKeyName(keyCode, (int)Math.Round(color.R / 255.0 * 100), (int)Math.Round(color.G / 255.0 * 100), (int)Math.Round(color.B / 255.0 * 100));

    public static void FlashSingleKey(KeyNames keyName, Color color, int msDuration, int msInterval) =>
        FlashSingleKey(keyName, (int)Math.Round(color.R / 255.0 * 100), (int)Math.Round(color.G / 255.0 * 100), (int)Math.Round(color.B / 255.0 * 100), msDuration, msInterval);
}