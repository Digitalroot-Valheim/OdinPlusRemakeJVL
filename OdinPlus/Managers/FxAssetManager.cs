using OdinPlus.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace OdinPlus.Managers
{
  public class FxAssetManager : AbstractManager<FxAssetManager>
  {
    private Transform _root;
    private readonly Dictionary<string, GameObject> _fxNnList = new Dictionary<string, GameObject>();

    protected override void OnInitialize()
    {
      base.OnInitialize();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _root = new GameObject("Assets").transform;
      _root.SetParent(OdinPlus.PrefabParent.transform);
      SetupFxNN();
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        if (_fxNnList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _fxNnList.Count: {_fxNnList.Count}";
        }

        if (!_fxNnList.ContainsKey(OdinPlusFx.RedSmoke))
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _fxNnList.ContainsKey({OdinPlusFx.RedSmoke}): {_fxNnList.ContainsKey(OdinPlusFx.RedSmoke)}";
        }

        if (!_fxNnList.ContainsKey(OdinPlusFx.BlueSmoke))
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _fxNnList.ContainsKey({OdinPlusFx.BlueSmoke}): {_fxNnList.ContainsKey(OdinPlusFx.BlueSmoke)}";
        }

        if (!_fxNnList.ContainsKey(OdinPlusFx.YellowSmoke))
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _fxNnList.ContainsKey({OdinPlusFx.YellowSmoke}): {_fxNnList.ContainsKey(OdinPlusFx.YellowSmoke)}";
        }

        if (!_fxNnList.ContainsKey(OdinPlusFx.GreenSmoke))
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _fxNnList.ContainsKey({OdinPlusFx.GreenSmoke}): {_fxNnList.ContainsKey(OdinPlusFx.GreenSmoke)}";
        }

        if (_root == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _root == null: {_root == null}";
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

    public override bool HasDependencyError()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !StatusEffectsManager.Instance.IsInitialized
                            || StatusEffectsManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal($"StatusEffectsManager.Instance.IsInitialized: {StatusEffectsManager.Instance.IsInitialized}");
        Log.Fatal($"StatusEffectsManager.Instance.HasDependencyError: {StatusEffectsManager.Instance.HasDependencyError()}");
      }

      return dependencyError;
    }

    private GameObject InsVal(string prefab, string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var a = UnityEngine.Object.Instantiate(ZNetScene.instance.GetPrefab(prefab), _root);
        a.name = name;
        return a;
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(prefab), prefab);
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    private GameObject InsVal(string prefab, string par, string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var a = UnityEngine.Object.Instantiate(ZNetScene.instance.GetPrefab(prefab).FindObject(par), _root);
        a.name = name;
        return a;
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(prefab), prefab);
        e.Data.Add(nameof(par), par);
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    private GameObject ValFXcc(string prefab, string name, Color color, int whichList = 0)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var go = InsVal(prefab, name);
        go.GetComponent<Renderer>().material.color = color;
        SelectList(go, name, whichList);
        return go;
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(prefab), prefab);
        e.Data.Add(nameof(name), name);
        e.Data.Add(nameof(color), color.ToString());
        e.Data.Add(nameof(whichList), whichList);
        throw;
      }
    }

    private GameObject ValFXcc(string prefab, string par, string name, Color color, Action<GameObject> action, int whichList = 0)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var gameObject = InsVal(prefab, par, name);
        gameObject.GetComponent<Renderer>().material.color = color;
        SelectList(gameObject, name, whichList);
        action(gameObject);
        return gameObject;
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(prefab), prefab);
        e.Data.Add(nameof(par), par);
        e.Data.Add(nameof(name), name);
        e.Data.Add(nameof(color), color.ToString());
        e.Data.Add(nameof(action), action.ToString());
        e.Data.Add(nameof(whichList), whichList);
        throw;
      }
    }

    private void SelectList(GameObject gameObject, string name, int whichList)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        switch (whichList)
        {
          case 0:
            break;
          case 1:
            _fxNnList.Add(name, gameObject);
            break;
        }
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(gameObject), JsonSerializationProvider.ToJson(gameObject));
        e.Data.Add(nameof(name), name);
        e.Data.Add(nameof(whichList), whichList);
        throw;
      }
    }

    private void OdinSmoke(GameObject gameObject)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var ps = gameObject.GetComponent<ParticleSystem>();
        var emission = ps.emission;
        var main = ps.main;
        main.maxParticles = 60;
        emission.rateOverTime = 30;
        main.startLifetime = 1;
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(gameObject), JsonSerializationProvider.ToJson(gameObject));
        throw;
      }
    }

    // ReSharper disable once InconsistentNaming
    public GameObject GetFxNN(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        return _fxNnList[name];
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    // ReSharper disable once InconsistentNaming
    private void SetupFxNN()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      ValFXcc("odin", "odinsmoke", OdinPlusFx.RedSmoke, Color.red, OdinSmoke, 1);
      ValFXcc("odin", "odinsmoke", OdinPlusFx.BlueSmoke, Color.blue, OdinSmoke, 1);
      ValFXcc("odin", "odinsmoke", OdinPlusFx.YellowSmoke, Color.yellow, OdinSmoke, 1);
      ValFXcc("odin", "odinsmoke", OdinPlusFx.GreenSmoke, Color.green, OdinSmoke, 1);
    }
  }
}
