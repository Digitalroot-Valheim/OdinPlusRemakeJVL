using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public class OdinNpc : AbstractNpc, ISecondaryInteractable 
  {
    public GameObject Odin => GameObjectInstance;

    public void Awake()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Name = "$op_god";
        GameObjectInstance = Instantiate(PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalOdin));
        Head = Odin.transform.Find("visual/Armature/Hips/Spine0/Spine1/Spine2/Head");
        Talker = Odin;
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    //public bool Summon()
    //{
    //  Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    //  ReadSkill();
    //  return true;
    //}

    public override string GetHoverName() => Name;

    public override string GetHoverText()
    {
      // Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      StringBuilder stringBuilder = new StringBuilder($"<color=lightblue><b>{Name}</b></color>");
      // string s = string.Format("\n<color=lightblue><b>$op_crd:{0}</b></color>", OdinData.Credits);
      // string a = string.Format("\n[<color=yellow><b>$KEY_Use</b></color>] $op_use[<color=green><b>{0}</b></color>]", cskill);
      // string b = "\n[<color=yellow><b>1-8</b></color>]$op_offer";
      // b += String.Format("\n<color=yellow><b>[{0}]</b></color>$op_switch", Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString());
      // return Localization.instance.Localize(n + s + a + b);
      return Common.Utils.Localize(stringBuilder.ToString());
      // return $"<color=lightblue><b>Odin</b></color> is here";
    }

    public override bool Interact(Humanoid user, bool hold)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({user.name}, {hold})");
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
      Say($"{user.name} $op_raise");
      return true;
    }

    public void SecondaryInteract(Humanoid user)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      // SwitchSkill();
    }

    // public void Spawn(Transform parent)
    // {
    //   Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    //   NpcInstance = Common.Utils.Spawn(CustomPrefabNames.Odin, parent.position, parent);
    //   Head = NpcInstance.transform.Find("visual/Armature/Hips/Spine0/Spine1/Spine2/Head");
    //   Talker = NpcInstance;
    // }

    public override bool UseItem(Humanoid user, ItemDrop.ItemData item) //trans
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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

    //private void CreateOdin()
    //{
    //  Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

    //  var odinPrefab = PrefabManager.Instance.GetPrefab(CustomPrefabNames.Odin);

    //  if (odinPrefab == null)
    //  {
    //    Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.Odin}");
    //    odinPrefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.Odin, ItemNames.Odin);

    //    if (odinPrefab != null)
    //    {
    //      DestroyImmediate(odinPrefab.GetComponent<ZNetView>());
    //      DestroyImmediate(odinPrefab.GetComponent<ZSyncTransform>());
    //      DestroyImmediate(odinPrefab.GetComponent<Odin>());
    //      DestroyImmediate(odinPrefab.GetComponent<Rigidbody>());

    //      foreach (var item in odinPrefab.GetComponentsInChildren<Aoe>())
    //      {
    //        DestroyImmediate(item);
    //      }

    //      foreach (var item in odinPrefab.GetComponentsInChildren<EffectArea>())
    //      {
    //        DestroyImmediate(item);
    //      }

    //      PrefabManager.Instance.AddPrefab(odinPrefab);
    //    }
    //  }

    //  if (odinPrefab == null)
    //  {
    //    Log.Error($"[{GetType().Name}] Error with prefabs.");
    //    Log.Error($"[{GetType().Name}] odinPrefab == null: true");
    //  }
    //}

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
