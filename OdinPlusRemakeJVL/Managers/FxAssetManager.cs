using Jotunn.Managers;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Managers
{
  public class FxAssetManager : AbstractManager<FxAssetManager>, IOnZNetSceneReady
  {
    private readonly Dictionary<string, GameObject> _fxList = new Dictionary<string, GameObject>();

    public override void Initialize()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Trace($"[{GetType().Name}] {nameof(IsInitialized)}: {IsInitialized}");
      if (IsInitialized) return;

      IsInitialized = OnInitialize();
      if (!IsInitialized)
      {
        Log.Fatal($"[{GetType().Name}]: IsInitialized: {IsInitialized}");
        Log.Fatal($"[{GetType().Name}]: Failed to IsInitialize");
        Log.Fatal($"[{GetType().Name}]: Checking health status");
        HealthManager.Instance.HealthCheck();
        HealthManager.Instance.OnHealthCheck -= HealthCheck;
      }
    }

    public override bool HasDependencyError()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !Common.Utils.IsZNetSceneReady();
                            
      if (dependencyError)
      {
        Log.Fatal($"[{GetType().Name}] Common.Utils.IsZNetSceneReady(): {Common.Utils.IsZNetSceneReady()}");
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

        foreach (var name in FxNames.AllNames)
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

    public void OnZNetSceneReady(ZNetScene zNetScene)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (HasDependencyError()) return;
        CreateFxs(zNetScene);
      }
      catch (Exception ex)
      {
        Log.Error(ex);
      }
    }

    private void CreateFxs(ZNetScene zNetScene)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      CreateFx(zNetScene, "odin", "odinsmoke", FxNames.RedSmoke, Color.red);
      CreateFx(zNetScene, "odin", "odinsmoke", FxNames.BlueSmoke, Color.blue);
      CreateFx(zNetScene, "odin", "odinsmoke", FxNames.YellowSmoke, Color.yellow);
      CreateFx(zNetScene, "odin", "odinsmoke", FxNames.GreenSmoke, Color.green);
    }

    private void CreateFx(ZNetScene zNetScene, string prefab, string particle, string name, Color color)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab}, {particle}, {name}, {color})");
        var gameObject = UnityEngine.Object.Instantiate(PrefabManager.Instance.GetPrefab(prefab).FindGameObject(particle));
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
