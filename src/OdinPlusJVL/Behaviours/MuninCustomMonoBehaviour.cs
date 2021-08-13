using JetBrains.Annotations;
using OdinPlusJVL.Common.Interfaces;
using System.Text;
using UnityEngine;

namespace OdinPlusJVL.Behaviours
{
  public class MuninCustomMonoBehaviour : AbstractCustomMonoBehaviour, ITalkable, Hoverable, Interactable, ISecondaryInteractable
  {
    private Animator _animator;

    [UsedImplicitly]
    public void Awake()
    {
      _animator = gameObject.GetComponentInChildren<Animator>();
    }

    [UsedImplicitly]
    public void Start()
    {
      _animator.SetTrigger("teleportin");
      _animator.SetTrigger("talk");
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
          // .Append($"\n<color=lightblue><b>$op_munin_quest_lvl :{QuestManager.Instance.Level}</b></color>")
          // .Append($"\n$op_munin_questnum_b <color=lightblue><b>{QuestManager.Instance.Count()}</b></color> $op_munin_questnum_a")
          .Append("\n[<color=yellow><b>1-8</b></color>]$op_offer")
        // .Append($"\n[<color=yellow><b>$KEY_Use</b></color>]{currentChoice}")
        // .Append($"\n<color=yellow><b>[{Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString()}]</b></color>$op_switch")
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

    /// <inheritdoc />
    public bool UseItem(Humanoid user, ItemDrop.ItemData item)
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

      Say(Talker, gameObject.name, "$op_munin_notenough");
      return true;
    }

    #endregion

    #region Implementation of ISecondaryInteractable

    /// <inheritdoc />
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

    #endregion
  }
}
