using BepInEx.Logging;
using JetBrains.Annotations;
using System;
using System.IO;
using System.Text;

namespace OdinPlus.Common
{
  /// <summary>
  /// Source: https://github.com/Digitalroot/digitalroot-valheim-mods/blob/main/Digitalroot.Valheim.Common/Log.cs
  /// Permission granted to OdinPlus under 
  /// License: "GNU Affero General Public License v3.0"
  /// License ref: https://github.com/Digitalroot/digitalroot-valheim-mods/blob/main/LICENSE
  /// </summary>
  public sealed class Log : IDisposable
  {
    private readonly ManualLogSource _loggerRef;
    
    private static Log Instance { get; } = new Log();

    static Log()
    {
    }

    private Log()
    {
      _loggerRef = Logger.CreateLogSource(Main.Name);

      if (_traceFileInfo.Exists)
      {
        _traceFileInfo.Delete();
        _traceFileInfo.Refresh();
      }

      _timer.AutoReset = true;
      _timer.Enabled = true;
      _timer.Elapsed += TimerElapsed;

      if (TraceEnabled)
      {
        StartTrace();
      }
    }

    public void Dispose()
    {
      _timer.Stop();
      _timer.Dispose();
      _streamWriter.Flush();
      _fileStream.Flush();
      _streamWriter.Close();
      _fileStream.Close();
      _streamWriter.Dispose();
      _fileStream.Dispose();
    }

    #region Trace

    public static bool TraceEnabled { get; private set; }

    private static readonly object FileLock = new object();
    private readonly FileInfo _traceFileInfo = new FileInfo(Path.Combine(BepInEx.Paths.BepInExRootPath, $"{Main.Name}.Trace.log"));
    private FileStream _fileStream;
    private StreamWriter _streamWriter;
    private readonly System.Timers.Timer _timer = new System.Timers.Timer(3000);

    private void StartTrace()
    {
      if (_fileStream == null) _fileStream = new FileStream(_traceFileInfo.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read, 4096, FileOptions.WriteThrough);
      if (_streamWriter == null) _streamWriter = new StreamWriter(_fileStream, Encoding.ASCII, 4096);

      _timer.Start();
      _loggerRef.LogEvent += OnLogEvent;
    }

    private void StopTrace()
    {
      _timer.Stop();
      _loggerRef.LogEvent -= OnLogEvent;
    }

    private void OnLogEvent(object sender, LogEventArgs e)
    {
      if (e.Source.SourceName == Main.Name)
      {
        lock (FileLock)
        {
          if (e.Data is string)
          {
            _streamWriter.WriteLine($"[{e.Level,-7}:{e.Source.SourceName,10}] {e.Data}");
          }
          else
          {
            _streamWriter.WriteLine($"[{e.Level,-7}:{e.Source.SourceName,10}] {JsonSerializationProvider.ToJson(e.Data)}");
          }
        }
      }
    }

    private void TimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      lock (FileLock)
      {
        _streamWriter.Flush();
      }
    }

    [UsedImplicitly]
    public static void EnableTrace()
    {
      Instance.StartTrace();
      TraceEnabled = true;
    }

    [UsedImplicitly]
    public static void DisableTrace()
    {
      Instance.StopTrace();
      TraceEnabled = false;
    }

    #endregion

    #region Logging

    [UsedImplicitly]
    public static void Fatal(Exception e, int i = 1)
    {
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

    #endregion
  }
}
