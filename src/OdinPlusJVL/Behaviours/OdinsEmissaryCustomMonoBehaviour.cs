using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Common.Interfaces;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OdinPlusJVL.Behaviours
{
  [UsedImplicitly]
  public class OdinsEmissaryCustomMonoBehaviour : AbstractCustomMonoBehaviour, ITalkable, Hoverable, Interactable, ISecondaryInteractable
  {
    public void Awake()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Head = gameObject.transform.Find("visual/Armature/Hips/Spine0/Spine1/Spine2/Head");
        Talker = gameObject;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
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
      // Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      StringBuilder stringBuilder = new StringBuilder($"<color=lightblue><b>{gameObject.name}</b></color>");
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
      Say(Talker, gameObject.name, $"$op_raise");
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
      Say(Talker, gameObject.name, "$op_god_takeoffer");
      return true;
    }

    #endregion

    #region Implementation of ISecondaryInteractable

    /// <inheritdoc />
    public void SecondaryInteract(Humanoid user)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      // SwitchSkill();
    }

    #endregion

    public void SetPosition()
    {
      //Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      //Odin.transform.localPosition = Player.m_localPlayer.transform.localPosition + Vector3.forward * 4;
      //Odin.transform.localPosition = Odin.transform.localPosition + Vector3.down;
    }

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
