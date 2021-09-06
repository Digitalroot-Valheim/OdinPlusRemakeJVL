using Digitalroot.Valheim.Common;
using FSMSharp;
using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using UnityEngine;

namespace OdinPlusJVL.FiniteStateMachines
{
  /// <inheritdoc />
  [UsedImplicitly]
  internal class MuninAnimatorFSM : MonoBehaviour
  {
    private enum MuninsAnimationsStates
    {
      Spawn
      , FlyIn
      , Landed
      , Leaving
      , Away
      , FlyOut
      , Idle
      , Talk
      , TeleportIn
      , TeleportOut
    }

    // Create the FSM
    private readonly FSM<MuninsAnimationsStates> _fsm = new(nameof(MuninAnimatorFSM));

    internal void Awake()
    {
      _fsm.DebugLogHandler = s => Log.Trace(Main.Instance, s);

      #region FlyIn

      _fsm.Add(MuninsAnimationsStates.Spawn)
        .Expires(10f)
        .GoesTo(MuninsAnimationsStates.FlyIn);

      _fsm.Add(MuninsAnimationsStates.FlyIn)
        .Expires(4f)
        .GoesTo(MuninsAnimationsStates.Landed)
        .OnEnter(() => SetAnimation(MuninsAnimations.FlyIn));

      _fsm.Add(MuninsAnimationsStates.Landed)
        .Expires(.5f)
        .GoesTo(MuninsAnimationsStates.Idle)
        .OnEnter(OnLanded);

      #endregion

      #region FlyAway

      _fsm.Add(MuninsAnimationsStates.Leaving)
        .Expires(1.5f)
        .GoesTo(MuninsAnimationsStates.FlyOut);

      _fsm.Add(MuninsAnimationsStates.FlyOut)
        .Expires(15f)
        .GoesTo(MuninsAnimationsStates.Away)
        .OnEnter(() => SetAnimation(MuninsAnimations.FlyOut));

      #endregion

      _fsm.Add(MuninsAnimationsStates.TeleportIn)
        .Expires(1f)
        .GoesTo(MuninsAnimationsStates.Idle)
        .OnEnter(() => SetAnimation(MuninsAnimations.TeleportIn));

      _fsm.Add(MuninsAnimationsStates.TeleportOut)
        .Expires(1f)
        .GoesTo(MuninsAnimationsStates.Away)
        .OnEnter(() => SetAnimation(MuninsAnimations.TeleportOut));

      _fsm.Add(MuninsAnimationsStates.Talk)
        .Expires(1.5f)
        .GoesTo(MuninsAnimationsStates.Idle)
        .OnEnter(() => SetAnimation(MuninsAnimations.Talk));

      _fsm.Add(MuninsAnimationsStates.Idle)
        .OnEnter(() => SetAnimation(MuninsAnimations.Idle));

      _fsm.Add(MuninsAnimationsStates.Away);
    }

    internal void Update()
    {
      _fsm.Process(Time.time);
    }

    public void Spawn()
    {
      _fsm.CurrentState = MuninsAnimationsStates.Spawn;
    }

    public void Leave()
    {
      _fsm.CurrentState = MuninsAnimationsStates.Leaving;
    }

    public void Talk()
    {
      _fsm.CurrentState = MuninsAnimationsStates.Talk;
    }

    public void TeleportIn()
    {
      _fsm.CurrentState = MuninsAnimationsStates.TeleportIn;
    }

    public void TeleportOut()
    {
      _fsm.CurrentState = MuninsAnimationsStates.TeleportOut;
    }

    private void OnLanded()
    {
      SetAnimation(MuninsAnimations.Talk);
      var cmb = gameObject.GetComponent<MuninCustomMonoBehaviour>();
      cmb.Say("$op_munin_greet");
    }

    private void SetAnimation(string animation)
    {
      var animator = gameObject.GetComponentInChildren<Animator>();
      animator.SetTrigger(animation);
    }

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
