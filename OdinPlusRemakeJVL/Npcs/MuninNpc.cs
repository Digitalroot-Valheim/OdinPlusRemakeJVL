using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using System.Text;
using OdinPlusRemakeJVL.Common.Names;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public class MuninNpc : AbstractNpc, ISecondaryInteractable
  {
    public GameObject Munin => GameObjectInstance;

    private Animator m_animator;

    public void Awake()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Name = "Munin";
        GameObjectInstance = Instantiate(PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalMunin));
        Talker = Munin;
        m_animator = Munin.GetComponentInChildren<Animator>();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
    private void Start()
    {
      m_animator.SetTrigger("teleportin");
      m_animator.SetTrigger("talk");
    }

    public override string GetHoverName() => Name;

    public override string GetHoverText()
    {
      StringBuilder n = new StringBuilder($"<color=lightblue><b>{Name}</b></color>")
          // .Append($"\n<color=lightblue><b>$op_munin_quest_lvl :{QuestManager.Instance.Level}</b></color>")
          // .Append($"\n$op_munin_questnum_b <color=lightblue><b>{QuestManager.Instance.Count()}</b></color> $op_munin_questnum_a")
          .Append("\n[<color=yellow><b>1-8</b></color>]$op_offer")
        // .Append($"\n[<color=yellow><b>$KEY_Use</b></color>]{currentChoice}")
        // .Append($"\n<color=yellow><b>[{Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString()}]</b></color>$op_switch")
        ;
      return Localization.instance.Localize(n.ToString());
    }

    public override bool Interact(Humanoid user, bool hold)
    {
      if (hold)
      {
        return false;
      }

      // switch (index)
      // {
      //   case 0:
      //     CreatSideQuest();
      //     break;
      //   case 1:
      //     GiveUpQuest();
      //     break;
      //   case 2:
      //     ChangeLevel();
      //     break;
      //   case 3:
      //     if (QuestManager.Instance.HasQuest())
      //     {
      //       QuestManager.Instance.PrintQuestList();
      //       Say("$op_munin_wait_hug");
      //       break;
      //     }
      //
      //     Say("$op_munin_noquest");
      //     break;
      // }

      return true;
    }

    public override bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      //if (!SearchQuestProcessor.CanOffer(item.m_dropPrefab.name))
      //{
      //  return false;
      //}

      //if (SearchQuestProcessor.CanFinish(item.m_dropPrefab.name))
      //{
      //  Say("$op_munin_takeoffer");
      //  return true;
      //}

      Say("$op_munin_notenough");
      return true;
    }

    public void SecondaryInteract(Humanoid user)
    {
      // index += 1;
      // if (index + 1 > choice.Length)
      // {
      //   index = 0;
      // }
      //
      // currentChoice = choice[index];
    }
  }
}
