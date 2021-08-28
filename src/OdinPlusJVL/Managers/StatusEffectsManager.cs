using Digitalroot.Valheim.Common;
using Jotunn.Entities;
using OdinPlusJVL.StatusEffects;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Managers
{
  internal class StatusEffectsManager : AbstractManager<StatusEffectsManager>
  {
    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;
        AddStatusEffects();
        return true;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        return false;
      }
    }

    public override bool HasDependencyError()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !SpriteManager.Instance.IsInitialized
                            || SpriteManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal(Main.Instance, $"SpriteManager.Instance.IsInitialized: {SpriteManager.Instance.IsInitialized}");
        Log.Fatal(Main.Instance, $"SpriteManager.Instance.HasDependencyError: {SpriteManager.Instance.HasDependencyError()}");
      }

      return dependencyError;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = GetType().Name;
        return healthCheckStatus;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        healthCheckStatus.HealthStatus = HealthStatus.Failed;
        healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: {e.Message}";
        return healthCheckStatus;
      }
    }

    private void AddStatusEffects()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_ExpMeadS>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_ExpMeadM>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_ExpMeadL>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_WeightMeadS>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_WeightMeadM>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_WeightMeadL>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_InvisibleMeadS>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_InvisibleMeadM>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_InvisibleMeadL>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_PickaxeMeadS>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_PickaxeMeadM>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_PickaxeMeadL>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_BowsMeadS>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_BowsMeadM>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_BowsMeadL>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_SwordsMeadS>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_SwordsMeadM>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_SwordsMeadL>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_AxeMeadS>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_AxeMeadM>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_AxeMeadL>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_RunSpeed>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_MonsterAttackAmp1>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_MonsterAttackAmp2>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_MonsterAttackAmp3>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_MonsterAttackAmp4>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_MonsterAttackAmp5>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_SummonTrollPet>());
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(GetCustomStatusEffect<SE_SummonWolfPet>());
    }

    private CustomStatusEffect GetCustomStatusEffect<T>() where T : StatusEffect
    {
      Log.Trace(Main.Instance, $"[{GetType().Name}] Adding {typeof(T).Name}");
      return new CustomStatusEffect(ScriptableObject.CreateInstance<T>(), false);
    }
  }
}
