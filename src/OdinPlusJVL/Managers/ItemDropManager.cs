using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using OdinPlusJVL.ItemDrops;
using System;
using System.Reflection;

namespace OdinPlusJVL.Managers
{
  internal class ItemDropManager : AbstractManager<ItemDropManager>, IOnVanillaItemsAvailable
  {
    //private GameObject _meadTasty;
    //private GameObject _trophyGoblinShaman;

    //private readonly Dictionary<string, Sprite> _petItemList = new Dictionary<string, Sprite>
    //{
    //  {ItemDropNames.ScrollTroll, OdinPlus.TrollHeadIcon},
    //  {ItemDropNames.ScrollWolf, OdinPlus.WolfHeadIcon}
    //};

    // private readonly Dictionary<string, GameObject> _objectList = new Dictionary<string, GameObject>();

    public override bool HasDependencyError()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !SpriteManager.Instance.IsInitialized
                            || SpriteManager.Instance.HasDependencyError()
                            || !StatusEffectsManager.Instance.IsInitialized
                            || StatusEffectsManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal(Main.Instance, $"SpriteManager.Instance.IsInitialized: {SpriteManager.Instance.IsInitialized}");
        Log.Fatal(Main.Instance, $"SpriteManager.Instance.HasDependencyError: {SpriteManager.Instance.HasDependencyError()}");
        Log.Fatal(Main.Instance, $"FxAssetManager.Instance.IsInitialized: {FxAssetManager.Instance.IsInitialized}");
        Log.Fatal(Main.Instance, $"FxAssetManager.Instance.HasDependencyError: {FxAssetManager.Instance.HasDependencyError()}");
        Log.Fatal(Main.Instance, $"StatusEffectsManager.Instance.IsInitialized: {StatusEffectsManager.Instance.IsInitialized}");
        Log.Fatal(Main.Instance, $"StatusEffectsManager.Instance.HasDependencyError: {StatusEffectsManager.Instance.HasDependencyError()}");
      }

      return dependencyError;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        //if (_meadTasty == null)
        //{
        //  healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //  healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _meadTasty == null: true";
        //}

        //if (_trophyGoblinShaman == null)
        //{
        //  healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //  healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _trophyGoblinShaman == null: true";
        //}

        //if (_petItemList.Count == 0)
        //{
        //  healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //  healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _petItemList.Count: {_petItemList.Count}";
        //}

        //if (_objectList.Count == 0)
        //{
        //  healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //  healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _objectList.Count: {_objectList.Count}";
        //}
        //else
        //{
        //  if (!_objectList.ContainsKey(ItemDropNames.OdinLegacy))
        //  {
        //    healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //    healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _objectList.ContainsKey(ItemDropNames.OdinLegacy): false";
        //  }

        //  if (!_objectList.ContainsKey(ItemDropNames.ScrollWolf))
        //  {
        //    healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //    healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _objectList.ContainsKey(ItemDropNames.ScrollWolf): false";
        //  }

        //  if (!_objectList.ContainsKey(ItemDropNames.ScrollTroll))
        //  {
        //    healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //    healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _objectList.ContainsKey(ItemDropNames.ScrollTroll): false";
        //  }
        //}

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

    public void OnVanillaItemsAvailable()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        OdinLegacy.Create();
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }
  }
}
