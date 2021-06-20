using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using JetBrains.Annotations;
using Jotunn.Entities;
using Jotunn.Managers;
using OdinPlus.Common;
using OdinPlusRemakeJVL.Icons;
using OdinPlusRemakeJVL.Items;
using OdinPlusRemakeJVL.StatusEffects;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using OdinPlusItem = OdinPlus.Common.OdinPlusItem;

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
    public static Main Instance;

    public static ConfigEntry<int> NexusId;
    public readonly Dictionary<string, Sprite> OdinMeadsIcons = new Dictionary<string, Sprite>();

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

        ItemManager.OnVanillaItemsAvailable += AddClonedItems;
        AddSprites();
        AddStatusEffects();
        
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

    private static void AddClonedItems()
    {
      OdinLegacy.Create();
      ItemManager.OnVanillaItemsAvailable -= AddClonedItems;
    }

    private void AddSprites()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      foreach (var meadName in OdinPlusMead.MeadsNames)
      {
        if (OdinMeadsIcons.ContainsKey(meadName)) continue;
        OdinMeadsIcons.Add(meadName, IconManager.LoadResourceIcon(meadName));
      }

      foreach (var odinPlusPetsStatusEffect in OdinPlusPetsStatusEffects.PetsStatusEffectsNames)
      {
        if (OdinMeadsIcons.ContainsKey(odinPlusPetsStatusEffect)) continue;
        OdinMeadsIcons.Add(odinPlusPetsStatusEffect, IconManager.LoadResourceIcon(odinPlusPetsStatusEffect));
      }

      if (OdinMeadsIcons.ContainsKey(Items.OdinPlusItem.OdinLegacy)) return;
      OdinMeadsIcons.Add(Items.OdinPlusItem.OdinLegacy, IconManager.LoadResourceIcon(Items.OdinPlusItem.OdinLegacy));
    }

    private void AddStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var statusEffect in StatusEffectsManager.CreateStatusEffects(StatusEffectsManager.MasterStatusEffectDataDictionary, OdinMeadsIcons)) 
      {
        try
        {
          ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(statusEffect, fixReference: false));
        }
        catch (Exception e)
        {
          e.Data.Add(nameof(statusEffect), JsonSerializationProvider.ToJson(statusEffect));
          throw;
        }
        
      }
      ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(StatusEffectsManager.GetSpeedStatusEffect(OdinMeadsIcons), fixReference: false));
      ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(StatusEffectsManager.GetSummonTrollPetStatusEffect(OdinMeadsIcons), fixReference: false));
      ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(StatusEffectsManager.GetSummonWolfPetStatusEffect(OdinMeadsIcons), fixReference: false));
    }

    private void RegisterObjects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }
  }
}
