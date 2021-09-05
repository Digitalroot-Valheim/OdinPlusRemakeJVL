using FSMSharp;
using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using UnityEngine;

namespace OdinPlusJVL.FiniteStateMachines
{
  [UsedImplicitly]
  internal class MuninChoicesFSM : MonoBehaviour
  {
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

    // Create the FSM
    private readonly FSM<Choices> _fsm = new(nameof(MuninChoicesFSM));

    internal void Awake()
    {
      _fsm.Add(Choices.AcceptSideQuest)
        .GoesTo(Choices.GiveUpQuest)
        .OnEnter(() => SetChoiceIndex((int)Choices.AcceptSideQuest));

      _fsm.Add(Choices.GiveUpQuest)
        .GoesTo(Choices.ChangeQuestLevel)
        .OnEnter(() => SetChoiceIndex((int)Choices.GiveUpQuest));

      _fsm.Add(Choices.ChangeQuestLevel)
        .GoesTo(Choices.ShowQuestList)
        .OnEnter(() => SetChoiceIndex((int)Choices.ChangeQuestLevel));

      _fsm.Add(Choices.ShowQuestList)
        .GoesTo(Choices.AcceptSideQuest)
        .OnEnter(() => SetChoiceIndex((int)Choices.ShowQuestList));

      _fsm.CurrentState = Choices.AcceptSideQuest;
    }

    private void SetChoiceIndex(int i)
    {
      var cmb = gameObject.GetComponent<MuninCustomMonoBehaviour>();
      cmb.SetChoiceIndex(i);
    }

    public void Next() => _fsm.Next();
    public Choices CurrentState => _fsm.CurrentState;
  }
}
