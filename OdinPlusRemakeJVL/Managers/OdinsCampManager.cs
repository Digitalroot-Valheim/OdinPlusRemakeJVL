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
    private GameObject terrain;

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
      if (pot != null)
      {
        pot.Spawn(_odinCampGameObject.transform);
        pot.FirePit.transform.SetParent(_odinCampGameObject.transform);
        pot.Cauldron.transform.SetParent(_odinCampGameObject.transform);
        pot.FirePit.transform.localPosition = new Vector3(1.5f, 0, -0.5f);
        pot.Cauldron.transform.localPosition = new Vector3(1.5f, 0, -0.5f);
      }

      var odin = _odinCampGameObject.GetComponent<OdinNpc>();
      if (odin != null)
      {
        odin.Spawn(_odinCampGameObject.transform);
        odin.Odin.transform.localPosition = Vector3.forward * 3f;
        odin.Odin.transform.localPosition = Vector3.down * 0.1f;
        odin.Odin.transform.Rotate(0f, 45f, 0f);  
      }
    }

    public void OnZoneSystemLoaded()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _odinCampGameObject.transform.position = Common.Utils.GetStartTemplesPosition() + new Vector3(-12.6f, 0.05f, -11f);
      _odinCampGameObject.SetActive(true);

      AddForceField();
    }

    public void OnZNetSceneReady(ZNetScene zNetScene)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({zNetScene.name})");
      AddTerrain();
      AddOdin();
      AddPot();
      // AddBird();
      // PrefabManager.Instance.AddPrefab(_odinCampGameObject);
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

    private void AddBird()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    private void AddForceField()
    {
      var forceFieldPreFab = ZoneSystem.instance.m_locations[85].m_prefab.transform.Find(ItemNames.ForceField);
      var forceField = UnityEngine.Object.Instantiate(forceFieldPreFab, _odinCampGameObject.transform);
      forceField.transform.localScale = Vector3.one * 10;
    }

    private void AddOdin()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _odinCampGameObject.AddComponent<OdinNpc>();
      var odin = _odinCampGameObject.GetComponent<OdinNpc>();
      odin.transform.SetParent(_odinCampGameObject.transform);
    }

    private void AddPot()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _odinCampGameObject.AddComponent<OdinPot>();
      var pot = _odinCampGameObject.GetComponent<OdinPot>();
      pot.transform.SetParent(_odinCampGameObject.transform);
      pot.Talker = _odinCampGameObject.GetComponent<OdinNpc>().gameObject;

      // m_odinPot = caul.AddComponent<OdinTrader>();
      // OdinPlus.traderNameList.Add(m_odinPot.m_name);
    }

    private void AddTerrain()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (terrain == null)
      {
        terrain = new GameObject("terrain");
        //terrain.AddComponent<ZNetView>();
        //terrain.AddComponent<Piece>();
        var tm = terrain.AddComponent<TerrainModifier>();
        terrain.gameObject.transform.SetParent(_odinCampGameObject.transform);
        terrain.gameObject.transform.localPosition = new Vector3(0, 0, 0);
        tm.m_playerModifiction = false;
        tm.m_levelOffset = 0.01f;

        tm.m_level = true;
        tm.m_levelRadius = 4f;
        tm.m_square = false;

        tm.m_smooth = false;

        tm.m_smoothRadius = 9.5f;
        tm.m_smoothPower = 3f;


        tm.m_paintRadius = 3.5f;
        tm.m_paintCleared = true;
        tm.m_paintType = TerrainModifier.PaintType.Dirt;
      }
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
