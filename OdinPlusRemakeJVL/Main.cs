using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.ConsoleCommands;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL
{
  [BepInPlugin(Guid, Name, Version)]
  [BepInIncompatibility("buzz.valheim.OdinPlus")]
  public class Main : BaseUnityPlugin
  {
    public const string Version = "1.0.0";
    public const string Name = "OdinPlusRemakeJVL";
    public const string Guid = "digitalroot.valheim.mods.odinplusremakejvl";
    public const string Namespace = "OdinPlusRemakeJVL";
    public static Main Instance;
    public static ConfigEntry<int> NexusId;

    private Harmony _harmony;
    private readonly List<IAbstractManager> _managers = new List<IAbstractManager>();
    private readonly Stopwatch _stopwatch = new Stopwatch();

    public static ConfigEntry<Vector3> ConfigEntryOdinPosition;
    public static ConfigEntry<bool> ConfigEntryForceOdinPosition;

    public Main()
    {
      Log.EnableTrace();
      Instance = this;
    }

    [UsedImplicitly]
    private void Awake()
    {
      try
      {
        StartStopwatch();
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        NexusId = Config.Bind("General", "NexusID", 000, "Nexus mod ID for updates");
        ConfigEntryOdinPosition = Config.Bind("2Server set only", "Odin's Position", Vector3.zero);
        ConfigEntryForceOdinPosition = Config.Bind("2Server set only", "Force Odin's Position", false);

        _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Guid);

        ConsoleCommandManager.Instance.AddConsoleCommand(new ForceLoadCommand());

        if (!_managers.Contains(HealthManager.Instance)) _managers.Add(HealthManager.Instance);
        if (!_managers.Contains(ConsoleCommandManager.Instance)) _managers.Add(ConsoleCommandManager.Instance);
        if (!_managers.Contains(SpriteManager.Instance)) _managers.Add(SpriteManager.Instance);
        if (!_managers.Contains(StatusEffectsManager.Instance)) _managers.Add(StatusEffectsManager.Instance);
        if (!_managers.Contains(FxAssetManager.Instance)) _managers.Add(FxAssetManager.Instance);
        if (!_managers.Contains(ItemManager.Instance)) _managers.Add(ItemManager.Instance);

        foreach (var manager in _managers)
        {
          manager.Initialize();
        }

        foreach (var manager in _managers)
        {
          manager.PostInitialize();
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
      finally
      {
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      _harmony?.UnpatchSelf();
    }

    #region Events

    #region ZNetSceneReady

    public event EventHandler ZNetSceneReady;

    public void OnZNetSceneReady()
    {
      StartStopwatch();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug("Calling ZNetSceneReady Subscribers");
      try
      {
        // Log.Trace($"[{GetType().Name}] Instance != null: {Instance != null}");
        // Log.Trace($"[{GetType().Name}] Instance?.ZNetSceneReady != null: {Instance?.ZNetSceneReady != null}");
        // Log.Trace($"[{GetType().Name}] Instance?.ZNetSceneReady?.GetInvocationList().ToList() != null: {Instance?.ZNetSceneReady?.GetInvocationList().ToList() != null}");

        foreach (Delegate @delegate in Instance?.ZNetSceneReady?.GetInvocationList()?.ToList())
        {
          try
          {
            Log.Trace($"[{GetType().Name}] {@delegate.Target}.{@delegate.Method.Name}()");
            EventHandler subscriber = (EventHandler) @delegate;
            subscriber.Invoke(this, EventArgs.Empty);
          }
          catch (Exception e)
          {
            HandleDelegateError(@delegate.Method, e);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
      finally
      {
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    #endregion

    #region ZNetReady

    public event EventHandler ZNetReady;

    public void OnZNetReady()
    {
      StartStopwatch();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug("Calling ZNetReady Subscribers");
      try
      {
        // Log.Trace($"[{GetType().Name}] Instance != null: {Instance != null}");
        // Log.Trace($"[{GetType().Name}] Instance?.ZNetSceneReady != null: {Instance?.ZNetSceneReady != null}");
        // Log.Trace($"[{GetType().Name}] Instance?.ZNetSceneReady?.GetInvocationList().ToList() != null: {Instance?.ZNetSceneReady?.GetInvocationList().ToList() != null}");

        foreach (Delegate @delegate in Instance?.ZNetReady?.GetInvocationList()?.ToList())
        {
          try
          {
            Log.Trace($"[{GetType().Name}] {@delegate.Target}.{@delegate.Method.Name}()");
            EventHandler subscriber = (EventHandler)@delegate;
            subscriber.Invoke(this, EventArgs.Empty);
          }
          catch (Exception e)
          {
            HandleDelegateError(@delegate.Method, e);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
      finally
      {
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    #endregion

    private static void HandleDelegateError(MethodInfo method, Exception exception)
    {
      Log.Error($"[{method}] {exception.Message}");
    }

    #endregion

    #region Performance

    [Conditional("DEBUG")]
    private void StartStopwatch()
    {
      _stopwatch.Restart();
    }

    [Conditional("DEBUG")]
    private void StopStopwatch()
    {
      _stopwatch.Stop();
    }

    [Conditional("DEBUG")]
    private void ReportLoadTime(string method)
    {
      Log.Debug($"[{Name}] {method} Execution Time: {_stopwatch.Elapsed.Minutes}m, {_stopwatch.Elapsed.Seconds}s, {_stopwatch.Elapsed.Milliseconds}ms");
    }

    #endregion

    private void RegisterObjects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }
  }
}
