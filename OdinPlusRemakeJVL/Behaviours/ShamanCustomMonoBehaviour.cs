using HarmonyLib;
using JetBrains.Annotations;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace OdinPlusRemakeJVL.Behaviours
{
  public class ShamanCustomMonoBehaviour : AbstractCustomMonoBehaviour, ITalkable, Hoverable, Interactable
  {
    [UsedImplicitly]
    public void Awake()
    {
      Talker = gameObject;
    }

    [UsedImplicitly]
    public void Start()
    {
      ZNetView znv = gameObject.GetComponent<ZNetView>();
      ZDO zdo = gameObject.GetComponent<ZNetView>().GetZDO();
      DestroyImmediate(gameObject.GetComponent<ZNetView>());
      DestroyImmediate(gameObject.GetComponent<ZSyncAnimation>());
      DestroyImmediate(gameObject.GetComponent<ZSyncTransform>());
      DestroyImmediate(gameObject.GetComponent<MonsterAI>());
      DestroyImmediate(gameObject.GetComponent<VisEquipment>());
      DestroyImmediate(gameObject.GetComponent<CharacterDrop>());
      DestroyImmediate(gameObject.GetComponent<Humanoid>());
      DestroyImmediate(gameObject.GetComponent<FootStep>());
      DestroyImmediate(gameObject.GetComponent<Rigidbody>());
      foreach (var comp in gameObject.GetComponents<Component>())
      {
        if (!(comp is Transform) && !(comp is ShamanCustomMonoBehaviour) && !(comp is CapsuleCollider)
          //&& !(comp is OdinsFirePitCustomGameObject) && !(comp is OdinsCauldronCustomGameObject) && !(comp is OdinNpc) && !(comp is MuninNpc)
        )
        {
          Log.Trace($"[{GetType().Name}] DestroyImmediate({comp.name} - {comp.GetType().Name})");
          DestroyImmediate(comp);
        }
      }
      var a = Traverse.Create(ZNetScene.instance).Field<Dictionary<ZDO, ZNetView>>("m_instances").Value;
      a.Remove(zdo);
      ZDOMan.instance.DestroyZDO(zdo);
    }

    #region Implementation of ITalkable

    /// <inheritdoc />
    public Transform Head { get; set; }

    /// <inheritdoc />
    public GameObject Talker { get; set; }

    /// <inheritdoc />
    public void Say(string topic, string msg) => Say(Talker, topic, msg);

    #endregion

    #region Implementation of Hoverable

    /// <inheritdoc />
    public string GetHoverText()
    {
      StringBuilder n = new StringBuilder($"<color=lightblue><b>{gameObject.name}</b></color>")
          // .Append($"\n<color=green><b>Credits:{OdinData.Credits}</b></color>")
          // .Append($"\n[<color=yellow><b>$KEY_Use</b></color>] $op_buy")
          .Append("\n[<color=yellow><b>1-8</b></color>]$op_shaman_offer")
        // .Append($"\n<color=yellow><b>[{Plugin.KeyboardShortcutSecondInteractKey.Value.MainKey}]</b></color>$op_shaman_use")
        ;
      return Localization.instance.Localize(n.ToString());
    }

    /// <inheritdoc />
    public string GetHoverName() => gameObject.name;

    #endregion

    #region Implementation of Interactable

    /// <inheritdoc />
    public bool Interact(Humanoid user, bool hold)
    {
      if (hold)
      {
        return false;
      }
      return true;
    }

    /// <inheritdoc />
    public bool UseItem(Humanoid user, ItemDrop.ItemData item)
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
      Say(Talker, gameObject.name,  "$op_shaman_no");
      return true;
    }

    #endregion
  }
}
