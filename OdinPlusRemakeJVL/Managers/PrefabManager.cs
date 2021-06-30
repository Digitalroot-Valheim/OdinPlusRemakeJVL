using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Prefabs;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Managers
{
  internal class PrefabManager : AbstractManager<PrefabManager>, IOnPrefabsRegistered
  {
    private readonly List<ICreateable> _createables = new List<ICreateable>();

    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;
        _createables.Add(new OrnamentalFirePit());
        _createables.Add(new OrnamentalCauldron());
        _createables.Add(new OrnamentalOdin());
        _createables.Add(new OrnamentalMunin());
        _createables.Add(new OrnamentalGoblinShaman());
        return true;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return false;
      }
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus) => healthCheckStatus;

    /// <summary>
    /// Add our Custom Prefabs
    /// </summary>
    public void OnPrefabsRegistered()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Log.Trace($"[{GetType().Name}] Total _createables : {_createables.Count}");
        foreach (var createable in _createables)
        {
          try
          {
            Log.Trace($"[{GetType().Name}] Creating {createable}");
            Jotunn.Managers.PrefabManager.Instance.AddPrefab(createable?.Create());
          }
          catch (Exception e)
          {
            Log.Error(e);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    public void AddPrefab(GameObject prefab) => Jotunn.Managers.PrefabManager.Instance.AddPrefab(prefab);
    public GameObject CreateClonedPrefab(string name, string basename) => Jotunn.Managers.PrefabManager.Instance.CreateClonedPrefab(name, basename);
    public GameObject GetPrefab(string prefabName) => Jotunn.Managers.PrefabManager.Instance.GetPrefab(prefabName);

  }
}
