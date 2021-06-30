using System.Collections.Generic;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using OdinPlusRemakeJVL.Npcs;
using UnityEngine;
using UnityEngine.UI;

namespace OdinPlusRemakeJVL.Pieces
{
  internal class OdinTradeShop : Trader, Hoverable, Interactable
  {
    public GameObject m_talker;

    private new void Start()
    {
    }

    private new void Update()
    {
    }

    public new string GetHoverText()
    {
      string n = string.Format("\n<color=lightblue><b>{0}</b></color>", m_name);
      string u = "\n[<color=yellow><b>$KEY_Use</b></color>] $op_buy";
      return Common.Utils.Localize(n + u);
    }

    public new string GetHoverName()
    {
      return m_name;
    }

    public new bool Interact(Humanoid user, bool hold)
    {
      if (hold)
      {
        return false;
      }

      Say("$op_pot_open");
      StoreGui.instance.Show(this);
      return true;
    }

    public new bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      return false;
    }

    protected void Say(string text)
    {
      Chat.instance.SetNpcText(m_talker, Vector3.up * 1.5f, 60f, 5, m_talker.GetComponent<OdinNpc>().Name, text, false);
    }

    public static void TweakGui(StoreGui __instance, bool set)
    {
      var go = __instance.gameObject;
      var sell = __instance.m_sellButton.transform.parent.gameObject;
      var icon = __instance.m_coinText.transform.parent.GetChild(0).GetComponent<Image>();
      sell.gameObject.SetActive(!set);
      icon.sprite = !set ? SpriteManager.Instance.GetSprite(ItemNames.Coins) : SpriteManager.Instance.GetSprite(ItemNames.OdinCredit);
      GameObject.Find("/_GameMain/GUI/PixelFix/IngameGui(Clone)/Store_Screen/Store/topic").GetComponent<Text>().text = Common.Utils.Localize(set ? "OdinStore" : @"$store_topic");
    }

    private new void Say(List<string> texts, string trigger)
    {
    }

    private new void Say(string text, string trigger)
    {
    }

    private new void RandomTalk()
    {
    }

    private new void OnBought(TradeItem item)
    {
    }

    private new void OnSold()
    {
    }
  }
}
