using System;
using JetBrains.Annotations;
using OdinPlus.Common;
using OdinPlus.Quests;
using System.Collections.Generic;
using System.Reflection;
using OdinPlus.Items;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinPlus.Managers
{
  public class PrefabManager : AbstractManager<PrefabManager>
  {
    private readonly Dictionary<string, GameObject> _prefabList = new Dictionary<string, GameObject>();

    public GameObject Root { get; private set; }

    [UsedImplicitly]
    private void Awake()
    {
      HealthManager.Instance.OnHealthCheck += HealthCheck;
    }

    private protected override void OnInitialize()
    {
      base.OnInitialize();
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      
      Root = new GameObject("OdinPrefab");
      Root.transform.SetParent(OdinPlus.PrefabParent.transform);
    }

    private protected override void OnPostInitialize()
    {
      base.OnPostInitialize();
      CreateLegacyChest();
      CreateHuntTargetMonster();
      OdinPlus.OdinPostRegister(_prefabList);
    }

    private protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        if (_prefabList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _prefabList.Count: {_prefabList.Count}";
        }

        for (int i = 1; i <= 10; i++)
        {
          if (_prefabList.ContainsKey($"{OdinPlusItem.LegacyChest}{i}")) continue;
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _prefabList.ContainsKey({OdinPlusItem.LegacyChest}{i}): {_prefabList.Count}{_prefabList.ContainsKey($"{OdinPlusItem.LegacyChest}{i}")}";
        }

        foreach (var item in QuestRef.HunterMonsterList)
        {
          if (_prefabList.ContainsKey($"{item}Hunt")) continue;
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _prefabList.ContainsKey({item}Hunt): {_prefabList.Count}{_prefabList.ContainsKey($"{item}Hunt")}";
        }

        var odinLegacyPrefab = ZNetScene.instance.GetPrefab(OdinPlusItem.OdinLegacy);
        if (odinLegacyPrefab == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: odinLegacyPrefab == null: true";
        }

        var chestPrefab = ZNetScene.instance.GetPrefab(OdinPlusItem.Chest);
        if (chestPrefab == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: chestPrefab == null: true";
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

    public void AddManagedPrefab(string name, GameObject gameObject)
    {
      if (_prefabList.ContainsKey(name)) return;
      _prefabList.Add(name, gameObject);
    }

    public GameObject GetManagedPrefab(string name)
    {
      return !_prefabList.ContainsKey(name) ? null : _prefabList[name];
    }

    private void CreateHuntTargetMonster()
    {
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

      if (ZNetScene.instance == null || ZNetScene.instance.m_prefabs == null || ZNetScene.instance.m_prefabs.Count <= 0)
      {
        Log.Debug("ZNetScene.instance is null");
        return;
      }

      foreach (var item in QuestRef.HunterMonsterList)
      {
        _prefabList.Add($"{item}Hunt", HuntTarget.CreateMonster(item));
        Log.Debug($"Added {item}Hunt to PrefabList:{_prefabList.Count}");
      }
    }

    private void CreateLegacyChest()
    {
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");

      if (ZNetScene.instance == null || ZNetScene.instance.m_prefabs == null || ZNetScene.instance.m_prefabs.Count <= 0)
      {
        Log.Debug("ZNetScene.instance is null");
        return;
      }

      for (int i = 1; i <= 10; i++)
      {
        var odinLegacyPrefab = ZNetScene.instance.GetPrefab(OdinPlusItem.OdinLegacy);
        Log.Debug($"odinLegacyPrefab == null:{odinLegacyPrefab == null}");

        var chestPrefab = ZNetScene.instance.GetPrefab(OdinPlusItem.Chest);
        Log.Debug($"chestPrefab == null:{chestPrefab == null}");

        GameObject chest = Object.Instantiate(chestPrefab, Root.transform);
        Log.Debug($"chest == null:{chest == null}");
        

        chest.name = $"{OdinPlusItem.LegacyChest}{i}";

        Object.DestroyImmediate(chest.GetComponent<Rigidbody>());
        var staticPhysics = chest.AddComponent<StaticPhysics>();
        staticPhysics.m_pushUp = false;
        var container = chest.GetComponent<Container>();
        chest.AddComponent<LegacyChest>();
        var material = chest.GetComponentInChildren<Renderer>().material;

        material.SetFloat("_Hue", 0.3f);
        material.SetFloat("_Saturation", 0.5f);

        container.m_name = OdinPlusItem.LegacyChest;
        container.m_width = 1;
        container.m_height = 1;
        container.m_defaultItems.m_drops.Add(new DropTable.DropData {m_item = odinLegacyPrefab, m_stackMax = i, m_stackMin = i, m_weight = 1});
        
        Object.Instantiate(FxAssetManager.Instance.GetFxNN(OdinPlusFx.BlueSmoke), chest.transform);

        _prefabList.Add(chest.name, chest);
        Log.Debug($"Added {chest.name} to PrefabList:{_prefabList.Count}");
      }
    }
  }
}
