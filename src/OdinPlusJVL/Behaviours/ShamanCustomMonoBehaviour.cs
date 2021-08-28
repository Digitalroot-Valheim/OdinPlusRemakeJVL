using Digitalroot.Valheim.Common;
using HarmonyLib;
using JetBrains.Annotations;
using OdinPlusJVL.Common.Interfaces;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace OdinPlusJVL.Behaviours
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
      StringBuilder n = new StringBuilder($"<color=lightblue><b>$op_shaman_name</b></color>")
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
      Say(Talker, "$op_shaman_name",  "$op_shaman_no");
      return true;
    }

    #endregion
  }
}
