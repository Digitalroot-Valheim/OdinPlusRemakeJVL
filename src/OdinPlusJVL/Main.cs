using BepInEx;
using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Utils;
using OdinPlusJVL.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OdinPlusJVL
{
  [BepInPlugin(Guid, Name, Version)]
  [BepInDependency(Jotunn.Main.ModGuid)]
  [BepInIncompatibility("buzz.valheim.OdinPlus")]
  [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Minor)]
  public partial class Main : BaseUnityPlugin, ITraceableLogging
  {
    public Main()
    {
      Instance = this;
#if DEBUG
      EnableTrace = true;
      Log.RegisterSource(Instance);
#else
      EnableTrace = false;
#endif
    }

    [UsedImplicitly]
    public void Awake()
    {
      try
      {
        Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        InitializeConfigs();
        _harmony = Harmony.CreateAndPatchAll(typeof(Main).Assembly, Guid);

        InitializeRootObject();
        
        Jotunn.Managers.ItemManager.OnVanillaItemsAvailable += OnVanillaItemsAvailable;
        Jotunn.Managers.PrefabManager.OnPrefabsRegistered += OnPrefabsRegistered;

        _managersList = new List<IInitializeable>
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

        foreach (var manager in _managersList)
        {
          manager.Initialize();
        }

        foreach (var manager in _managersList)
        {
          manager.PostInitialize();
        }

        AddCommands();
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IDestroyable");
      try
      {
        foreach (var destroyable in _managersList.Select(manager => manager as IDestroyable))
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
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
      finally
      {
        _harmony?.UnpatchSelf();
      }
    }

    #region Implementation of ITraceableLogging

    /// <inheritdoc />
    public string Source => Namespace;

    /// <inheritdoc />
    public bool EnableTrace { get; }

    #endregion

    #region Jotunn Events

    private void OnPrefabsRegistered()
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnPrefabsRegistered");
        foreach (var onPrefabsRegistered in _managersList.Select(manager => manager as IOnPrefabsRegistered))
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
      }
    }

    private void OnVanillaItemsAvailable()
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnVanillaItemsAvailable");
        foreach (var onVanillaItemsAvailable in _managersList.Select(manager => manager as IOnVanillaItemsAvailable))
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
      }
    }

    #endregion
  }
}
