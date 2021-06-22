﻿using HarmonyLib;
using JetBrains.Annotations;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;

namespace OdinPlusRemakeJVL
{
  public class Patch
  {
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

          // ToDo: Create an event for this that triggers off of Main.cs
          FxAssetManager.Instance.Initialize();
          FxAssetManager.Instance.PostInitialize();

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
  }
}
