using System;
using System.Collections;
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
    public static void Fatal(Exception e, int i = 1)
    {
      Fatal("FatalError");
      Fatal($"Message: {e.Message}");
      Fatal($"StackTrace: {e.StackTrace}");
      Fatal($"Source: {e.Source}");
      if (e.Data.Count > 0)
      {
        foreach (var key in e.Data.Keys)
        {
          Fatal($"key: {key}, value: {e.Data[key]}");
        }
      }

      if (e.InnerException != null)
      {
        Fatal($"--- InnerException [{i}][Start] ---");
        Fatal(e.InnerException, ++i);
      }
    }

    [UsedImplicitly]
    public static void Fatal(object value)
    {
      Instance._loggerRef.LogFatal(value);
    }

    [UsedImplicitly]
    public static void Error(Exception e, int i = 1)
    {
      Error("ERROR");
      Error($"Message: {e.Message}");
      Error($"StackTrace: {e.StackTrace}");
      Error($"Source: {e.Source}");
      if (e.Data.Count > 0)
      {
        foreach (var key in e.Data.Keys)
        {
          Error($"key: {key}, value: {e.Data[key]}");
        }
      }

      if (e.InnerException != null)
      {
        Error($"--- InnerException [{i}][Start] ---");
        Error(e.InnerException, ++i);
      }
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
