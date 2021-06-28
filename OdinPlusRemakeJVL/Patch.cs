using HarmonyLib;
using JetBrains.Annotations;
using OdinPlusRemakeJVL.Common;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL
{
  public class Patch
  {
    #region Game

    [HarmonyPatch(typeof(Game))]
    public static class PatchGame
    {
      [HarmonyPostfix]
      [HarmonyPatch("SpawnPlayer")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PostfixLoad(Vector3 spawnPoint)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}({spawnPoint})");

          if (Player.m_localPlayer == null)
          {
            Log.Debug("Player is null");
            return;
          }
          Main.Instance.OnSpawnedPlayer(spawnPoint);
        }
        catch (Exception e)
        {
          ZLog.LogError(e);
        }
      }
    }

    #endregion

    #region ZNetScene

    [HarmonyPatch(typeof(ZNetScene))]
    public static class PatchZNetScene
    {
      [HarmonyPostfix]
      [HarmonyPatch("Awake")]
      [HarmonyPriority(600)]
      [HarmonyBefore("buzz.valheim.AllTameable", "org.bepinex.plugins.creaturelevelcontrol")]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixAwake(ZNetScene __instance)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          if (!Common.Utils.IsZNetSceneReady())
          {
            Log.Debug("ZNetScene.instance is null");
            return;
          }

          Main.Instance.OnZNetSceneReady(__instance);
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }

      [HarmonyPostfix]
      [HarmonyPatch("Shutdown")]
      [UsedImplicitly]
      public static void PostfixShutdown()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          // if (ZNet.instance.IsServer() && ZNet.instance.IsDedicated())
          // {
          //   OdinData.SaveOdinData(ZNet.instance.GetWorldName());
          // }
          //
          // OdinPlus.UnRegister();
          // OdinPlus.Clear();
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region ZNet

    [HarmonyPatch(typeof(ZNet))]
    public static class PatchZNet
    {
      [HarmonyPostfix]
      [HarmonyPatch("Awake")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixAwake(ZNet __instance)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          if (!Common.Utils.IsZNetSceneReady())
          {
            Log.Debug("ZNet.instance is null");
            return;
          }

          Main.Instance.OnZNetReady(__instance);
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region ZoneSystem

    [HarmonyPatch(typeof(ZoneSystem))]
    public static class PatchZoneSystem
    {
      [HarmonyPostfix]
      [HarmonyPatch("Load")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PostfixLoad()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          Main.Instance.OnZoneSystemLoaded();
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

  }
}
