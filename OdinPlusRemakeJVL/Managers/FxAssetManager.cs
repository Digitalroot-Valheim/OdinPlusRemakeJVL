using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Managers
{
  public class FxAssetManager : AbstractManager<FxAssetManager>
  {
    private readonly Dictionary<string, GameObject> _fxList = new Dictionary<string, GameObject>();

    protected override bool OnInitialize()
    {
      try
      {
        if (!base.OnInitialize()) return false;
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

        if (!Common.Utils.IsZNetSceneReady())
        {
          Log.Debug("ZNetScene.instance is null");
          return false;
        }

        CreateFxs();
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
      var dependencyError = !Common.Utils.IsZNetSceneReady();
                            
      if (dependencyError)
      {
        Log.Fatal($"Common.Utils.IsZNetSceneReady(): {Common.Utils.IsZNetSceneReady()}");
      }
      return dependencyError;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({healthCheckStatus.HealthStatus})");
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        if (!_fxList.Any())
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _fxList.Count: {_fxList.Count}";
        }

        foreach (var name in OdinPlusFxNames.AllNames)
        {
          if (!_fxList.ContainsKey(name))
          {
            healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
            healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _fxList.ContainsKey({name}): {_fxList.ContainsKey(name)}";
          }
        }

        if (!Common.Utils.IsZNetSceneReady())
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: Common.Utils.IsZNetSceneReady(): {Common.Utils.IsZNetSceneReady()}";
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

    private void CreateFxs()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      CreateFx("odin", "odinsmoke", OdinPlusFxNames.RedSmoke, Color.red);
      CreateFx("odin", "odinsmoke", OdinPlusFxNames.BlueSmoke, Color.blue);
      CreateFx("odin", "odinsmoke", OdinPlusFxNames.YellowSmoke, Color.yellow);
      CreateFx("odin", "odinsmoke", OdinPlusFxNames.GreenSmoke, Color.green);
    }

    private void CreateFx(string prefab, string particle, string name, Color color)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab}, {particle}, {name}, {color})");
        var gameObject = UnityEngine.Object.Instantiate(ZNetScene.instance.GetPrefab(prefab).FindGameObject(particle));
        gameObject.name = name;
        var material = gameObject.GetComponent<Renderer>().material;
        material.color = color;
        var particleSystem = gameObject.GetComponent<ParticleSystem>();
        var emission = particleSystem.emission;
        var main = particleSystem.main;
        main.maxParticles = 60;
        emission.rateOverTime = 30;
        main.startLifetime = 1;

        if (!_fxList.ContainsKey(name))
        {
          _fxList.Add(name, gameObject);
        }
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(prefab), prefab);
        e.Data.Add(nameof(particle), particle);
        e.Data.Add(nameof(name), name);
        e.Data.Add(nameof(color), color.ToString());
        throw;
      }
    }

    public GameObject GetFx(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({name})");
        return _fxList.ContainsKey(name) ? _fxList[name] : null;
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }
  }
}
