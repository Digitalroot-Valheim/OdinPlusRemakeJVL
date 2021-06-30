using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OdinPlusRemakeJVL.Common.Names;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace OdinPlusRemakeJVL.Managers
{
  internal class SpriteManager : AbstractManager<SpriteManager>, IOnVanillaItemsAvailable, IOnPrefabsRegistered
  {
    private readonly Dictionary<string, Sprite> _spriteDictionary = new Dictionary<string, Sprite>();

    public override bool HasDependencyError()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      return false;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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
        Log.Error(e);
        healthCheckStatus.HealthStatus = HealthStatus.Failed;
        healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: {e.Message}";
        return healthCheckStatus;
      }
    }

    public void OnVanillaItemsAvailable()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      AddSpriteFromPrefab(PrefabNames.OdinCredit, ItemDropNames.HelmetOdin);
      AddSpriteFromPrefab(ItemDropNames.ScrollTroll, ItemDropNames.TrophyFrostTroll);
      AddSpriteFromPrefab(ItemDropNames.ScrollWolf, ItemDropNames.TrophyWolf);
      AddSpriteFromPrefab(ItemDropNames.Coins);
    }

    private void AddSpriteFromPrefab(string name, string prefabName)
    {
      try
      {
        Log.Trace($"{nameof(SpriteManager)}.{MethodBase.GetCurrentMethod().Name}({name},{prefabName})");
        if (!_spriteDictionary.ContainsKey(name))
        {
          _spriteDictionary.Add(name, LoadPrefabIcon(prefabName));
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void AddSpriteFromPrefab(string prefabName)
    {
      try
      {
        Log.Trace($"{nameof(SpriteManager)}.{MethodBase.GetCurrentMethod().Name}({prefabName})");
        if (!_spriteDictionary.ContainsKey(prefabName))
        {
          _spriteDictionary.Add(prefabName, LoadPrefabIcon(prefabName));
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void AddSpriteFromResource(string prefabName)
    {
      try
      {
        Log.Trace($"{nameof(SpriteManager)}.{MethodBase.GetCurrentMethod().Name}({prefabName})");
        if (!_spriteDictionary.ContainsKey(prefabName))
        {
          _spriteDictionary.Add(prefabName, LoadResourceIcon(prefabName));
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private static byte[] GetResource(Assembly asm, string resourceName)
    {
      var manifestResourceStream = asm.GetManifestResourceStream(resourceName);
      Debug.Assert(manifestResourceStream != null, nameof(manifestResourceStream) + " != null");
      var array = new byte[manifestResourceStream.Length];
      manifestResourceStream.Read(array, 0, (int) manifestResourceStream.Length);
      return array;
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
      return LoadSpriteFromTexture(LoadTextureRaw(GetResource(Assembly.GetCallingAssembly(), "OdinPlusRemakeJVL.Resources." + name + ".png")));
    }

    private static Sprite LoadSpriteFromTexture(Texture2D spriteTexture, float pixelsPerUnit = 100f)
    {
      return spriteTexture ? Sprite.Create(spriteTexture, new Rect(0f, 0f, spriteTexture.width, spriteTexture.height), new Vector2(0f, 0f), pixelsPerUnit) : null;
    }

    private static Texture2D LoadTextureRaw(byte[] file)
    {
      if (file.Any())
      {
        Texture2D texture2D = new Texture2D(2, 2);
        bool flag2 = texture2D.LoadImage(file);
        if (flag2)
        {
          return texture2D;
        }
      }

      return null;
    }
  }
}
