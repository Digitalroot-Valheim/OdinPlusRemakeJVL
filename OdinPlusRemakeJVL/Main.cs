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
    private readonly Dictionary<string, Sprite> _odinMeadsIcons = new Dictionary<string, Sprite>();

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

    private void AddSprites()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      foreach (var meadName in OdinPlusMead.MeadsNames)
      {
        if (_odinMeadsIcons.ContainsKey(meadName)) continue;
        _odinMeadsIcons.Add(meadName, IconManager.LoadResourceIcon(meadName));
      }

      foreach (var odinPlusPetsStatusEffect in OdinPlusPetsStatusEffects.PetsStatusEffectsNames)
      {
        if (_odinMeadsIcons.ContainsKey(odinPlusPetsStatusEffect)) continue;
        _odinMeadsIcons.Add(odinPlusPetsStatusEffect, IconManager.LoadResourceIcon(odinPlusPetsStatusEffect));
      }

      if (_odinMeadsIcons.ContainsKey(Items.OdinPlusItem.OdinLegacy)) return;
      _odinMeadsIcons.Add(Items.OdinPlusItem.OdinLegacy, IconManager.LoadResourceIcon(Items.OdinPlusItem.OdinLegacy));
    }

    private void AddStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var statusEffect in StatusEffectsManager.CreateStatusEffects(StatusEffectsManager.MasterStatusEffectDataDictionary, _odinMeadsIcons)) 
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
      ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(StatusEffectsManager.GetSpeedStatusEffect(_odinMeadsIcons), fixReference: false));
      ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(StatusEffectsManager.GetSummonTrollPetStatusEffect(_odinMeadsIcons), fixReference: false));
      ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(StatusEffectsManager.GetSummonWolfPetStatusEffect(_odinMeadsIcons), fixReference: false));
    }

    private void RegisterObjects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }
  }
}
