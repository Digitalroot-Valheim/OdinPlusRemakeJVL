using System;
using BepInEx.Logging;
using JetBrains.Annotations;

namespace OdinPlus.Common
{
  /// <summary>
  /// Source: https://github.com/Digitalroot/digitalroot-valheim-mods/blob/main/Digitalroot.Valheim.Common/Log.cs
  /// Permission granted to OdinPlus under 
  /// License: "GNU Affero General Public License v3.0"
  /// License ref: https://github.com/Digitalroot/digitalroot-valheim-mods/blob/main/LICENSE
  /// </summary>
  public sealed class Log
  {
    static Log() { }

    private Log()
    {
      _loggerRef = Logger.CreateLogSource("OdinPlusRemake");
    }

    private readonly ManualLogSource _loggerRef;

    private static Log Instance { get; } = new Log();

    public static bool TraceEnabled { get; private set; }

    [UsedImplicitly]
    public static void EnableTrace()
    {
      TraceEnabled = true;
    }

    [UsedImplicitly]
    public static void DisableTrace()
    {
      TraceEnabled = false;
    }

    [UsedImplicitly]
    public static void Fatal(Exception e)
    {
      Fatal("ERROR");
      Fatal($"Message: {e.Message}");
      Fatal($"StackTrace: {e.StackTrace}");
      Fatal($"Source: {e.Source}");
      if (e.InnerException != null)
      {
        Fatal("--- InnerException ---");
        Fatal(e.InnerException);
      }
    }

    [UsedImplicitly]
    public static void Fatal(object value)
    {
      Instance._loggerRef.LogFatal(value);
    }

    [UsedImplicitly]
    public static void Error(object value)
    {
      Instance._loggerRef.LogError(value);
    }

    [UsedImplicitly]
    public static void Warning(object value)
    {
      Instance._loggerRef.LogWarning(value);
    }

    [UsedImplicitly]
    public static void Message(object value)
    {
      Instance._loggerRef.LogMessage(value);
    }

    [UsedImplicitly]
    public static void Info(object value)
    {
      Instance._loggerRef.LogInfo(value);
    }

    [UsedImplicitly]
    public static void Debug(object value)
    {
      Instance._loggerRef.LogDebug(value);
    }

    [UsedImplicitly]
    public static void Trace(object value)
    {
      if (TraceEnabled)
      {
        Instance._loggerRef.Log(LogLevel.All, value);
      }
    }
  }
}
