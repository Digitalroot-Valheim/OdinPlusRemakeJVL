using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Common.Names;
using OdinPlusRemakeJVL.GameObjects;
using OdinPlusRemakeJVL.GameObjects.Npcs;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinPlusRemakeJVL.Managers
{
  /// <summary>
  /// Manages Odin's Camp
  /// </summary>
  internal class OdinsCampManager : AbstractManager<OdinsCampManager>, IOnZoneSystemLoaded, IOnZNetSceneReady, IOnSpawnedPlayer, IDestroyable
  {
    private GameObject _odinCampGameObject;
    private GameObject terrain;
    private readonly Dictionary<string, ISpawnable> _customGameObjects = new Dictionary<string, ISpawnable>();
    private readonly List<AbstractCustomGameObject> _odinPlusObjects = new List<AbstractCustomGameObject>();

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
      if (_odinPlusObjects.Count > 0) _odinPlusObjects.Clear();

      AddOdinsEmissary();
      AddOdinsFirePit();
      AddOdinsCauldron();
      AddMunin();
      AddShaman();

      Log.Debug($"[{GetType().Name}] Total Spawnables ({_customGameObjects.Count}) ");
      foreach (var spawnable in _customGameObjects)
      {
        try
        {
          Log.Debug($"[{GetType().Name}] Spawning : {spawnable.Key}");
          spawnable.Value.Spawn(_odinCampGameObject.transform);
        }
        catch (Exception e)
        {
          Log.Error(e);
        }
      }
    }

    public void OnZoneSystemLoaded()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      SetSpawnPosition();
      _odinCampGameObject.SetActive(true);
      AddForceField();
    }

    public void OnZNetSceneReady(ZNetScene zNetScene)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({zNetScene.name})");
      AddTerrain();
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

    private void AddForceField() // ToDo: Refactor into Prefab
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var forceFieldPreFab = ZoneSystem.instance.m_locations[85].m_prefab.transform.Find(PrefabNames.ForceField);
        var forceField = Object.Instantiate(forceFieldPreFab, _odinCampGameObject.transform);
        forceField.transform.localScale = Vector3.one * 10;
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void AddMunin()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var customGameObject = new MuninCustomGameObject();
        customGameObject.SetLocalPositionOffset(new Vector3(2.56f, -0.001f, -1.98f));
        customGameObject.SetLocalRotationOffset(new Quaternion(0, 0.345f, 0, -0.9386f));
        _customGameObjects.Add(customGameObject.Name, customGameObject);
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void AddOdinsEmissary()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var customGameObject = new OdinsEmissaryCustomGameObject();
        customGameObject.SetLocalPositionOffset(new Vector3(0.6f, -0.045f, 2.1f));
        customGameObject.SetLocalRotationOffset(new Quaternion(0, 0.6605f, 0, 0.7508f));
        _customGameObjects.Add(customGameObject.Name, customGameObject);
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void AddOdinsFirePit()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var customGameObject = new OdinsFirePitCustomGameObject();
        customGameObject.SetLocalPositionOffset(new Vector3(-0.42f, -0.015f, -1f));
        _customGameObjects.Add(customGameObject.Name, customGameObject);
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void AddOdinsCauldron()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var customGameObject = new OdinsCauldronCustomGameObject();
        customGameObject.SetLocalPositionOffset(new Vector3(-0.42f, -0.015f, -1f));
        customGameObject.SetLocalRotationOffset(new Quaternion(0, 0.6817f, 0, 0.7316f));

        foreach (var gameObject in _customGameObjects)
        {
          Log.Error($"{gameObject.Key}:{gameObject.Value}");
        }

        // customGameObject.Cauldron.GetComponent<OdinsCauldronCustomMonoBehaviour>().Talker = ((OdinsEmissaryCustomGameObject)_customGameObjects[Common.Utils.Utils.Localize("$op_god")]).OdinsEmissary;
        // m_odinPot = caul.AddComponent<OdinTradeShop>();
        // OdinPlus.traderNameList.Add(m_odinPot.m_name);
        _customGameObjects.Add(customGameObject.Name, customGameObject);
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void AddShaman()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var customGameObject = new ShamanCustomGameObject();
        customGameObject.SetLocalPositionOffset(new Vector3(-1.7409f, -0.008f, 0.2318f));
        customGameObject.SetLocalRotationOffset(new Quaternion(0, 0.8026f, 0, 0.5965f));
        _customGameObjects.Add(customGameObject.Name, customGameObject);
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void AddTerrain() // ToDo: Refactor into Behaviour
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (terrain == null)
        {
          terrain = new GameObject("terrain");
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
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    public Vector3 GetOdinCampLocation()
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

    private void SetSpawnPosition()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _odinCampGameObject.transform.localPosition = Common.Utils.Utils.GetStartTemplesPosition() + new Vector3(-12.6f, 0.05f, -11f);
    }

    #region Implementation of IDestroyable

    /// <inheritdoc />
    public void OnDestroy()
    {
      Log.Debug($"[{GetType().Name}] Total Spawnables ({_customGameObjects.Count}) ");
      foreach (var spawnable in _customGameObjects)
      {
        try
        {
          Log.Debug($"[{GetType().Name}] Destroying : {spawnable.Key}");
          spawnable.Value.OnDestroy();
        }
        catch (Exception e)
        {
          Log.Error(e);
        }
      }
    }

    #endregion
  }
}
