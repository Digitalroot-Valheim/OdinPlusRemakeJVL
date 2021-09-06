using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using JetBrains.Annotations;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;
using ITalkable = OdinPlusJVL.Common.Interfaces.ITalkable;

namespace OdinPlusJVL.Behaviours
{
  [UsedImplicitly]
  public class OdinsEmissaryCustomMonoBehaviour : AbstractCustomMonoBehaviour, ITalkable, Hoverable, Interactable, ISecondaryInteractable
  {
    private Animator _animator;
    private Transform Head { get; set; }

    [UsedImplicitly]
    public void Awake()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Head = gameObject.transform.Find("visual/Armature/Hips/Spine0/Spine1/Spine2/Head");
        _animator = gameObject.GetComponentInChildren<Animator>();
        TalkingBehaviour = new TalkableMonoBehaviour(gameObject, "$op_odin_emissary", 2.5f, 20f, 10f, 10f);
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
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
      => TalkingBehaviour.Say(msg, topic, showName, longTimeout, large);

    #endregion

    #region Implementation of Hoverable

    /// <inheritdoc />
    public string GetHoverText()
    {
      // Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      StringBuilder stringBuilder = new StringBuilder($"<color=lightblue><b>$op_odin_emissary</b></color>");
      // string s = string.Format("\n<color=lightblue><b>$op_crd:{0}</b></color>", OdinData.Credits);
      // string a = string.Format("\n[<color=yellow><b>$KEY_Use</b></color>] $op_use[<color=green><b>{0}</b></color>]", cskill);
      // string b = "\n[<color=yellow><b>1-8</b></color>]$op_offer";
      // b += String.Format("\n<color=yellow><b>[{0}]</b></color>$op_switch", Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString());
      // return Localization.instance.Localize(n + s + a + b);
      return Digitalroot.Valheim.Common.Utils.Localize(stringBuilder.ToString());
      // return $"<color=lightblue><b>Odin</b></color> is here";
    }

    /// <inheritdoc />
    public string GetHoverName() => gameObject.name;

    #endregion

    #region Implementation of Interactable

    /// <inheritdoc />
    public bool Interact(Humanoid user, bool hold)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({user.name}, {hold})");
      if (hold)
      {
        return false;
      }
      // if (!OdinData.RemoveCredits(Main.RaiseCost))
      //{
      //  Say("$op_god_nocrd");
      //  return false;
      //}

      // user.GetSkills().RaiseSkill(stlist[cskillIndex], Main.RaiseFactor);
      Say($"$op_raise");
      return true;
    }

    /// <inheritdoc />
    public bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var name = item.m_dropPrefab.name;
      int value = 1;
      //if (!OdinData.ItemSellValue.ContainsKey(name))
      //{
      //  Say("$op_god_randomitem " + randomName());
      //  return false;
      //}
      //value = OdinData.ItemSellValue[name];
      //OdinData.AddCredits(value * item.m_stack * item.m_quality, m_head);
      //user.GetInventory().RemoveItem(item.m_shared.m_name, item.m_stack);
      Say("$op_god_takeoffer");
      return true;
    }

    #endregion

    #region Implementation of ISecondaryInteractable

    /// <inheritdoc />
    public void SecondaryInteract(Humanoid user, bool hold)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (hold) return;

      // SwitchSkill();
    }

    #endregion

    private void ReadSkill()
    {
      // slist.Clear();
      // stlist.Clear();
      // foreach (object obj in Enum.GetValues(typeof(Skills.SkillType)))
      // {
      //   Skills.SkillType skillType = (Skills.SkillType)obj;
      //   var s = skillType.ToString();
      //   if (s != "None" && s != "FrostMagic" && s != "All" && s != "FireMagic")
      //   {
      //     slist.Add(skillType.ToString());
      //     stlist.Add(skillType);
      //   }
      // }
      // cskill = slist[cskillIndex];
    }

    public void SwitchSkill()
    {
      //cskillIndex += 1;
      //if (cskillIndex + 1 > slist.Count())
      //{
      //  cskillIndex = 0;
      //}
      //cskill = slist[cskillIndex];
    }
  }
}
