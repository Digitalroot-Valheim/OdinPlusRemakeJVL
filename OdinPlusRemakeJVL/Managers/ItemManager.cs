using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Items;
using System;
using System.Reflection;

namespace OdinPlusRemakeJVL.Managers
{
  internal class ItemManager : AbstractManager<ItemManager>
  {
    //private GameObject _meadTasty;
    //private GameObject _trophyGoblinShaman;

    //private readonly Dictionary<string, Sprite> _petItemList = new Dictionary<string, Sprite>
    //{
    //  {ItemNames.ScrollTroll, OdinPlus.TrollHeadIcon},
    //  {ItemNames.ScrollWolf, OdinPlus.WolfHeadIcon}
    //};

    // private readonly Dictionary<string, GameObject> _objectList = new Dictionary<string, GameObject>();

    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;
        Jotunn.Managers.ItemManager.OnVanillaItemsAvailable += AddClonedItems;
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
      var dependencyError = !SpriteManager.Instance.IsInitialized
                            || SpriteManager.Instance.HasDependencyError()
                            // || !FxAssetManager.Instance.IsInitialized
                            // || FxAssetManager.Instance.HasDependencyError()
                            || !StatusEffectsManager.Instance.IsInitialized
                            || StatusEffectsManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal($"SpriteManager.Instance.IsInitialized: {SpriteManager.Instance.IsInitialized}");
        Log.Fatal($"SpriteManager.Instance.HasDependencyError: {SpriteManager.Instance.HasDependencyError()}");
        Log.Fatal($"FxAssetManager.Instance.IsInitialized: {FxAssetManager.Instance.IsInitialized}");
        Log.Fatal($"FxAssetManager.Instance.HasDependencyError: {FxAssetManager.Instance.HasDependencyError()}");
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
        //  if (!_objectList.ContainsKey(ItemNames.OdinLegacy))
        //  {
        //    healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //    healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _objectList.ContainsKey(ItemNames.OdinLegacy): false";
        //  }

        //  if (!_objectList.ContainsKey(ItemNames.ScrollWolf))
        //  {
        //    healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //    healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _objectList.ContainsKey(ItemNames.ScrollWolf): false";
        //  }

        //  if (!_objectList.ContainsKey(ItemNames.ScrollTroll))
        //  {
        //    healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //    healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _objectList.ContainsKey(ItemNames.ScrollTroll): false";
        //  }
        //}

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

    private void AddClonedItems()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        OdinLegacy.Create();
      }
      catch (Exception e)
      {
        Log.Error(e);
        throw;
      }
      finally
      {
        Jotunn.Managers.ItemManager.OnVanillaItemsAvailable -= AddClonedItems;
      }
    }
  }
}
