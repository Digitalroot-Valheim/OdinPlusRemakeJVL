using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using OdinPlusJVL.GameObjects;
using System;
using System.Reflection;
using OdinPlusJVL.Managers;
using UnityEngine;

namespace OdinPlusJVL
{
  [UsedImplicitly]
  public class Patch
  {
    #region Game

    [HarmonyPatch(typeof(Game), nameof(Game.SpawnPlayer))]
    public static class PatchGameSpawnPlayer
    {
      [UsedImplicitly]
      [HarmonyPostfix]
      [HarmonyPriority(Priority.Normal)]
      public static void Postfix(Vector3 spawnPoint)
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}({spawnPoint})");

          if (!Digitalroot.Valheim.Common.Utils.IsPlayerReady())
          {
            Log.Debug(Main.Instance, "Player is null");
            return;
          }
          Main.Instance.OnSpawnedPlayer(spawnPoint);
        }
        catch (Exception e)
        {
          Log.Error(Main.Instance, e);
        }
      }
    }

    #endregion

    #region StoreGui

    [HarmonyPatch(typeof(StoreGui))]
    public static class PatchStoreGui
    {
      [HarmonyPostfix]
      [HarmonyPatch(nameof(StoreGui.Show))]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixShow(ref StoreGui __instance, ref Trader trader)
      {
        return;
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (Main.traderNameList.Contains(trader.m_name))
          {
            OdinTradeShop.TweakGui(__instance, true);
          }
        }
        catch (Exception e)
        {
          Log.Fatal(Main.Instance, e);
          __instance.Hide();
        }
      }

      [HarmonyPostfix]
      [HarmonyPatch(nameof(StoreGui.Hide))]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixHide(ref StoreGui __instance)
      {
        return;
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (Main.traderNameList.Contains(__instance.m_trader.m_name))
          {
            OdinTradeShop.TweakGui(__instance, false);
          }
        }
        catch (Exception e)
        {
          Log.Fatal(Main.Instance, e);
        }
      }

      [HarmonyPostfix]
      [HarmonyPatch(nameof(StoreGui.GetPlayerCoins))]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable InconsistentNaming
      public static void PostfixGetPlayerCoins(ref StoreGui __instance, ref int __result)
        // ReSharper restore InconsistentNaming
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (__instance.m_trader == null) return;

          if (Main.traderNameList.Contains(__instance.m_trader.m_name))
          {
            __result = Main.Credits;
          }
        }
        catch (Exception e)
        {
          Log.Fatal(Main.Instance, e);
        }
      }

      [HarmonyPrefix]
      [HarmonyPatch(nameof(StoreGui.GetPlayerCoins))]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static bool PrefixBuySelectedItem(StoreGui __instance)
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

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
          Log.Fatal(Main.Instance, e);
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
      [HarmonyPatch(nameof(ZNetScene.Awake))]
      [HarmonyPriority(600)]
      [HarmonyBefore("buzz.valheim.AllTameable", "org.bepinex.plugins.creaturelevelcontrol")]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixAwake(ref ZNetScene __instance)
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          if (!Digitalroot.Valheim.Common.Utils.IsZNetSceneReady())
          {
            Log.Debug(Main.Instance, "ZNetScene.instance is null");
            return;
          }

          Main.Instance.OnZNetSceneReady(ref __instance);
        }
        catch (Exception e)
        {
          Log.Fatal(Main.Instance, e);
        }
      }

      [HarmonyPrefix]
      [HarmonyPatch(nameof(ZNetScene.Shutdown))]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PrefixShutdown(ref ZNetScene __instance)
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          // if (ZNet.instance.IsServer() && ZNet.instance.IsDedicated())
          // {
          //   OdinData.SaveOdinData(ZNet.instance.GetWorldName());
          // }
          Main.Instance.OnZNetSceneShutdown(ref __instance);
          // OdinPlus.UnRegister();
          // OdinPlus.Clear();
        }
        catch (Exception e)
        {
          Log.Fatal(Main.Instance, e);
        }
      }
    }

    #endregion

    #region ZNet

    [HarmonyPatch(typeof(ZNet))]
    public static class PatchZNet
    {
      [UsedImplicitly]
      [HarmonyPostfix]
      [HarmonyPatch(nameof(ZNet.Awake))]
      [HarmonyPriority(Priority.Normal)]
      // ReSharper disable once InconsistentNaming
      public static void PostfixAwake(ref ZNet __instance)
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          if (!Digitalroot.Valheim.Common.Utils.IsZNetReady())
          {
            Log.Debug(Main.Instance, "ZNet.instance is null");
            return;
          }

          Main.Instance.OnZNetReady(ref __instance);
        }
        catch (Exception e)
        {
          Log.Fatal(Main.Instance, e);
        }
      }
    }

    #endregion

    #region ZoneSystem

    [HarmonyPatch(typeof(ZoneSystem))]
    public static class PatchZoneSystem
    {
      [HarmonyPostfix]
      [HarmonyPatch(nameof(ZoneSystem.Load))]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PostfixLoad()
      {
        try
        {
          Log.Trace(Main.Instance, $"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          Main.Instance.OnZoneSystemLoaded();
        }
        catch (Exception e)
        {
          Log.Fatal(Main.Instance, e);
        }
      }
    }

    #endregion

    #region StoreGuiReplacement

    [HarmonyPatch(typeof(StoreGui), nameof(StoreGui.Awake))]
    public static class StoreSwapPatch
    {
      public static void Postfix(StoreGui __instance)
      {
        Main.Instantiate(PrefabManager.CustomTrader, __instance.GetComponentInParent<GameObject>().transform, false);
      }
    }

    #endregion

  }
}
