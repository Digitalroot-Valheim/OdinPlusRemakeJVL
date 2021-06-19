using System;
using System.Collections.Generic;
using System.Reflection;
using OdinPlus.Common;
using UnityEngine;

namespace OdinPlus.Managers
{
  public class OdinMeadsManager : AbstractManager<OdinMeadsManager>
  {
    private static GameObject _meadTasty;
    private static Dictionary<string, GameObject> _meadList = new Dictionary<string, GameObject>();
    private static GameObject _root;

    protected override void OnInitialize()
    {
      base.OnInitialize();
      Log.Trace($"{GetType().Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      _root = new GameObject(OdinPlusItem.MeadTasty);
      _root.transform.SetParent(OdinPlus.PrefabParent.transform);
      _root.SetActive(false);

      var objectDB = ObjectDB.instance;
      _meadTasty = objectDB.GetItemPrefab(OdinPlusItem.MeadTasty);

      InitValMead();
      InitBuzzMead();

      OdinPlus.OdinPreRegister(_meadList, nameof(_meadList));
    }

    protected override void OnPostInitialize()
    {
      base.OnPostInitialize();
      Log.Trace($"{GetType().Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public override bool HasDependencyError()
    {
      Log.Trace($"{GetType().Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !ResourceAssetManager.Instance.IsInitialized;

      if (dependencyError)
      {
        Log.Fatal($"ResourceAssetManager.Instance.IsInitialized: {ResourceAssetManager.Instance.IsInitialized}");
      }

      return dependencyError;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        if (_meadTasty == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _meadTasty == null: {_meadTasty == null}";
        }

        if (_root == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _root == null: {_root == null}";
        }

        if (_meadList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _meadList.Count: {_meadList.Count}";
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

    private void CreateValMeadPrefab(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
        GameObject gameObject = UnityEngine.Object.Instantiate(_meadTasty, _root.transform);
        gameObject.name = name;
        var id = gameObject.GetComponent<ItemDrop>().m_itemData.m_shared;
        id.m_name = "$op_" + name + "_name";
        id.m_icons[0] = ResourceAssetManager.Instance.GetIconSprite(name);
        id.m_description = "$op_" + name + "_desc";
        id.m_consumeStatusEffect = StatusEffectsManager.Instance.StatusEffectsList[name];
        _meadList.Add(name, gameObject);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    private void InitBuzzMead()
    {
      Log.Trace($"{GetType().Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var item in StatusEffectsManager.Instance.BuzzList.Keys)
      {
        CreateValMeadPrefab(item);
      }
    }

    private void InitValMead()
    {
      Log.Trace($"{GetType().Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var item in StatusEffectsManager.Instance.ValDataList.Keys)
      {
        CreateValMeadPrefab(item);
      }
    }
  }
}
