using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace OdinPlusRemakeJVL.Managers
{
  internal class SpriteManager : AbstractManager<SpriteManager>
  {
    private readonly Dictionary<string, Sprite> _spriteDictionary = new Dictionary<string, Sprite>();

    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;
        AddSprites();
        return true;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return false;
      }
    }

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

    public Sprite GetSprite(string name)
    {
      return _spriteDictionary.ContainsKey(name) ? _spriteDictionary[name] : null;
    }

    private void AddSprites()
    {
      Log.Trace($"{nameof(SpriteManager)}.{MethodBase.GetCurrentMethod().Name}()");

      foreach (var meadName in OdinPlusMead.MeadsNames)
      {
        if (_spriteDictionary.ContainsKey(meadName)) continue;
        _spriteDictionary.Add(meadName, LoadResourceIcon(meadName));
      }

      foreach (var odinPlusPetsStatusEffect in PetsStatusEffectNames.AllNames)
      {
        if (_spriteDictionary.ContainsKey(odinPlusPetsStatusEffect)) continue;
        _spriteDictionary.Add(odinPlusPetsStatusEffect, LoadResourceIcon(odinPlusPetsStatusEffect));
      }

      if (_spriteDictionary.ContainsKey(OdinPlusItem.OdinLegacy)) return;
      _spriteDictionary.Add(OdinPlusItem.OdinLegacy, LoadResourceIcon(OdinPlusItem.OdinLegacy));
    }

    private static Sprite LoadResourceIcon(string name)
    {
      return LoadSpriteFromTexture(LoadTextureRaw(GetResource(Assembly.GetCallingAssembly(), "OdinPlusRemakeJVL.Resources." + name + ".png")));
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

    private static byte[] GetResource(Assembly asm, string resourceName)
    {
      var manifestResourceStream = asm.GetManifestResourceStream(resourceName);
      Debug.Assert(manifestResourceStream != null, nameof(manifestResourceStream) + " != null");
      var array = new byte[manifestResourceStream.Length];
      manifestResourceStream.Read(array, 0, (int)manifestResourceStream.Length);
      return array;
    }

    private static Sprite LoadSpriteFromTexture(Texture2D spriteTexture, float pixelsPerUnit = 100f)
    {
      return spriteTexture ? Sprite.Create(spriteTexture, new Rect(0f, 0f, spriteTexture.width, spriteTexture.height), new Vector2(0f, 0f), pixelsPerUnit) : null;
    }
  }
}
