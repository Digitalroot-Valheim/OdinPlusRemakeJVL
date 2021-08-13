using BepInEx;
using BepInEx.Configuration;
using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Utils;
using OdinPlusJVL.Common.EventArgs;
using OdinPlusJVL.ConsoleCommands;
using OdinPlusJVL.Extensions;
using OdinPlusJVL.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL
{
  [BepInPlugin(Guid, Name, Version)]
  [BepInDependency(Jotunn.Main.ModGuid)]
  [BepInIncompatibility("buzz.valheim.OdinPlus")]
  [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
  public class Main : BaseUnityPlugin, ITraceableLogging
  {
    #region Plug-in

    public const string Version = "1.0.0";
    public const string Name = "OdinPlusJVL";

    // ReSharper disable once MemberCanBePrivate.Global
    public const string Guid = "digitalroot.valheim.mods.odinplusjvl";
    public const string Namespace = Name;
    public static Main Instance;
    private Harmony _harmony;

    #endregion

    #region Config

    // ReSharper disable once MemberCanBePrivate.Global
    public static ConfigEntry<int> NexusId;
    internal static ConfigEntry<Vector3> ConfigEntryOdinPosition;

    // ReSharper disable once MemberCanBePrivate.Global
    internal static ConfigEntry<bool> ConfigEntryForceOdinPosition;

    #endregion

    private List<IInitializeable> _initializeables;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    internal static GameObject RootObject;
    public static List<string> traderNameList = new List<string>();
    public static int Credits;

    public Main()
    {
      Instance = this;
#if DEBUG
      EnableTrace = true;
      Log.RegisterSource(this);
#else
      EnableTrace = false;
#endif
    }

    [UsedImplicitly]
    private void Awake()
    {
      try
      {
        StartStopwatch();
        Log.Trace(this, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Config.SaveOnConfigSet = true;
        NexusId = Config.Bind("Config", "NexusID", 000, new ConfigDescription("Nexus mod ID for updates", null, new ConfigurationManagerAttributes { Browsable = false }));
        ConfigEntryOdinPosition = Config.Bind("Config", "Odins Position", Vector3.zero, new ConfigDescription("Odin's Position", null, new ConfigurationManagerAttributes { Browsable = false, IsAdminOnly = true }));
        ConfigEntryForceOdinPosition = Config.Bind("Config", "Force Odins Position", false, new ConfigDescription("Force Odin's Position", null, new ConfigurationManagerAttributes { Browsable = false, IsAdminOnly = true }));
        _harmony = Harmony.CreateAndPatchAll(typeof(Main).Assembly, Guid);

        RootObject = new GameObject(Name)
        {
          transform =
          {
            position = Vector3.zero, rotation = Quaternion.identity
          }
        };
        DontDestroyOnLoad(RootObject);

        Jotunn.Managers.ItemManager.OnVanillaItemsAvailable += OnVanillaItemsAvailable;
        Jotunn.Managers.PrefabManager.OnPrefabsRegistered += OnPrefabsRegistered;

        _initializeables = new List<IInitializeable>()
        {
          HealthManager.Instance
          , ConsoleCommandManager.Instance
          , SpriteManager.Instance
          , StatusEffectsManager.Instance
          , FxAssetManager.Instance
          , ItemDropManager.Instance
          , PrefabManager.Instance
          , LocationManager.Instance
          , OdinsCampManager.Instance
        };

        foreach (var manager in _initializeables)
        {
          manager.Initialize();
        }

        foreach (var manager in _initializeables)
        {
          manager.PostInitialize();
        }

        AddCommands();
      }
      catch (Exception e)
      {
        Log.Error(this, e);
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
      Log.Debug(this, $"[{GetType().Name}] Calling Managers with IDestroyable");
      try
      {
        foreach (var destroyable in _initializeables.Select(manager => manager as IDestroyable))
        {
          try
          {
            destroyable?.OnDestroy();
          }
          catch (Exception e)
          {
            Log.Error(this, e);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(this, e);
      }
      finally
      {
        _harmony?.UnpatchSelf();
      }
    }

    #region Refactor

    public static void AddCredits(int val, Transform m_head)
    {
      AddCredits(val);
      Player.m_localPlayer.m_skillLevelupEffects.Create(m_head.position, m_head.rotation, m_head);
    }

    public static bool RemoveCredits(int s)
    {
      if (Credits - s < 0)
      {
        return false;
      }

      Credits -= s;
      return true;
    }

    public static void AddCredits(int val)
    {
      Credits += val;
    }

    public static void AddCredits(int val, bool _notice)
    {
      AddCredits(val);
      if (_notice)
      {
        string n = String.Format("Odin Credits added : <color=lightblue><b>[{0}]</b></color>", Credits);
        MessageHud.instance.ShowBiomeFoundMsg(n, true); //trans
      }
    }

    #endregion

    #region Events

    #region ZNetScene

    public event EventHandler ZNetSceneReady;

    public void OnZNetSceneReady(ref ZNetScene zNetScene)
    {
      StartStopwatch();
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug(Instance, $"[{GetType().Name}] Calling ZNetSceneReady Subscribers");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnZNetSceneReady");
        foreach (var onZNetSceneReady in _initializeables.Select(manager => manager as IOnZNetSceneReady))
        {
          try
          {
            onZNetSceneReady?.OnZNetSceneReady(zNetScene);
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }

        Log.Debug(Instance, $"[{GetType().Name}] Calling ZNetSceneReady Event Subscribers");
        if (Instance.ZNetSceneReady != null && Instance.ZNetSceneReady.GetInvocationList().Length > 0)
        {
          foreach (Delegate @delegate in Instance?.ZNetSceneReady?.GetInvocationList()?.ToList())
          {
            try
            {
              Log.Trace(Instance, $"[{GetType().Name}] {@delegate.Method.DeclaringType?.Name}.{@delegate.Method.Name}()");
              EventHandler subscriber = (EventHandler)@delegate;
              subscriber.Invoke(this, new OnZNetSceneReadyEventArgs(zNetScene));
            }
            catch (Exception e)
            {
              HandleDelegateError(@delegate.Method, e);
            }
          }
        }

        //_NetSceneRoot.keeper_prefab(Clone)
        while (zNetScene?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)") != null)
        {
          var keeper = zNetScene?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)");
          if (keeper != null)
          {
            Log.Error(Instance, $"Found {keeper?.name}");
            DestroyImmediate(keeper);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
      finally
      {
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    public void OnZNetSceneShutdown(ref ZNetScene zNetScene)
    {
      StartStopwatch();
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IDestroyable");
        foreach (var destroyable in _initializeables.Select(manager => manager as IDestroyable))
        {
          try
          {
            destroyable?.OnDestroy();
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }

        //_NetSceneRoot.keeper_prefab(Clone)
        while (zNetScene?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)") != null)
        {
          var keeper = zNetScene?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)");
          if (keeper != null)
          {
            Log.Error(Instance, $"Found {keeper?.name}");
            DestroyImmediate(keeper);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
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

    public void OnZNetReady(ref ZNet zNet)
    {
      StartStopwatch();
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug(Instance, $"[{GetType().Name}] Calling ZNetReady Subscribers");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnZNetReady");
        foreach (var onZNetReady in _initializeables.Select(manager => manager as IOnZNetReady))
        {
          try
          {
            onZNetReady?.OnZNetReady(zNet);
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }

        Log.Debug(Instance, $"[{GetType().Name}] Calling ZNetReady Event Subscribers");
        if (Instance.ZNetReady != null && Instance.ZNetReady.GetInvocationList().Length > 0)
        {
          foreach (Delegate @delegate in Instance?.ZNetReady?.GetInvocationList()?.ToList())
          {
            try
            {
              Log.Trace(Instance, $"[{GetType().Name}] {@delegate.Target}.{@delegate.Method.Name}()");
              EventHandler subscriber = (EventHandler)@delegate;
              subscriber.Invoke(this, new OnZNetReadyEventArgs(zNet));
            }
            catch (Exception e)
            {
              HandleDelegateError(@delegate.Method, e);
            }
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
      finally
      {
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    #endregion

    #region ZoneSystemLoaded

    public event EventHandler ZoneSystemLoaded;

    public void OnZoneSystemLoaded()
    {
      StartStopwatch();
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      RootObject.SetActive(true);

      Log.Debug(Instance, $"[{GetType().Name}] Calling OnZoneSystemLoaded Subscribers");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnZoneSystemLoaded");
        foreach (var onZoneSystemLoaded in _initializeables.Select(manager => manager as IOnZoneSystemLoaded))
        {
          try
          {
            onZoneSystemLoaded?.OnZoneSystemLoaded();
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }

        Log.Debug(Instance, $"[{GetType().Name}] Calling ZoneSystemLoaded Event Subscribers");
        if (Instance.ZoneSystemLoaded != null && Instance.ZoneSystemLoaded.GetInvocationList().Length > 0)
        {
          foreach (Delegate @delegate in Instance.ZoneSystemLoaded.GetInvocationList()?.ToList())
          {
            try
            {
              Log.Trace(Instance, $"[{GetType().Name}] {@delegate.Target}.{@delegate.Method.Name}()");
              EventHandler subscriber = (EventHandler)@delegate;
              subscriber.Invoke(this, EventArgs.Empty);
            }
            catch (Exception e)
            {
              HandleDelegateError(@delegate.Method, e);
            }
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
      finally
      {
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    #endregion

    #region SpawnedPlayer

    public event EventHandler SpawnedPlayer;

    public void OnSpawnedPlayer(Vector3 spawnPoint)
    {
      StartStopwatch();
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug(Instance, $"[{GetType().Name}] Calling SpawnedPlayer Subscribers");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnSpawnedPlayer");
        foreach (var onSpawnedPlayer in _initializeables.Select(manager => manager as IOnSpawnedPlayer))
        {
          try
          {
            onSpawnedPlayer?.OnSpawnedPlayer(spawnPoint);
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }

        Log.Debug(Instance, $"[{GetType().Name}] Calling SpawnedPlayer Event Subscribers");
        if (Instance.SpawnedPlayer != null && Instance.SpawnedPlayer.GetInvocationList().Length > 0)
        {
          foreach (Delegate @delegate in Instance?.SpawnedPlayer?.GetInvocationList()?.ToList())
          {
            try
            {
              Log.Trace(Instance, $"[{GetType().Name}] {@delegate.Method.DeclaringType?.Name}.{@delegate.Method.Name}()");
              EventHandler subscriber = (EventHandler)@delegate;
              subscriber.Invoke(this, new OnSpawnedPlayerEventArgs(spawnPoint));
            }
            catch (Exception e)
            {
              HandleDelegateError(@delegate.Method, e);
            }
          }
        }

        //_NetSceneRoot.keeper_prefab(Clone)
        while (ZNetScene.instance?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)") != null)
        {
          var keeper = ZNetScene.instance?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)");
          if (keeper != null)
          {
            Log.Error(Instance, $"Found {keeper?.name}");
            DestroyImmediate(keeper);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
      finally
      {
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    #endregion

    #region Jotunn Events

    private void OnPrefabsRegistered()
    {
      StartStopwatch();
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnPrefabsRegistered");
        foreach (var onPrefabsRegistered in _initializeables.Select(manager => manager as IOnPrefabsRegistered))
        {
          try
          {
            onPrefabsRegistered?.OnPrefabsRegistered();
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
      finally
      {
        Jotunn.Managers.PrefabManager.OnPrefabsRegistered -= OnPrefabsRegistered;
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    private void OnVanillaItemsAvailable()
    {
      StartStopwatch();
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnVanillaItemsAvailable");
        foreach (var onVanillaItemsAvailable in _initializeables.Select(manager => manager as IOnVanillaItemsAvailable))
        {
          try
          {
            onVanillaItemsAvailable?.OnVanillaItemsAvailable();
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
      finally
      {
        Jotunn.Managers.ItemManager.OnVanillaItemsAvailable -= OnVanillaItemsAvailable;
        StopStopwatch();
        ReportLoadTime(MethodBase.GetCurrentMethod().Name);
      }
    }

    #endregion

    private static void HandleDelegateError(MethodInfo method, Exception exception)
    {
      Log.Error(Instance, $"[{method}] {exception.Message}");
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
      Log.Debug(Instance, $"[{Name}] {method} Execution Time: {_stopwatch.Elapsed.Minutes}m, {_stopwatch.Elapsed.Seconds}s, {_stopwatch.Elapsed.Milliseconds}ms");
    }

    #endregion

    private void AddCommands()
    {
      ConsoleCommandManager.Instance.AddConsoleCommand(new ForceLoadCommand());
    }

    private void RegisterObjects()
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    #region Implementation of ITraceableLogging

    /// <inheritdoc />
    public string Source => Namespace;

    /// <inheritdoc />
    public bool EnableTrace { get; }

    #endregion
  }
}
