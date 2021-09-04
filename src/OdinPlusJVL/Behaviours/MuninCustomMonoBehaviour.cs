using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using JetBrains.Annotations;
using OdinPlusJVL.Quests;
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
    private readonly System.Timers.Timer _spawnTimer = new(10000);
    private readonly string[] _choiceList = { "$op_munin_c1", "$op_munin_c2", "$op_munin_c3", "$op_munin_c4" };
    private int _index;
    private string _currentChoice = "$op_munin_c1";
    private bool _hasLanded;

    [UsedImplicitly]
    public void Awake()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _spawnTimer.AutoReset = false;
      _spawnTimer.Enabled = false;
      _spawnTimer.Elapsed += SpawnTimerOnElapsed;
      _animator = gameObject.GetComponentInChildren<Animator>();
      TalkingBehaviour = new TalkableMonoBehaviour(gameObject, "$op_munin_name", _animator, 1.5f, 20f, 10f, 10f);
    }

    private void SpawnTimerOnElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (!_hasLanded)
      {
        _spawnTimer.Stop();
        _animator.SetTrigger(MuninsAnimations.FlyIn);
        _hasLanded = true;
        _spawnTimer.Interval = 5000;
        _spawnTimer.Start();
        return;
      }
      Say("$op_munin_greet");
      _spawnTimer.Stop();
      _spawnTimer.Elapsed -= SpawnTimerOnElapsed;
      _spawnTimer.Dispose();
    }

    [UsedImplicitly]
    public void Start()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _spawnTimer.Start();
    }

    #region Implementation of ITalkable

    /// <inheritdoc />
    public TalkableMonoBehaviour TalkingBehaviour { get; set; }

    /// <inheritdoc />
    public void Say(string msg
      , string topic = null
      , string animationTriggerName = null
      , bool showName = true
      , bool longTimeout = false
      , bool large = false)
      => TalkingBehaviour.Say(msg, topic, animationTriggerName, showName, longTimeout, large);

    #endregion

    #region Implementation of Hoverable

    /// <inheritdoc />
    public string GetHoverText()
    {
      StringBuilder n = new StringBuilder($"<color=lightblue><b>$op_munin_name</b></color>")
          .Append($"\n<color=lightblue><b>$op_munin_quest_lvl :{QuestManager.Instance.Level}</b></color>")
          .Append($"\n$op_munin_questnum_a <color=lightblue><b>{QuestManager.Instance.Count()}</b></color> $op_munin_questnum_b")
          .Append("\n[<color=yellow><b>1-8</b></color>] $op_offer")
          .Append($"\n[<color=yellow><b>$KEY_Use</b></color>] {_currentChoice}")
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
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Say("$op_munin_notenough");
      return true;
    }

    #endregion

    #region Implementation of ISecondaryInteractable

    /// <inheritdoc />
    public void SecondaryInteract(Humanoid user)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      _index += 1;
      if (_index + 1 > _choiceList.Length)
      {
        _index = 0;
      }
      Log.Trace(Main.Instance, $"[{GetType().Name}] _currentChoice = {_currentChoice}");
      _currentChoice = _choiceList[_index];
      Log.Trace(Main.Instance, $"[{GetType().Name}] _currentChoice = {_currentChoice}");
    }

    #endregion

    // ReSharper disable once IdentifierTypo
    private static class MuninsAnimations
    {
      internal static string AngLevel = nameof(AngLevel).ToLower(); // No idea?
      internal static string FlyIn = nameof(FlyIn).ToLower();
      internal static string FlyOut = "flyaway";
      internal static string Idle = nameof(Idle).ToLower();
      internal static string Talk = nameof(Talk).ToLower();
      internal static string TeleportIn = nameof(TeleportIn).ToLower();
      internal static string TeleportOut = "poff";
      internal static string TeleportOut2 = "Poff";
    }
  }
}
