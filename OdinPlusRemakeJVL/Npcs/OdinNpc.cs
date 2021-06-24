using System;
using System.Reflection;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  class OdinNpc : AbstractNpc<OdinNpc>
  {
    private readonly Action _getPosition;

    public OdinNpc()
    {
      _getPosition = GetPosition;
    }

    private void Awake()
    {
      
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_god";
      Head = gameObject.transform.Find("visual/Armature/Hips/Spine0/Spine1/Spine2/Head");
      Talker = gameObject;
      // Summon();
      InvokeRepeating(_getPosition.Method.Name, 1, 3);
      Log.Debug("Client start to Calling Request Odin Location");
    }

    public override void SecondaryInteract(Humanoid user)
    {
      // SwitchSkill();
    }

    private void GetPosition()
    {
      // if (NpcManager.Root.transform.position == Vector3.zero)
      // {
      //   LocationManager.Instance.GetStartPosition();
      //   return;
      // }
      // Log.Debug("Client Stop Request odin position");
      // CancelInvoke(_getPosition.Method.Name);
    }

    //public override string GetHoverText()
    //{
    //  StringBuilder stringBuilder = new StringBuilder($"<color=lightblue><b>{Name}</b></color>");
    //  // string s = string.Format("\n<color=lightblue><b>$op_crd:{0}</b></color>", OdinData.Credits);
    //  // string a = string.Format("\n[<color=yellow><b>$KEY_Use</b></color>] $op_use[<color=green><b>{0}</b></color>]", cskill);
    //  // string b = "\n[<color=yellow><b>1-8</b></color>]$op_offer";
    //  // b += String.Format("\n<color=yellow><b>[{0}]</b></color>$op_switch", Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString());
    //  // return Localization.instance.Localize(n + s + a + b);
    //  return Jotunn.Managers.LocalizationManager.Instance.TryTranslate(stringBuilder.ToString());
    //}
  }
}
