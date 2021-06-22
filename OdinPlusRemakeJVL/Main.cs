using System;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Managers;
using OdinPlusRemakeJVL.Items;
using System.Reflection;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.ConsoleCommands;
using OdinPlusRemakeJVL.Managers;

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

    private Harmony _harmony;
    private readonly List<IAbstractManager> _managers = new List<IAbstractManager>();
    public static Main Instance;
    public static ConfigEntry<int> NexusId;
    
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
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        NexusId = Config.Bind("General", "NexusID", 000, "Nexus mod ID for updates");

        _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Guid);

        ItemManager.OnVanillaItemsAvailable += AddClonedItems; // ToDo move this to an item manager.

        ConsoleCommandManager.Instance.AddConsoleCommand(new ForceLoadCommand());

        if (!_managers.Contains(HealthManager.Instance)) _managers.Add(HealthManager.Instance);
        if (!_managers.Contains(ConsoleCommandManager.Instance)) _managers.Add(ConsoleCommandManager.Instance);
        if (!_managers.Contains(SpriteManager.Instance)) _managers.Add(SpriteManager.Instance);
        if (!_managers.Contains(StatusEffectsManager.Instance)) _managers.Add(StatusEffectsManager.Instance);
        // if (!_managers.Contains(FxAssetManager.Instance)) _managers.Add(FxAssetManager.Instance);

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
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      _harmony?.UnpatchSelf();
    }

    private void AddClonedItems()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        OdinLegacy.Create();
      }
      catch (Exception e)
      {
        Log.Error(e);
        throw;
      }
      finally
      {
        ItemManager.OnVanillaItemsAvailable -= AddClonedItems;
      }
    }

    private void RegisterObjects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }
  }
}
