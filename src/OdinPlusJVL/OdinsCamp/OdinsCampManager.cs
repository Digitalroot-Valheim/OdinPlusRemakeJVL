using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using OdinPlusJVL.Behaviours;
using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Extensions;
using OdinPlusJVL.Managers;
using System;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinPlusJVL.OdinsCamp
{
  /// <summary>
  /// Manages Odin's Camp
  /// </summary>
  internal class OdinsCampManager : AbstractManager<OdinsCampManager>, IOnZoneSystemLoaded, IOnZNetSceneReady, IOnSpawnedPlayer, IDestroyable
  {
    private GameObject _odinCamp
      , _odinsCampTerrain
      , _odinsCauldron
      , _odinsEmissary
      , _odinsFirePit
      , _odinsMunin
      , _odinsShaman
      ;

    private Transform _odinsForceField;

    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;
        _odinCamp = new GameObject(PrefabNames.OdinsCamp);
        _odinCamp.transform.SetParent(Main.RootObject.transform);
        _odinCamp.SetActive(false);
        return true;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        return false;
      }
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = GetType().Name;
        if (_odinCamp == null)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}] _odinCamp != null : {_odinCamp != null}";
        }

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

    public void OnSpawnedPlayer(Vector3 position)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({position})");
      AddOdinsFirePit();
      AddOdinsCauldron();
      AddOdinsMunin();
      AddOdinsShaman();
      AddOdinsEmissary();
      UpdateOdinsCauldron();
    }

    public void OnZoneSystemLoaded()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      SetSpawnPosition();
      _odinCamp.SetActive(true);
      AddOdinsForceField();
    }

    public void OnZNetSceneReady(ZNetScene zNetScene)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({zNetScene.name})");
      AddOdinsCampTerrain();
    }

    private void AddOdinsCampTerrain()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (_odinsCampTerrain != null) return;
        _odinsCampTerrain = new GameObject("terrain");
        var tm = _odinsCampTerrain.AddComponent<TerrainModifier>();
        _odinsCampTerrain.gameObject.transform.SetParent(_odinCamp.transform);
        _odinsCampTerrain.gameObject.transform.localPosition = new Vector3(0, 0, 0);
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
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void AddOdinsCauldron()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        _odinsCauldron = PrefabManager.Instance.CreateClonedPrefab(PrefabNames.OdinsCampCauldron, PrefabNames.OrnamentalCauldron);
        _odinsCauldron.AddMonoBehaviour<UnRemoveableCustomMonoBehaviour>();
        _odinsCauldron.AddMonoBehaviour<OdinsCauldronCustomMonoBehaviour>();
        _odinsCauldron.transform.SetParent(_odinCamp.transform);
        _odinsCauldron.transform.localPosition = new Vector3(-0.42f, -0.015f, -1f);
        _odinsCauldron.transform.localRotation = new Quaternion(0, 0.6817f, 0, 0.7316f);

        //foreach (var gameObject in _customGameObjects)
        //{
        //  Log.Trace(Main.Instance, $"{gameObject.Key}:{gameObject.Value}");
        //}

        //var talkerGo = _customGameObjects[Digitalroot.Valheim.Common.Utils.Localize("$op_odin_emissary")] as OdinsEmissaryCustomGameObject;
        //Log.Trace(Main.Instance, $"[{GetType().Name}] talkerGo == null : {talkerGo == null}");

        //var cmb = customGameObject.Cauldron.GetComponent<OdinsCauldronCustomMonoBehaviour>();
        //Log.Trace(Main.Instance, $"[{GetType().Name}] talkerGo == null : {talkerGo == null}");
        //Log.Trace(Main.Instance, $"[{GetType().Name}] talkerGo.OdinsEmissary == null : {talkerGo?.OdinsEmissary == null}");
        // cmb.Talker = talkerGo?.OdinsEmissary;
        // m_odinPot = caul.AddComponent<OdinTradeShop>();
        // OdinPlus.traderNameList.Add(m_odinPot.m_name);
        // _customGameObjects.Add(customGameObject.Name, customGameObject);
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void UpdateOdinsCauldron()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var cauldronCmb = _odinsCauldron.GetComponent<OdinsCauldronCustomMonoBehaviour>();
      var emissaryCmb = _odinsEmissary.GetComponent<OdinsEmissaryCustomMonoBehaviour>();
      Log.Trace(Main.Instance, $"[{GetType().Name}] cauldronCmb == null : {cauldronCmb == null}");
      Log.Trace(Main.Instance, $"[{GetType().Name}] emissaryCmb == null : {emissaryCmb == null}");
      cauldronCmb.Talker = emissaryCmb.Talker;
      cauldronCmb.Head = emissaryCmb.Head;
    }

    private void AddOdinsEmissary()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        _odinsEmissary = PrefabManager.Instance.CreateClonedPrefab(PrefabNames.OdinsCampEmissary, PrefabNames.OrnamentalEmissary);
        _odinsEmissary.AddMonoBehaviour<OdinsEmissaryCustomMonoBehaviour>();
        _odinsEmissary.transform.SetParent(_odinCamp.transform);
        _odinsEmissary.transform.localPosition = new Vector3(0.6f, -0.045f, 2.1f);
        _odinsEmissary.transform.localRotation = new Quaternion(0, 0.6605f, 0, 0.7508f);
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void AddOdinsFirePit()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        _odinsFirePit = PrefabManager.Instance.CreateClonedPrefab(PrefabNames.OdinsCampFirePit, PrefabNames.OrnamentalFirePit);
        _odinsFirePit.AddMonoBehaviour<UnRemoveableCustomMonoBehaviour>();
        _odinsFirePit.transform.SetParent(_odinCamp.transform);
        _odinsFirePit.transform.localPosition = new Vector3(-0.42f, -0.015f, -1f);
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void AddOdinsForceField()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var forceFieldPreFab = ZoneSystem.instance.m_locations[85].m_prefab.transform.Find(Digitalroot.Valheim.Common.Names.PrefabNames.ForceField);
        _odinsForceField = Object.Instantiate(forceFieldPreFab, _odinCamp.transform);
        _odinsForceField.transform.localScale = Vector3.one * 11f;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void AddOdinsMunin()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        _odinsMunin = PrefabManager.Instance.CreateClonedPrefab(PrefabNames.OdinsCampMunin, PrefabNames.OrnamentalMunin);
        _odinsMunin.AddMonoBehaviour<MuninCustomMonoBehaviour>();
        _odinsMunin.transform.SetParent(_odinCamp.transform);
        _odinsMunin.transform.localPosition = new Vector3(2.56f, -0.001f, -1.98f);
        _odinsMunin.transform.localRotation = new Quaternion(0, 0.345f, 0, -0.9386f);
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    private void AddOdinsShaman()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        _odinsShaman = PrefabManager.Instance.CreateClonedPrefab(PrefabNames.OdinsCampGoblinShaman, PrefabNames.OrnamentalFatTroll);
        _odinsShaman.AddMonoBehaviour<ShamanCustomMonoBehaviour>();
        _odinsShaman.transform.SetParent(_odinCamp.transform);
        _odinsShaman.transform.localPosition = new Vector3(-1.7409f, -0.008f, 0.2318f);
        _odinsShaman.transform.localRotation = new Quaternion(0, 0.8026f, 0, 0.5965f);
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }

    public Vector3 GetOdinCampLocation()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (_odinCamp != null)
      {
        if (_odinCamp.transform != null)
        {
          return _odinCamp.transform.position;
        }

        Log.Trace(Main.Instance, $"[{GetType().Name}] _odinCamp.transform != null : {_odinCamp.transform != null}");
      }

      Log.Trace(Main.Instance, $"[{GetType().Name}] _odinCamp != null : {_odinCamp != null}");
      return Vector3.zero;
    }

    private void SetSpawnPosition()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _odinCamp.transform.localPosition = Digitalroot.Valheim.Common.Utils.GetStartTemplesPosition() + new Vector3(-12.6f, 0.05f, -11f);
    }

    #region Implementation of IDestroyable

    /// <inheritdoc />
    public void OnDestroy()
    {
      // Log.Debug(Main.Instance, $"[{GetType().Name}] Total Spawnables ({_customGameObjects.Count}) ");
      // foreach (var spawnable in _customGameObjects)
      // {
      //   try
      //   {
      //     Log.Debug(Main.Instance, $"[{GetType().Name}] Destroying : {spawnable.Key}");
      //     spawnable.Value.OnDestroy();
      //   }
      //   catch (Exception e)
      //   {
      //     Log.Error(Main.Instance, e);
      //   }
      // }
    }

    #endregion
  }
}
