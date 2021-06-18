using System.Text;
using OdinPlus.Common;
using OdinPlus.Items;
using OdinPlus.Managers;
using OdinPlus.Processors;
using UnityEngine;

namespace OdinPlus.Npcs
{
  class OdinMunin : OdinNpc
  {
    private string[] choice = {"$op_munin_c1", "$op_munin_c2", "$op_munin_c3", "$op_munin_c4"};
    private int index;
    private string currentChoice = "";
    private float timer;
    private float questCD = 5f;
    private Animator m_animator;
    public static OdinMunin instance;

    private void Awake()
    {
      instance = this;
      m_name = "$op_munin_name";
      m_talker = gameObject;
      currentChoice = choice[index];
      m_animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
      gameObject.transform.Rotate(0, -30f, 0);
      m_animator.SetTrigger("teleportin");
      m_animator.SetTrigger("talk");
    }

    private void Update()
    {
      if (timer > 0)
      {
        timer -= Time.deltaTime;
      }
    }

    private void OnDestroy()
    {
      instance = null;
    }

    #region Feature

    private void CreatSideQuest()
    {
      if (timer > 0)
      {
        var n = $"<color=yellow><b>{Mathf.CeilToInt(timer)}</b></color>";
        Say("$op_munin_cd " + n);
        return;
      }

      if (QuestManager.Instance.Count() >= 10)
      {
        Say("$op_munin_questfull");
        return;
      }

      QuestManager.Instance.CreateRandomQuest();
      Say("$op_munin_wait_hug");
      timer = questCD;
    }

    private void GiveUpQuest()
    {
      if (QuestManager.Instance.HasQuest())
      {
        //string n = string.Format("Which Quest you want to give up?", QuestManager.instance.Count());
        string n = "$op_munin_giveup";
        n = Localization.instance.Localize(n);
        TextInput.instance.RequestText(new TR_Giveup(), n, 3);
        ResetTimer();
        return;
      }

      Say("$op_munin_noquest");
    }

    private void ChangeLevel()
    {
      if (QuestManager.Instance.Level == QuestManager.MaxLevel)
      {
        QuestManager.Instance.Level = 1;
        return;
      }

      QuestManager.Instance.Level++;
    }

    #endregion Feature

    #region Val

    public override bool Interact(Humanoid user, bool hold)
    {
      if (hold)
      {
        return false;
      }

      switch (index)
      {
        case 0:
          CreatSideQuest();
          break;
        case 1:
          GiveUpQuest();
          break;
        case 2:
          ChangeLevel();
          break;
        case 3:
          if (QuestManager.Instance.HasQuest())
          {
            QuestManager.Instance.PrintQuestList();
            Say("$op_munin_wait_hug");
            break;
          }

          Say("$op_munin_noquest");
          break;
      }

      return true;
    }

    public override void SecondaryInteract(Humanoid user)
    {
      index += 1;
      if (index + 1 > choice.Length)
      {
        index = 0;
      }

      currentChoice = choice[index];
    }

    public override string GetHoverText()
    {
      StringBuilder n = new StringBuilder($"<color=lightblue><b>{m_name}</b></color>")
        .Append($"\n<color=lightblue><b>$op_munin_quest_lvl :{QuestManager.Instance.Level}</b></color>")
        .Append($"\n$op_munin_questnum_b <color=lightblue><b>{QuestManager.Instance.Count()}</b></color> $op_munin_questnum_a")
        .Append("\n[<color=yellow><b>1-8</b></color>]$op_offer")
        .Append($"\n[<color=yellow><b>$KEY_Use</b></color>]{currentChoice}")
        .Append($"\n<color=yellow><b>[{Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString()}]</b></color>$op_switch");
      return Localization.instance.Localize(n.ToString());
    }

    public override string GetHoverName()
    {
      return Localization.instance.Localize(m_name);
    }

    public override bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      if (!SearchQuestProcessor.CanOffer(item.m_dropPrefab.name))
      {
        return false;
      }

      if (SearchQuestProcessor.CanFinish(item.m_dropPrefab.name))
      {
        Say("$op_munin_takeoffer");
        return true;
      }

      Say("$op_munin_notenough");
      return true;
    }

    #endregion Val

    #region Tool

    public static void Reward(int key, int level)
    {
      var a = Instantiate(ZNetScene.instance.GetPrefab(OdinPlusItem.OdinLegacy), instance.transform.position + Vector3.up * 2f + Vector3.forward, Quaternion.identity);
      var id = a.GetComponent<ItemDrop>().m_itemData;
      id.m_stack = key;
      id.m_quality = level;
      ResetTimer();
    }

    public static void ResetTimer()
    {
      instance.timer = 0f;
    }

    #endregion Tool

    #region TextGUI

    private class TR_Giveup : TextReceiver
    {
      public string GetText()
      {
        return "";
      }

      public void SetText(string text)
      {
        int num;
        if (int.TryParse(text, out num))
        {
          if (!QuestManager.Instance.GiveUpQuest(num))
          {
            DBG.InfoCT("$op_munin_noq " + num);
            return;
          }

          return;
        }

        DBG.InfoCT("$op_wrong_num");
      }
    }

    #endregion TextGUI
  }
}
