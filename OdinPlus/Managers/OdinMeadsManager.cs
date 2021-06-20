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

    protected override bool OnInitialize()
    {
      try
      {
        if (!base.OnInitialize()) return false;
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

        if (!Util.IsObjectDBReady())
        {
          Log.Debug("ObjectDB not ready - skipping");
          return false;
        }

        _root = new GameObject(OdinPlusItem.MeadTasty);
        _root.transform.SetParent(OdinPlus.PrefabParent.transform);
        _root.SetActive(false);
        _meadTasty = ObjectDB.instance.GetItemPrefab(OdinPlusItem.MeadTasty);


        return true;

      }
      catch (Exception e)
      {
        Log.Error(e);
        return false;
      }
    }

    protected override bool OnPostInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnPostInitialize()) return false;
        InitValMead();
        InitBuzzMead();
        OdinPlus.OdinPreRegister(_meadList, nameof(_meadList));
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
      var dependencyError = !ResourceAssetManager.Instance.IsInitialized 
                            || ResourceAssetManager.Instance.HasDependencyError()
                            || !StatusEffectsManager.Instance.IsInitialized
                            || StatusEffectsManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal($"ResourceAssetManager.Instance.IsInitialized: {ResourceAssetManager.Instance.IsInitialized}");
        Log.Fatal($"ResourceAssetManager.Instance.HasDependencyError: {ResourceAssetManager.Instance.HasDependencyError()}");
        Log.Fatal($"StatusEffectsManager.Instance.IsInitialized: {StatusEffectsManager.Instance.IsInitialized}");
        Log.Fatal($"StatusEffectsManager.Instance.HasDependencyError: {StatusEffectsManager.Instance.HasDependencyError()}");
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
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({name})");

        var statusEffects = StatusEffectsManager.Instance.GetStatusEffects(name);
        if (statusEffects == null)
        {
          Log.Error($"[{GetType().Name}] Unable to GetStatusEffects({name})");
          Log.Error($"[{GetType().Name}] {name} not created");
          return;
        }

        GameObject gameObject = UnityEngine.Object.Instantiate(_meadTasty, _root.transform);
        gameObject.name = name;
        var id = gameObject.GetComponent<ItemDrop>().m_itemData.m_shared;
        id.m_name = "$op_" + name + "_name";
        id.m_icons[0] = ResourceAssetManager.Instance.GetIconSprite(name);
        id.m_description = "$op_" + name + "_desc";
        id.m_consumeStatusEffect = statusEffects;
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
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var item in StatusEffectsManager.Instance.BuzzListKeys)
      {
        CreateValMeadPrefab(item);
      }
    }

    private void InitValMead()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var item in StatusEffectsManager.Instance.ValDataListKeys)
      {
        CreateValMeadPrefab(item);
      }
    }
  }
}
