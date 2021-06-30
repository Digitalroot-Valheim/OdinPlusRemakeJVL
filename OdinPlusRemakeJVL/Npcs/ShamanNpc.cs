using HarmonyLib;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using OdinPlusRemakeJVL.Pieces;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public class ShamanNpc : AbstractNpc
  {
    public GameObject Shaman => GameObjectInstance;

    public void Awake()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Name = "$op_shaman";
        GameObjectInstance = Instantiate(PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalGoblinShaman));
        Talker = Shaman;
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    private void Start()
    {
      var prefab = Shaman;
      ZNetView znv = prefab.GetComponent<ZNetView>();
      ZDO zdo = prefab.GetComponent<ZNetView>().GetZDO();
      DestroyImmediate(prefab.GetComponent<ZNetView>());
      DestroyImmediate(prefab.GetComponent<ZSyncAnimation>());
      DestroyImmediate(prefab.GetComponent<ZSyncTransform>());
      DestroyImmediate(prefab.GetComponent<MonsterAI>());
      // DestroyImmediate(prefab.GetComponent<VisEquipment>());
      DestroyImmediate(prefab.GetComponent<CharacterDrop>());
      DestroyImmediate(prefab.GetComponent<Humanoid>());
      DestroyImmediate(prefab.GetComponent<FootStep>());
      DestroyImmediate(prefab.GetComponent<Rigidbody>());
      foreach (var comp in gameObject.GetComponents<Component>())
      {
        if (!(comp is Transform) && !(comp is ShamanNpc) && !(comp is CapsuleCollider) && !(comp is OdinsFirePit) && !(comp is OdinsCauldron) && !(comp is OdinNpc) && !(comp is MuninNpc))
        {
          Log.Trace($"[{GetType().Name}] DestroyImmediate({comp.name} - {comp.GetType().Name})");
          DestroyImmediate(comp);
        }
      }
      var a = Traverse.Create(ZNetScene.instance).Field<Dictionary<ZDO, ZNetView>>("m_instances").Value;
      a.Remove(zdo);
      ZDOMan.instance.DestroyZDO(zdo);
    }


    public override string GetHoverName() => Name;
     
    public override string GetHoverText()
    {
      StringBuilder n = new StringBuilder($"<color=lightblue><b>{Name}</b></color>")
          // .Append($"\n<color=green><b>Credits:{OdinData.Credits}</b></color>")
          // .Append($"\n[<color=yellow><b>$KEY_Use</b></color>] $op_buy")
          .Append("\n[<color=yellow><b>1-8</b></color>]$op_shaman_offer")
        // .Append($"\n<color=yellow><b>[{Plugin.KeyboardShortcutSecondInteractKey.Value.MainKey}]</b></color>$op_shaman_use")
        ;
      return Localization.instance.Localize(n.ToString());

    }

    public override bool Interact(Humanoid user, bool hold)
    {
      if (hold)
      {
        return false;
      }
      return true;
    }

    public override bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      //var name = item.m_dropPrefab.name;
      //if (GoodsList.ContainsKey(name))
      //{
      //  var gd = GoodsList[name];
      //  if (item.m_stack >= gd.Value)
      //  {
      //    var goodItemData = OdinItemManager.Instance.GetItemData(gd.Good);
      //    if (user.GetInventory().AddItem(goodItemData))
      //    {
      //      user.GetInventory().RemoveItem(item, gd.Value);
      //      Say(goodItemData.m_shared.m_description);
      //      return true;
      //    }
      //    DBG.InfoCT("$op_inventory_full");
      //    return true;
      //  }
      //  Say("$op_shaman_notenough");
      //  return true;
      //}
      Say("$op_shaman_no");
      return true;
    }
  }
}
