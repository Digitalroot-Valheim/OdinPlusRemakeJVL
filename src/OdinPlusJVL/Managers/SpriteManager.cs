using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using Jotunn.Utils;
using OdinPlusJVL.Common.Names;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Managers
{
  internal class SpriteManager : AbstractManager<SpriteManager>, IOnVanillaItemsAvailable, IOnPrefabsRegistered
  {
    private readonly Dictionary<string, Sprite> _spriteDictionary = new Dictionary<string, Sprite>();

    public override bool HasDependencyError()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      return false;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = GetType().Name;
        if (!_spriteDictionary.Any())
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _spriteDictionary.Any(): false";
        }

        return healthCheckStatus;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        healthCheckStatus.HealthStatus = HealthStatus.Failed;
        healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: {e.Message}";
        return healthCheckStatus;
      }
    }

    public void OnVanillaItemsAvailable()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      foreach (var meadName in MeadNames.AllNames)
      {
        AddSpriteFromResource(meadName);
      }

      foreach (var odinPlusPetsStatusEffect in PetsStatusEffectNames.AllNames)
      {
        AddSpriteFromResource(odinPlusPetsStatusEffect);
      }

      AddSpriteFromResource(ItemDropNames.OdinLegacy);
    }

    public void OnPrefabsRegistered()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      AddSpriteFromPrefab(PrefabNames.OdinCredit, ItemDropNames.HelmetOdin);
      AddSpriteFromPrefab(ItemDropNames.ScrollTroll, ItemDropNames.TrophyFrostTroll);
      AddSpriteFromPrefab(ItemDropNames.ScrollWolf, ItemDropNames.TrophyWolf);
      AddSpriteFromPrefab(ItemDropNames.Coins);
    }

    private void AddSpriteFromPrefab(string name, string prefabName)
    {
      try
      {
        Log.Trace(Main.Instance, $"{nameof(SpriteManager)}.{MethodBase.GetCurrentMethod().Name}({name},{prefabName})");
        if (!_spriteDictionary.ContainsKey(name))
        {
          _spriteDictionary.Add(name, LoadPrefabIcon(prefabName));
        }
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void AddSpriteFromPrefab(string prefabName)
    {
      try
      {
        Log.Trace(Main.Instance, $"{nameof(SpriteManager)}.{MethodBase.GetCurrentMethod().Name}({prefabName})");
        if (!_spriteDictionary.ContainsKey(prefabName))
        {
          _spriteDictionary.Add(prefabName, LoadPrefabIcon(prefabName));
        }
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void AddSpriteFromResource(string prefabName)
    {
      try
      {
        Log.Trace(Main.Instance, $"{nameof(SpriteManager)}.{MethodBase.GetCurrentMethod().Name}({prefabName})");
        if (!_spriteDictionary.ContainsKey(prefabName))
        {
          _spriteDictionary.Add(prefabName, LoadResourceIcon(prefabName));
        }
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    public Sprite GetSprite(string name)
    {
      return _spriteDictionary.ContainsKey(name) ? _spriteDictionary[name] : null;
    }

    private static Sprite LoadPrefabIcon(string name)
    {
      return PrefabManager.Instance.GetPrefab(name).GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
    }

    private static Sprite LoadResourceIcon(string name)
    {
      return AssetUtils.LoadSprite($"{Main.Name}.Assets.{name}.png");
    }
  }
}
