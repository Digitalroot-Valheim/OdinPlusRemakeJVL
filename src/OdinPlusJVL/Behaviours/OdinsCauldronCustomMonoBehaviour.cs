﻿using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Common.Interfaces;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OdinPlusJVL.Behaviours
{
  public class OdinsCauldronCustomMonoBehaviour : AbstractCustomMonoBehaviour, ITalkable, Hoverable, Interactable
  {
    [UsedImplicitly]
    public void Start()
    {
      gameObject.transform.Find("HaveFire").gameObject.SetActive(true);
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
      StringBuilder stringBuilder = new StringBuilder($"<color=lightblue><b>{gameObject.name}</b></color>");
      // string s = string.Format("\n<color=lightblue><b>$op_crd:{0}</b></color>", OdinData.Credits);
      // string a = string.Format("\n[<color=yellow><b>$KEY_Use</b></color>] $op_use[<color=green><b>{0}</b></color>]", cskill);
      // string b = "\n[<color=yellow><b>1-8</b></color>]$op_offer";
      // b += String.Format("\n<color=yellow><b>[{0}]</b></color>$op_switch", Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString());
      // return Localization.instance.Localize(n + s + a + b);
      return Digitalroot.Valheim.Common.Utils.Localize(stringBuilder.ToString());
      // return $"<color=lightblue><b>Odin</b></color> is here";
    }

    /// <inheritdoc />
    public string GetHoverName() => gameObject.name;

    #endregion

    #region Implementation of Interactable

    /// <inheritdoc />
    public bool Interact(Humanoid user, bool hold)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      return false;
    }

    /// <inheritdoc />
    public bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      return false;
    }

    #endregion
  }
}
