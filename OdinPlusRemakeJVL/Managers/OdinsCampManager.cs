using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Npcs;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Managers
{
  /// <summary>
  /// Manages Odin's Camp
  /// </summary>
  internal class OdinsCampManager : AbstractManager<OdinsCampManager>, IOnZoneSystemLoaded, IOnZNetSceneReady, IOnSpawnedPlayer
  {
    private GameObject _odinCampGameObject;

    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;
        _odinCampGameObject = new GameObject(CustomPrefabNames.OdinsCamp);
        _odinCampGameObject.transform.SetParent(Main.RootObject.transform);
        _odinCampGameObject.SetActive(false);
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
      return false;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = GetType().Name;
        if (_odinCampGameObject == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}] _odinCampGameObject != null : {_odinCampGameObject != null}";
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

    public void OnSpawnedPlayer(Vector3 position)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({position})");
      OdinPot pot = _odinCampGameObject.GetComponent<OdinPot>();
      pot?.Spawn(_odinCampGameObject.transform);
      // var odin = _odinCampGameObject.GetComponent<OdinNpc>();
      // Log.Trace($"[{GetType().Name}] odin == null : {odin == null}");
      // odin?.Spawn(_odinCampGameObject.transform);
      // Debug.Assert(odin != null, nameof(odin) + " != null");
      // odin.transform.localPosition = Vector3.forward * 4;
    }

    public void OnZoneSystemLoaded()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _odinCampGameObject.transform.position = Common.Utils.GetStartTemplesPosition() + new Vector3(-6, 0.1f, -8);
      _odinCampGameObject.SetActive(true);
    }

    public void OnZNetSceneReady(ZNetScene zNetScene)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({zNetScene.name})");
      // AddOdin();
      AddPot();
      // AddBird();
      // PrefabManager.Instance.AddPrefab(_odinCampGameObject);
      // PrefabManager.Instance.RegisterToZNetScene(_odinCampGameObject);
      // ZNetScene.instance.SpawnObject(_odinCampGameObject.gameObject.transform.position, Quaternion.Euler(_odinCampGameObject.gameObject.transform.position), _odinCampGameObject);
      // ZNetScene.instance.SpawnObject(_odinCampGameObject.gameObject.transform.position, Quaternion.Euler(_odinCampGameObject.gameObject.transform.position), PrefabManager.Instance.GetPrefab("fire_pit"));
    }

    //private Vector3 FindSpawnPoint()
    //{
    //  Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    //  var a = UnityEngine.Random.Range(10, 10);
    //  var b = UnityEngine.Random.Range(10, 10);
    //  var c = ZoneSystem.instance.GetGroundHeight(new Vector3(a, 500, b)); // ToDo: Research
    //  var startTemplePosition = Common.Utils.GetStartTemplesPosition();
    //  if (startTemplePosition != Vector3.zero)
    //  {
    //    Log.Trace($"[{GetType().Name}] StartTemple at {startTemplePosition}");
    //    // return new Vector3(locationInstance.m_position.x, locationInstance.m_position.y, locationInstance.m_position.z);
    //    // var odinsCampPosition = startTemplePosition + new Vector3(-6, 0.2f, -8);
    //    var odinsCampPosition = startTemplePosition;
    //    Log.Trace($"[{GetType().Name}] Creating Odin's Camp at {odinsCampPosition}");
    //    return odinsCampPosition;
    //  }
    //  Log.Error($"[{GetType().Name}] Can't find StartTemple");
    //  Log.Error($"[{GetType().Name}] Using (x,y,z) ({a},{c},{b})");
    //  return new Vector3(a, c, b);
    //}

    private void AddOdin()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _odinCampGameObject.AddComponent<OdinNpc>();
      var odin = _odinCampGameObject.GetComponent<OdinNpc>();
      odin.transform.SetParent(_odinCampGameObject.transform);
      
    }

    private void AddBird()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    private void AddPot()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _odinCampGameObject.AddComponent<OdinPot>();
      var pot = _odinCampGameObject.GetComponent<OdinPot>();
      pot.transform.SetParent(_odinCampGameObject.transform);

      // pot.Fire.transform.SetParent(_odinCampGameObject.transform);
      // pot.Cauldron.transform.SetParent(_odinCampGameObject.transform);
      // pot.Fire.transform.localPosition = new Vector3(1.5f, 0, -0.5f);
      // pot.Cauldron.transform.localPosition = new Vector3(1.5f, 0, -0.5f);
      // pot.Talker = _odinCampGameObject.GetComponent<OdinNpc>().gameObject;

      // m_odinPot = caul.AddComponent<OdinTrader>();
      // OdinPlus.traderNameList.Add(m_odinPot.m_name);
    }

    public Vector3 OdinCampLocation()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (_odinCampGameObject != null)
      {
        if (_odinCampGameObject.transform != null)
        {
          return _odinCampGameObject.transform.position;
        }
        Log.Error($"[{GetType().Name}] _odinCampGameObject.transform != null : {_odinCampGameObject.transform != null}");
      }
      Log.Error($"[{GetType().Name}] _odinCampGameObject != null : {_odinCampGameObject != null}");
      return Vector3.zero;
    }

    public Vector3 OdinLocation()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var odin = _odinCampGameObject.GetComponent<OdinNpc>();

      Log.Error($"[{GetType().Name}] odin != null : {odin != null}");

      if (odin != null)
      {
        Log.Debug($"[{GetType().Name}] odin.enabled : {odin.enabled}");
        Log.Debug($"[{GetType().Name}] odin.isActiveAndEnabled : {odin.isActiveAndEnabled}");
        return odin.transform.position;
      }
      Log.Debug($"[{GetType().Name}] Unable to find Odin's Location");
      return Vector3.zero;
    }
  }
}
