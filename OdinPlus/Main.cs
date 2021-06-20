using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using OdinPlus.Common;
using OdinPlus.Managers;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlus
{
  [BepInPlugin(Guid, Name, Version)]
  [BepInIncompatibility("buzz.valheim.OdinPlus")]
  public class Main : BaseUnityPlugin
  {
    public const string Version = "0.4.0";
    public const string Name = "OdinPlusRemake";
    public const string Guid = "digitalroot.valheim.mods.odinplusremake";
    public const string Namespace = "OdinPlus";
    public static Main Instance;
    private Harmony _harmony;

    public static ConfigEntry<int> NexusId;
    public static ConfigEntry<KeyboardShortcut> KeyboardShortcutSecondInteractKey;
    public static ConfigEntry<string> ConfigEntryItemSellValue;
    public static ConfigEntry<Vector3> ConfigEntryOdinPosition;
    public static ConfigEntry<bool> ConfigEntryForceOdinPosition;
    public static bool Set_FOP = false; // ToDo: What is this for?
    public static int RaiseCost = 10;
    public static int RaiseFactor = 100;
    public static GameObject OdinPlusRoot;
    public static Action PostZoneAction;
    public static Action RegisterRpcAction;

    public Main()
    {
      Log.EnableTrace();
      Instance = this;
    }

    [UsedImplicitly]
    private void Awake_d()
    {
      
      Log.Trace($"{Main.Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      ConfigEntryItemSellValue = Config.Bind("Config", "ItemSellValue", "TrophyBlob:20;TrophyBoar:5;TrophyBonemass:50;TrophyDeathsquito:20;TrophyDeer:5;TrophyDragonQueen:50;TrophyDraugr:20;TrophyDraugrElite:30;TrophyDraugrFem:20;TrophyEikthyr:50;TrophyFenring:30;TrophyForestTroll:30;TrophyFrostTroll:20;TrophyGoblin:20;TrophyGoblinBrute:30;TrophyGoblinKing:50;TrophyGoblinShaman:20;TrophyGreydwarf:5;TrophyGreydwarfBrute:15;TrophyGreydwarfShaman:15;TrophyHatchling:20;TrophyLeech:15;TrophyLox:20;TrophyNeck:5;TrophySerpent:30;TrophySGolem:30;TrophySkeleton:10;TrophySkeletonPoison:30;TrophySurtling:20;TrophyTheElder:50;TrophyWolf:20;TrophyWraith:30;AncientSeed:5;BoneFragments:1;Chitin:5;WitheredBone:10;DragonEgg:40;GoblinTotem:20;OdinLegacy:20");
      NexusId = Config.Bind("General", "NexusID", 798, "Nexus mod ID for updates");
      KeyboardShortcutSecondInteractKey = Config.Bind("1Hotkeys", "Second Interact key", new KeyboardShortcut(KeyCode.G));
      ConfigEntryOdinPosition = Config.Bind("2Server set only", "Odin position", Vector3.zero);
      ConfigEntryForceOdinPosition = Config.Bind("2Server set only", "Force Odin Position", false);
      RegisterRpcAction = RegisterRpc;

      // _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());

      OdinPlusRoot = new GameObject("OdinPlus");
      // OdinPlusRoot.AddComponent<ResourceAssetManager>();
      OdinPlusRoot.AddComponent<OdinPlus>();
      OdinPlusRoot.AddComponent<DevTool>(); // Debug

      DontDestroyOnLoad(OdinPlusRoot);
      Log.Info("OdinPlus Loaded");
    }

    public static void RegisterRpc()
    {
      Log.Info("Starting reg rpc");
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      _harmony?.UnpatchSelf();
    }
  }
}
