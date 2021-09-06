using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using JetBrains.Annotations;
using OdinPlusJVL.FiniteStateMachines;
using OdinPlusJVL.Quests;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using ITalkable = OdinPlusJVL.Common.Interfaces.ITalkable;

namespace OdinPlusJVL.Behaviours
{
  [UsedImplicitly]
  public class MuninCustomMonoBehaviour : AbstractCustomMonoBehaviour, ITalkable, Hoverable, Interactable, ISecondaryInteractable
  {
    private Animator _animator;
    private MuninAnimatorFSM _muninAnimatorFSM;
    private MuninChoicesFSM _muninChoicesFSM;

    [UsedImplicitly]
    public void Awake()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _animator = gameObject.GetComponentInChildren<Animator>();
      _muninAnimatorFSM = gameObject.GetComponentInChildren<MuninAnimatorFSM>();
      _muninChoicesFSM = gameObject.GetComponent<MuninChoicesFSM>();
      TalkingBehaviour = new TalkableMonoBehaviour(gameObject, "$op_munin_name", 1.5f, 20f, 10f, 10f);
    }

    [UsedImplicitly]
    public void Start()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _muninAnimatorFSM.Spawn();
    }

    #region Implementation of ITalkable

    /// <inheritdoc />
    // ReSharper disable once NotNullMemberIsNotInitialized
    public TalkableMonoBehaviour TalkingBehaviour { get; set; }

    /// <inheritdoc />
    public void Say(string msg
      , string topic = null
      , bool showName = true
      , bool longTimeout = false
      , bool large = false)
    {
      _muninAnimatorFSM.Talk();
      TalkingBehaviour.Say(msg, topic, showName, longTimeout, large);
    }

    #endregion

    #region Implementation of Hoverable

    /// <inheritdoc />
    public string GetHoverText()
    {
      StringBuilder n = new StringBuilder($"<color=lightblue><b>$op_munin_name</b></color>")
          .Append($"\n<color=lightblue><b>$op_munin_quest_lvl :{QuestManager.Instance.Level}</b></color>")
          .Append($"\n$op_munin_questnum_a <color=lightblue><b>{QuestManager.Instance.Count()}</b></color> $op_munin_questnum_b")
          .Append("\n[<color=yellow><b>1-8</b></color>] $op_offer")
          .Append($"\n[<color=yellow><b>$KEY_Use</b></color>] {_muninChoicesFSM.CurrentChoiceText}")
          .Append($"\n[<color=yellow><b>{Main.KeyboardShortcutSecondInteractKey.Value.MainKey}</b></color>] $op_switch")
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

      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      switch (_muninChoicesFSM.CurrentState)
      {
        // case 0:
        //   // CreatSideQuest();
        //   break;
        // case 1:
        //   // GiveUpQuest();
        //   break;
        // case 2:
        //   // ChangeLevel();
        //   break;
        // case 3:
        //   if (QuestManager.Instance.HasQuest())
        //   {
        //     QuestManager.Instance.PrintQuestList();
        //     Say("$op_munin_wait_hug");
        //     break;
        //   }
        //
        //   Say("$op_munin_noquest");
        //   break;
        case MuninChoicesFSM.Choices.AcceptSideQuest:
          // CreatSideQuest();
          break;
        case MuninChoicesFSM.Choices.GiveUpQuest:
          // GiveUpQuest();
          break;
        case MuninChoicesFSM.Choices.ChangeQuestLevel:
          // ChangeLevel();
          break;
        case MuninChoicesFSM.Choices.ShowQuestList:
          //   if (QuestManager.Instance.HasQuest())
          //   {
          //     QuestManager.Instance.PrintQuestList();
          //     Say("$op_munin_wait_hug");
          //     break;
          //   }
          //
          Say("$op_munin_noquest");
          break;

        case MuninChoicesFSM.Choices.Leave:
          Say("$op_munin_goodbye");
          _muninAnimatorFSM.Leave();
          break;

        default:
          throw new ArgumentOutOfRangeException();
      }

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
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Say("$op_munin_notenough");
      return true;
    }

    #endregion

    #region Implementation of ISecondaryInteractable

    private float _lastInteraction = -1f;
    /// <inheritdoc />
    public void SecondaryInteract(Humanoid user)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (_lastInteraction < 0)
      {
        _lastInteraction = Time.time;
        _muninChoicesFSM.Next();
        return;
      }

      if (Time.time - _lastInteraction < 0.25f) 
      {
        Log.Trace(Main.Instance, "Not long enough");
        return;
      }

      _lastInteraction = Time.time;
      _muninChoicesFSM.Next();
    }

    #endregion
  }
}
