using HarmonyLib;
using JetBrains.Annotations;
using OdinPlus.Common;
using OdinPlus.Data;
using OdinPlus.Managers;
using OdinPlus.Npcs;
using OdinPlus.Quests;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using UnityEngine;

namespace OdinPlus
{
  public static class Patch
  {
    public static bool CheckPlayerNull()
    {
      if (Player.m_localPlayer != null) return false;
      // Log.Trace("Player is Null"); // Spammy
      return true;
    }

    #region Character

    [HarmonyPatch(typeof(Character))]
    public static class PatchCharacter
    {
      [HarmonyPrefix]
      [HarmonyPatch("GetHoverText")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable InconsistentNaming
      public static bool PrefixGetHoverText(Character __instance, ref string __result)
        // ReSharper restore InconsistentNaming
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          Component component = __instance.GetComponent<HumanNpc>();
          if (!component) return true;
          __result = ((HumanNpc) component).GetHoverText();
          return false;
        }
        catch (Exception e)
        {
          Log.Fatal(e);
          return false;
        }
      }
    }

    #endregion

    #region Chat

    [HarmonyPatch(typeof(Chat))]
    public static class PatchChat
    {
      [HarmonyPrefix]
      [HarmonyPatch("InputText")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PrefixStart(Chat __instance)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (Player.m_localPlayer == null || !OdinPlus.IsNPCInitialized) return;

          switch (__instance.m_input.text.ToLower())
          {
            // ReSharper disable StringLiteralTypo
            case "/findfarm":
              FindFarmCommand();
              break;

            case "/odinhere":
              OdinHereCommand();
              break;

            case "/setodin":
              SetOdinCommand();
              break;

            case "/whereami":
              WhereAmICommand(__instance);
              break;

            case "/whereodin":
              WhereOdinCommand(__instance);
              break;

            // ReSharper restore StringLiteralTypo
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }

      // ReSharper disable once InconsistentNaming
      private static void ReportPosition(Chat __instance, Vector3 pos)
      {
        var s = $"{pos.x},{pos.y},{pos.z}";
        DBG.InfoCT(s);
        DBG.cprt(s);
        __instance.m_input.text = s;
      }

      private static void FindFarmCommand()
      {
        Game.instance.DiscoverClosestLocation("WoodFarm1", Player.m_localPlayer.transform.position, "Village", 0);
      }

      private static void OdinHereCommand()
      {
        if (Main.Set_FOP)
        {
          LocationManager.GetStartPos();
          return;
        }

        NpcManager.Root.transform.localPosition = Player.m_localPlayer.transform.localPosition + Vector3.forward * 4;
      }

      private static void SetOdinCommand()
      {
        Main.ConfigEntryOdinPosition.Value = NpcManager.Root.transform.localPosition;
      }

      // ReSharper disable once InconsistentNaming
      private static void WhereAmICommand(Chat __instance)
      {
        ReportPosition(__instance, Player.m_localPlayer.transform.position);
      }

      // ReSharper disable once InconsistentNaming
      private static void WhereOdinCommand(Chat __instance)
      {
        ReportPosition(__instance, NpcManager.Root.transform.localPosition);
      }
    }

    #endregion

    #region Console

    [HarmonyPatch(typeof(Console))]
    public static class PatchConsole
    {
      [HarmonyPrefix]
      [HarmonyPatch("InputText")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PrefixStart()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (DevTool.IsInitialized)
          {
            DevTool.InputCMD(Console.instance.m_input.text);
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region Container

    [HarmonyPatch(typeof(Container))]
    public static class PatchContainer
    {
      [HarmonyPostfix]
      [HarmonyPatch("Interact")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixInteract(Container __instance, Humanoid character, bool hold)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          var legacyChest = __instance.GetComponent<LegacyChest>();
          if (legacyChest)
          {
            legacyChest.OnOpen(character, hold);
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region DungeonGenerator

    [HarmonyPatch(typeof(DungeonGenerator))]
    public static class PatchDungeonGenerator
    {
      [HarmonyPostfix]
      [HarmonyPatch("Awake")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixAwake(DungeonGenerator __instance)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          if (__instance.GetComponent<ZNetView>())
          {
            __instance.gameObject.AddComponent<LocationMarker>();
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region FejdStartup

    [HarmonyPatch(typeof(FejdStartup))]
    public static class PatchFejdStartup
    {
      [HarmonyPostfix]
      [HarmonyPatch("Start")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PostfixStart()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (ZNetScene.instance == null || ZNetScene.instance.m_prefabs == null || ZNetScene.instance.m_prefabs.Count <= 0)
          {
            Log.Debug("ZNetScene.instance is null");
            return;
          }

          if (!Util.IsObjectDBReady())
          {
            Log.Debug("ObjectDB not ready - skipping");
            return;
          }

          if (OdinPlus.IsInitialized) return;
          OdinPlus.Initialize();
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region Localization

    [HarmonyPatch(typeof(Localization))]
    public static class PatchLocalization
    {
      [HarmonyPostfix]
      [HarmonyPatch("SetupLanguage")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixSetupLanguage(Localization __instance, string language)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          Log.Trace(language);
          BuzzLocal.Initialize(language, __instance);
          BuzzLocal.UpdateDictionary();
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region LocationProxy

    [HarmonyPatch(typeof(LocationProxy))]
    public static class PatchLocationProxy
    {
      [HarmonyPostfix]
      [HarmonyPatch("Awake")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixAwake(LocationProxy __instance)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (__instance.GetComponentInChildren<DungeonGenerator>(true) != null) return;
          __instance.gameObject.AddComponent<LocationMarker>();
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region ObjectDB

    [HarmonyPatch(typeof(ObjectDB))]
    public static class PatchObjectDB
    {
      [HarmonyPostfix]
      [HarmonyPatch("Awake")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PostfixAwake()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (!Util.IsObjectDBReady())
          {
            Log.Debug("ObjectDB not ready - skipping");
            return;
          }

          if (!OdinPlus.IsInitialized)
          {
            OdinPlus.Initialize();
          }
          OdinPlus.ValRegister(ObjectDB.instance);
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region Player

    [HarmonyPatch(typeof(Player))]
    public static class PatchPlayer
    {
      [HarmonyPostfix]
      [HarmonyPatch("Update")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PostfixUpdate(Player __instance)
      {
        try
        {
          if (CheckPlayerNull())
          {
            return;
          }

          // Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()"); // Spammy

          if (!Main.KeyboardShortcutSecondInteractKey.Value.IsDown() || __instance.GetHoverObject() == null) return;

          if (__instance.GetHoverObject().GetComponent<IOdinInteractable>() != null)
          {
            __instance.GetHoverObject().GetComponent<IOdinInteractable>().SecondaryInteract(__instance);
            return;
          }

          if (__instance.GetHoverObject().GetComponentInParent<IOdinInteractable>() != null)
          {
            __instance.GetHoverObject().GetComponentInParent<IOdinInteractable>().SecondaryInteract(__instance);
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region PlayerProfile

    [HarmonyPatch(typeof(PlayerProfile))]
    public static class PatchPlayerProfile
    {
      [HarmonyPrefix]
      [HarmonyPatch("SavePlayerToDisk")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PrefixSavePlayerToDisk()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (CheckPlayerNull()) return;
          OdinData.SaveOdinData($"{Player.m_localPlayer.GetPlayerName()}_{ZNet.instance.GetWorldName()}");
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }

      [HarmonyPostfix]
      [HarmonyPatch("LoadPlayerData")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PostfixLoadPlayerData()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (ZNet.instance == null || CheckPlayerNull() || OdinPlus.Instance.IsLoaded)
          {
            Log.Trace("ZNetScene.instance is null");
            return;
          }
          OdinData.LoadOdinData($"{Player.m_localPlayer.GetPlayerName()}_{ZNet.instance.GetWorldName()}");
        }
        catch (Exception e)
        {
          Log.Fatal(e);
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

          if (OdinPlus.traderNameList.Contains(trader.m_name))
          {
            OdinTrader.TweakGui(__instance, true);
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

          if (OdinPlus.traderNameList.Contains(__instance.m_trader.m_name))
          {
            OdinTrader.TweakGui(__instance, false);
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

          if (OdinPlus.traderNameList.Contains(__instance.m_trader.m_name))
          {
            __result = OdinData.Credits;
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
          if (!OdinPlus.traderNameList.Contains(name)) return true;

          // var selectedItem = Traverse.Create(__instance)?.Field<Trader.TradeItem>("m_selectedItem")?.Value;
          var selectedItem = __instance.m_selectedItem;
          if (selectedItem == null) return false;

          int stack = Mathf.Min(selectedItem.m_stack, selectedItem.m_prefab.m_itemData.m_shared.m_maxStackSize);
          if (selectedItem.m_price * stack - OdinData.Credits > 0) return false;

          int quality = selectedItem.m_prefab.m_itemData.m_quality;
          int variant = selectedItem.m_prefab.m_itemData.m_variant;

          if (Player.m_localPlayer.GetInventory().AddItem(selectedItem.m_prefab.name, stack, quality, variant, 0L, "") != null)
          {
            OdinData.RemoveCredits(selectedItem.m_price * stack); //?
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

    #region Tameable

    [HarmonyPatch(typeof(Tameable))]
    public static class PatchTameable
    {
      [HarmonyPostfix]
      [HarmonyPatch("GetHoverText")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable InconsistentNaming
      public static void PostfixGetHoverText(Tameable __instance, ref string __result)
        // ReSharper restore InconsistentNaming
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          if (__instance.gameObject.GetComponent<Character>().m_name == "$op_wolf_name")
          {
            __result += Localization.instance.Localize($"\n<color=yellow><b>[{Main.KeyboardShortcutSecondInteractKey.Value.MainKey}]</b></color>$op_wolf_use");
          }
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }
    }

    #endregion

    #region ZNetScene

    [HarmonyPatch(typeof(ZNetScene))]
    public static class PatchZNetScene
    {
      [HarmonyPrefix]
      [HarmonyPatch("Awake")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      // ReSharper disable once InconsistentNaming
      public static void PrefixAwake(ZNetScene __instance)
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          if (__instance == null || __instance.m_prefabs == null || __instance.m_prefabs.Count <= 0)
          {
            Log.Debug("ZNetScene.instance is null");
            return;
          }

          if (!Util.IsObjectDBReady())
          {
            Log.Debug("ObjectDB not ready - skipping");
            return;
          }

          if (!OdinPlus.IsInitialized) OdinPlus.Initialize();
          OdinPlus.PreZNetSceneHook(__instance);
        }
        catch (Exception e)
        {
          Log.Fatal(e);
        }
      }

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
          if (__instance == null || __instance.m_prefabs == null || __instance.m_prefabs.Count <= 0)
          {
            Log.Debug("ZNetScene.instance is null");
            return;
          }

          if (!Util.IsObjectDBReady())
          {
            Log.Debug("ObjectDB not ready - skipping");
            return;
          }

          if (!OdinPlus.IsInitialized) OdinPlus.Initialize();
          OdinPlus.PreZNetSceneHook(__instance);
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
          if (ZNet.instance.IsServer() && ZNet.instance.IsDedicated())
          {
            OdinData.SaveOdinData(ZNet.instance.GetWorldName());
          }

          OdinPlus.UnRegister();
          OdinPlus.Clear();
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
      [HarmonyPatch("Start")]
      [HarmonyPriority(Priority.Normal)]
      [UsedImplicitly]
      public static void PostfixStart()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
          Main.PostZoneAction?.Invoke();
          // if (Main.PostZoneAction != null)
          // {
          //   Main.PostZoneAction();
          // }

          OdinPlus.PostZone();
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
      public static void PostfixAwake()
      {
        try
        {
          Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

          Main.RegisterRpcAction();
          LocationManager.RequestServerFop();
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
