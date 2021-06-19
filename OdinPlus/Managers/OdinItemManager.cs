using OdinPlus.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace OdinPlus.Managers
{
  internal class OdinItemManager : AbstractManager<OdinItemManager>
  {
    private GameObject _meadTasty;
    private GameObject _trophyGoblinShaman;

    private readonly Dictionary<string, Sprite> _petItemList = new Dictionary<string, Sprite>
    {
      {OdinPlusItem.ScrollTroll, OdinPlus.TrollHeadIcon},
      {OdinPlusItem.ScrollWolf, OdinPlus.WolfHeadIcon}
    };

    private readonly Dictionary<string, GameObject> _objectList = new Dictionary<string, GameObject>();

    private GameObject _root;

    protected override void OnInitialize()
    {
      base.OnInitialize();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _root = new GameObject("ObjectList");
      _root.transform.SetParent(OdinPlus.PrefabParent.transform);
      _root.SetActive(false);

      var objectDB = ObjectDB.instance;
      _meadTasty = objectDB.GetItemPrefab(OdinPlusItem.MeadTasty);
      _trophyGoblinShaman = objectDB.GetItemPrefab(OdinPlusItem.TrophyGoblinShaman);

      InitLegacy();
      InitPetItem();
    }

    protected override void OnPostInitialize()
    {
      base.OnPostInitialize();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      OdinPlus.OdinPreRegister(_objectList, nameof(_objectList));
    }

    public override bool HasDependencyError()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !FxAssetManager.Instance.IsInitialized
                            || FxAssetManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal($"FxAssetManager.Instance.IsInitialized: {FxAssetManager.Instance.IsInitialized}");
        Log.Fatal($"FxAssetManager.Instance.HasDependencyError: {FxAssetManager.Instance.HasDependencyError()}");
      }

      return dependencyError;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        if (_meadTasty == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _meadTasty == null: {_meadTasty == null}";
        }

        if (_trophyGoblinShaman == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _trophyGoblinShaman == null: {_trophyGoblinShaman == null}";
        }

        if (_root == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _root == null: {_root == null}";
        }

        if (_petItemList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _petItemList.Count: {_petItemList.Count}";
        }

        if (_objectList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _objectList.Count: {_objectList.Count}";
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

    private void CreatePetItemPrefab(string iName, Sprite icon)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        GameObject go = UnityEngine.Object.Instantiate(_meadTasty, _root.transform);
        go.name = iName;

        var id = go.GetComponent<ItemDrop>().m_itemData.m_shared;
        id.m_name = $"$op_{iName}_name";
        id.m_icons[0] = icon;
        id.m_description = $"$op_{iName}_desc";

        id.m_maxStackSize = 1;
        id.m_consumeStatusEffect = StatusEffectsManager.Instance.StatusEffectsList[iName];

        go.GetComponent<ItemDrop>().m_itemData.m_quality = 4;
        id.m_maxQuality = 5;

        _objectList.Add(iName, go);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(iName), iName);
        e.Data.Add(nameof(icon), icon.ToString());
        throw;
      }
    }

    private void InitPetItem()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var pet in _petItemList)
      {
        CreatePetItemPrefab(pet.Key, pet.Value);
      }
    }

    private void InitLegacy()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      string name = OdinPlusItem.OdinLegacy;
      GameObject go = UnityEngine.Object.Instantiate(_trophyGoblinShaman, _root.transform);
      go.name = OdinPlusItem.OdinLegacy;
      var id = go.GetComponent<ItemDrop>().m_itemData.m_shared;
      id.m_name = $"$op_{name}_name";
      id.m_icons[0] = OdinPlus.OdinLegacyIcon;
      id.m_description = $"$op_{name}_desc";
      id.m_itemType = ItemDrop.ItemData.ItemType.None;

      id.m_maxStackSize = 10;
      id.m_maxQuality = 5;

      _objectList.Add(name, go);
    }

    public ItemDrop.ItemData GetItemData(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        return _objectList[name].GetComponent<ItemDrop>().m_itemData;
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    public GameObject GetObject(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        return _objectList[name];
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }
  }
}
