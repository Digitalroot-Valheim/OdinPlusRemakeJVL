using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  class OdinNpc : AbstractNpc<OdinNpc>
  {
    private readonly Action _getPosition;
    private OdinNpc _odin;

    public OdinNpc()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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

      var prefab = ZNetScene.instance.GetPrefab("odin");
      var odin = Instantiate(prefab);
      DestroyImmediate(odin.GetComponent<ZNetView>());
      DestroyImmediate(odin.GetComponent<ZSyncTransform>());
      DestroyImmediate(odin.GetComponent<Odin>());
      DestroyImmediate(odin.GetComponent<Rigidbody>());
      var aoes = odin.GetComponentsInChildren<Aoe>();
      var effectAreas = odin.GetComponentsInChildren<EffectArea>();
      foreach (var item in aoes)
      {
        DestroyImmediate(item);
      }
      foreach (var item in effectAreas)
      {
        DestroyImmediate(item);
      }
      
      _odin = odin.AddComponent<OdinNpc>();
      odin.transform.localPosition = new Vector3(0f, 0, 0f);

    }

    public override string GetHoverText()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      StringBuilder stringBuilder = new StringBuilder($"<color=lightblue><b>{Name}</b></color>");
      // string s = string.Format("\n<color=lightblue><b>$op_crd:{0}</b></color>", OdinData.Credits);
      // string a = string.Format("\n[<color=yellow><b>$KEY_Use</b></color>] $op_use[<color=green><b>{0}</b></color>]", cskill);
      // string b = "\n[<color=yellow><b>1-8</b></color>]$op_offer";
      // b += String.Format("\n<color=yellow><b>[{0}]</b></color>$op_switch", Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString());
      // return Localization.instance.Localize(n + s + a + b);
      return Jotunn.Managers.LocalizationManager.Instance.TryTranslate(stringBuilder.ToString());
    }

    public override bool Interact(Humanoid user, bool hold)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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
      Say("$op_raise");
      return true;
    }

    public override void Start()
    {
      base.Start();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Debug(gameObject.transform.parent.rotation);
      gameObject.transform.parent.Rotate(0, 42, 0);
      Log.Debug(gameObject.transform.parent.rotation);
    }

    public override void SecondaryInteract(Humanoid user)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      // SwitchSkill();
    }

    public override bool UseItem(Humanoid user, ItemDrop.ItemData item)//trans
    {
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

    public void GetPosition()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (_odin.transform.position == Vector3.zero)
      {
        LocationManager.Instance.GetStartPosition();
        return;
      }
      Log.Debug("Client Stop Request odin position");
      CancelInvoke(_getPosition.Method.Name);
    }

    public void SetPosition()
    {
      _odin.transform.localPosition = Player.m_localPlayer.transform.localPosition + Vector3.forward * 4;
    }

    private void ReadSkill()
    {
      //slist.Clear();
      //stlist.Clear();
      //foreach (object obj in Enum.GetValues(typeof(Skills.SkillType)))
      //{
      //  Skills.SkillType skillType = (Skills.SkillType)obj;
      //  var s = skillType.ToString();
      //  if (s != "None" && s != "FrostMagic" && s != "All" && s != "FireMagic")
      //  {
      //    slist.Add(skillType.ToString());
      //    stlist.Add(skillType);
      //  }
      //}
      //cskill = slist[cskillIndex];
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
