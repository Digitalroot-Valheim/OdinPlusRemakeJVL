using System;
using System.Collections.Generic;
using System.Reflection;
using OdinPlus.Common;
using UnityEngine;

namespace OdinPlus.Managers
{
  public class ResourceAssetManager : AbstractManager<ResourceAssetManager>
  {
    private readonly Dictionary<string, Sprite> _odinMeadsIcons = new Dictionary<string, Sprite>();

    private readonly string[] _odinMeadsName =
    {
      "ExpMeadS",
      "ExpMeadM",
      "ExpMeadL",
      "WeightMeadS",
      "WeightMeadM",
      "WeightMeadL",
      "InvisibleMeadS",
      "InvisibleMeadM",
      "InvisibleMeadL",
      "PickaxeMeadS",
      "PickaxeMeadM",
      "PickaxeMeadL",
      "BowsMeadS",
      "BowsMeadM",
      "BowsMeadL",
      "SwordsMeadS",
      "SwordsMeadM",
      "SwordsMeadL",
      "SpeedMeadsL",
      "AxeMeadS",
      "AxeMeadM",
      "AxeMeadL"
    };

    protected override void OnInitialize()
    {
      base.OnInitialize();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      LoadMeadsIcons();
    }

    public override bool HasDependencyError()
    {
      return false;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;

        if (_odinMeadsIcons.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _odinMeadsIcons.Count: {_odinMeadsIcons.Count}";
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

    private void AddIcon(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Sprite sprite = Util.LoadResouceIcon(name);
        _odinMeadsIcons.Add(name, sprite);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    public Sprite GetIconSprite(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        return _odinMeadsIcons.ContainsKey(name) ? _odinMeadsIcons[name] : null;
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    private void LoadMeadsIcons()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var name in _odinMeadsName)
      {
        AddIcon(name);
      }
    }
  }
}
