using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using OdinPlus.Common;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinPlus.Managers
{
  public class FxAssetManager : AbstractManager<FxAssetManager>
  {
    public Transform Root;
    private readonly Dictionary<string, GameObject> _fxNnList = new Dictionary<string, GameObject>();

    [UsedImplicitly]
    private void Awake()
    {
      Root = new GameObject("Assets").transform;
      Root.SetParent(OdinPlus.PrefabParent.transform);
    }

    private protected override void OnInitialize()
    {
      base.OnInitialize();
      SetupFxNN();
    }

    private protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
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

    private GameObject InsVal(string prefab, string name)
    {
      var a = Object.Instantiate(ZNetScene.instance.GetPrefab(prefab), Root);
      a.name = name;
      return a;
    }

    private GameObject InsVal(string prefab, string par, string name)
    {
      var a = Object.Instantiate(ZNetScene.instance.GetPrefab(prefab).FindObject(par), Root);
      a.name = name;
      return a;
    }

    private GameObject ValFXcc(string prefab, string name, Color col, int whichList = 0)
    {
      var go = InsVal(prefab, name);
      go.GetComponent<Renderer>().material.color = col;
      SelectList(go, name, whichList);
      return go;
    }

    private GameObject ValFXcc(string prefab, string par, string name, Color col, Action<GameObject> action, int whichList = 0)
    {
      var go = InsVal(prefab, par, name);
      go.GetComponent<Renderer>().material.color = col;
      SelectList(go, name, whichList);
      action(go);
      return go;
    }

    private void SelectList(GameObject go, string name, int whichList)
    {
      switch (whichList)
      {
        case 0:
          break;
        case 1:
          _fxNnList.Add(name, go);
          break;
      }
    }

    private static void Nothing(GameObject go)
    {
    }

    private static void odinSmoke(GameObject go)
    {
      var ps = go.GetComponent<ParticleSystem>();
      var e = ps.emission;
      var m = ps.main;
      m.maxParticles = 60;
      e.rateOverTime = 30;
      m.startLifetime = 1;
    }

    public GameObject GetFxNN(string name)
    {
      return _fxNnList[name];
    }

    private void SetupFxNN()
    {
      ValFXcc("odin", "odinsmoke", OdinPlusFx.RedSmoke, Color.red, odinSmoke, 1);
      ValFXcc("odin", "odinsmoke", OdinPlusFx.BlueSmoke, Color.blue, odinSmoke, 1);
      ValFXcc("odin", "odinsmoke", OdinPlusFx.YellowSmoke, Color.yellow, odinSmoke, 1);
      ValFXcc("odin", "odinsmoke", OdinPlusFx.GreenSmoke, Color.green, odinSmoke, 1);
    }
  }

  public static class OdinPlusFx
  {
    public static string RedSmoke = nameof(RedSmoke);
    public static string BlueSmoke = nameof(BlueSmoke);
    public static string YellowSmoke = nameof(YellowSmoke);
    public static string GreenSmoke = nameof(GreenSmoke);
  }
}
