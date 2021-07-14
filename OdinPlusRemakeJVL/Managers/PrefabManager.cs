﻿using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Prefabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Managers
{
  internal class PrefabManager : AbstractManager<PrefabManager>, IOnPrefabsRegistered
  {
    private readonly List<AbstractCustomPrefab> _createables = new List<AbstractCustomPrefab>();

    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;

        var assetBundle = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("OdinPlusRemakeJVL.Assets.digitalroot", Assembly.GetExecutingAssembly());
        var assets = assetBundle.LoadAllAssets<GameObject>();
        foreach (GameObject gameObject in assets)
        {
          AddPrefab(gameObject);
        }
        assetBundle.Unload(false);
        return true;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return false;
      }
    }

    #region Overrides of AbstractManager<PrefabManager>

    /// <inheritdoc />
    public override bool PostInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.PostInitialize()) return false;

        _createables.Add(new OrnamentalFirePit());
        _createables.Add(new OrnamentalCauldron());
        _createables.Add(new OrnamentalOdin());
        _createables.Add(new OrnamentalMunin());
        _createables.Add(new OrnamentalGoblinShaman());
        _createables.Add(new OrnamentalKeeper());
        return true;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return false;
      }
    }

    #endregion

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
        foreach (var createable in _createables.Select(c => c as ICreateable))
        {
          try
          {
            Log.Trace($"[{GetType().Name}] Creating {createable}");
            AddPrefab(createable?.Create());
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

    // ReSharper disable once MemberCanBePrivate.Global
    public void AddPrefab(GameObject prefab) => Jotunn.Managers.PrefabManager.Instance.AddPrefab(prefab);
    public GameObject CreateClonedPrefab(string name, string basename) => Jotunn.Managers.PrefabManager.Instance.CreateClonedPrefab(name, basename);
    public GameObject GetPrefab(string prefabName) => Jotunn.Managers.PrefabManager.Instance.GetPrefab(prefabName);

  }
}
