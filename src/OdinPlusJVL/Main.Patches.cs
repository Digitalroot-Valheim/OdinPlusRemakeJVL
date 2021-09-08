using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using OdinPlusJVL.Common.EventArgs;
using OdinPlusJVL.Extensions;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL
{
  public partial class Main
  {
    #region ZNetScene

    public void OnZNetSceneReady(ref ZNetScene zNetScene)
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug(Instance, $"[{GetType().Name}] Calling ZNetSceneReady Subscribers");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnZNetSceneReady");
        foreach (var onZNetSceneReady in _managersList.Select(manager => manager as IOnZNetSceneReady))
        {
          try
          {
            onZNetSceneReady?.OnZNetSceneReady(zNetScene);
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }

        //_NetSceneRoot.keeper_prefab(Clone)
        while (zNetScene?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)") != null)
        {
          var keeper = zNetScene?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)");
          if (keeper != null)
          {
            Log.Error(Instance, $"Found {keeper?.name}");
            DestroyImmediate(keeper);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    public void OnZNetSceneShutdown(ref ZNetScene zNetScene)
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IDestroyable");
        foreach (var destroyable in _managersList.Select(manager => manager as IDestroyable))
        {
          try
          {
            destroyable?.OnDestroy();
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }

        //_NetSceneRoot.keeper_prefab(Clone)
        while (zNetScene?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)") != null)
        {
          var keeper = zNetScene?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)");
          if (keeper != null)
          {
            Log.Error(Instance, $"Found {keeper?.name}");
            DestroyImmediate(keeper);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    #endregion

    #region ZNetReady

    public void OnZNetReady(ref ZNet zNet)
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug(Instance, $"[{GetType().Name}] Calling ZNetReady Subscribers");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnZNetReady");
        foreach (var onZNetReady in _managersList.Select(manager => manager as IOnZNetReady))
        {
          try
          {
            onZNetReady?.OnZNetReady(zNet);
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    #endregion

    #region ZoneSystemLoaded

    public void OnZoneSystemLoaded()
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      RootObject.SetActive(true);

      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnZoneSystemLoaded");
        foreach (var onZoneSystemLoaded in _managersList.Select(manager => manager as IOnZoneSystemLoaded))
        {
          try
          {
            onZoneSystemLoaded?.OnZoneSystemLoaded();
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    #endregion

    #region SpawnedPlayer

    public void OnSpawnedPlayer(Vector3 spawnPoint)
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug(Instance, $"[{GetType().Name}] Calling SpawnedPlayer Subscribers");
      try
      {
        Log.Debug(Instance, $"[{GetType().Name}] Calling Managers with IOnSpawnedPlayer");
        foreach (var onSpawnedPlayer in _managersList.Select(manager => manager as IOnSpawnedPlayer))
        {
          try
          {
            onSpawnedPlayer?.OnSpawnedPlayer(spawnPoint);
          }
          catch (Exception e)
          {
            Log.Error(Instance, e);
          }
        }

        //_NetSceneRoot.keeper_prefab(Clone)
        while (ZNetScene.instance?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)") != null)
        {
          var keeper = ZNetScene.instance?.m_netSceneRoot?.FindGameObject("keeper_prefab(Clone)");
          if (keeper != null)
          {
            Log.Error(Instance, $"Found {keeper?.name}");
            DestroyImmediate(keeper);
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Instance, e);
      }
    }

    #endregion
  }
}
