using Digitalroot.Valheim.Common;
using FSMSharp;
using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using UnityEngine;

namespace OdinPlusJVL.FiniteStateMachines
{
  [UsedImplicitly]
  internal class MuninChoicesFSM : MonoBehaviour
  {
    internal string CurrentChoiceText { get; private set; }
    public Choices CurrentState => _fsm.CurrentState;
    private readonly FSM<Choices> _fsm = new(nameof(MuninChoicesFSM));

    internal void Awake()
    {
      _fsm.DebugLogHandler = s => Log.Trace(Main.Instance, s);

      _fsm.Add(Choices.AcceptSideQuest)
        .GoesTo(Choices.GiveUpQuest)
        .OnEnter(() => CurrentChoiceText = "$op_munin_c1");

      _fsm.Add(Choices.GiveUpQuest)
        .GoesTo(Choices.ChangeQuestLevel)
        .OnEnter(() => CurrentChoiceText = "$op_munin_c2");

      _fsm.Add(Choices.ChangeQuestLevel)
        .GoesTo(Choices.ShowQuestList)
        .OnEnter(() => CurrentChoiceText = "$op_munin_c3");

#if DEBUG
      _fsm.Add(Choices.ShowQuestList)
        .GoesTo(Choices.Leave)
        .OnEnter(() => CurrentChoiceText = "$op_munin_c4");

      _fsm.Add(Choices.Leave)
        .GoesTo(Choices.AcceptSideQuest)
        .OnEnter(() => CurrentChoiceText = "$op_munin_c5");

#else
      _fsm.Add(Choices.ShowQuestList)
        .GoesTo(Choices.AcceptSideQuest)
        .OnEnter(() => CurrentChoiceText = "$op_munin_c4");
#endif

      _fsm.CurrentState = Choices.AcceptSideQuest;
    }

    public void Next() => _fsm.Next();

    internal enum Choices
    {
      // "op_munin_c1": "Accept Side Quest",
      // "op_munin_c2": "Give up Quest",
      // "op_munin_c3": "Change Quest Level",
      // "op_munin_c4": "Show me my Quest List",
      AcceptSideQuest
      , GiveUpQuest
      , ChangeQuestLevel
      , ShowQuestList
      , Leave
    }
  }
}
