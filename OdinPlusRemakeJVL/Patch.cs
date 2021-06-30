using HarmonyLib;
using JetBrains.Annotations;
using OdinPlusRemakeJVL.Common;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using OdinPlusRemakeJVL.Npcs;
using OdinPlusRemakeJVL.Pieces;
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

    #region StoreGui

    [HarmonyPatch(typeof(StoreGui))]
    public static class PatchStoreGui
    {
      [HarmonyPostfix]
      [HarmonyPatch("Show")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixShow(StoreGui __instance, Trader trader)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (Main.traderNameList.Contains(trader.m_name))
          {
            OdinTradeShop.TweakGui(__instance, true);
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
          __instance.Hide();
        }
      }

      [HarmonyPostfix]
      [HarmonyPatch("Hide")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixHide(StoreGui __instance)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (Main.traderNameList.Contains(__instance.m_trader.m_name))
          {
            OdinTradeShop.TweakGui(__instance, false);
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }

      [HarmonyPostfix]
      [HarmonyPatch("GetPlayerCoins")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      [SuppressMessage("ReSharper", "InconsistentNaming")]
      public static void PostfixGetPlayerCoins(StoreGui __instance, ref int __result)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (__instance.m_trader == null) return;

          if (Main.traderNameList.Contains(__instance.m_trader.m_name))
          {
            __result = Main.Credits;
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }

      [HarmonyPrefix]
      [HarmonyPatch("GetPlayerCoins")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static bool PrefixBuySelectedItem(StoreGui __instance)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          // string name = Traverse.Create(__instance).Field<Trader>("m_trader").Value.m_name;
          var name = __instance.m_trader.m_name;
          if (!Main.traderNameList.Contains(name)) return true;

          // var selectedItem = Traverse.Create(__instance)?.Field<Trader.TradeItem>("m_selectedItem")?.Value;
          var selectedItem = __instance.m_selectedItem;
          if (selectedItem == null) return false;

          int stack = Mathf.Min(selectedItem.m_stack, selectedItem.m_prefab.m_itemData.m_shared.m_maxStackSize);
          if (selectedItem.m_price * stack - Main.Credits > 0) return false;

          int quality = selectedItem.m_prefab.m_itemData.m_quality;
          int variant = selectedItem.m_prefab.m_itemData.m_variant;

          if (Player.m_localPlayer.GetInventory().AddItem(selectedItem.m_prefab.name, stack, quality, variant, 0L, "") != null)
          {
            Main.RemoveCredits(selectedItem.m_price * stack); //?
            __instance.m_buyEffects.Create(__instance.gameObject.transform.position, Quaternion.identity);
            Player.m_localPlayer.ShowPickupMessage(selectedItem.m_prefab.m_itemData, selectedItem.m_prefab.m_itemData.m_stack);
            // Traverse.Create(__instance)?.Method("FillList")?.GetValue();
            __instance.FillList();
            Gogan.LogEvent("Game", "BoughtItem", selectedItem.m_prefab.name, 0L);
          }

          return false;
        }
        catch (Exception e)
        {
          Log.Fatal(e);
          return true;
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
