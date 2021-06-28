using Jotunn.Managers;
using OdinPlusRemakeJVL.Common;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public class OdinNpc : AbstractNpc<OdinNpc>
  {
    // public static GameObject Root;
    public GameObject Odin;
    // private Action _getPosition;

    public override void Start()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      // var prefab = ZNetScene.instance.GetPrefab("odin");
      var odinPrefab = PrefabManager.Instance.CreateClonedPrefab(GetType().Name, "odin");
      Log.Trace($"[{GetType().Name}] odinPrefab == null: {odinPrefab == null}");
      PrefabManager.Instance.AddPrefab(odinPrefab);
      Odin = Instantiate(odinPrefab);
      Log.Trace($"[{GetType().Name}] Odin == null: {Odin == null}");

      if (Odin == null) return;

      Log.Trace($"[{GetType().Name}] Remove Prefab stuff we do not want.");
      DestroyImmediate(Odin.GetComponent<ZNetView>());
      DestroyImmediate(Odin.GetComponent<ZSyncTransform>());
      DestroyImmediate(Odin.GetComponent<Odin>());
      DestroyImmediate(Odin.GetComponent<Rigidbody>());
      
      foreach (var item in Odin.GetComponentsInChildren<Aoe>())
      {
        DestroyImmediate(item);
      }
      
      foreach (var item in Odin.GetComponentsInChildren<EffectArea>())
      {
        DestroyImmediate(item);
      }

      Log.Trace($"[{GetType().Name}] Prefab cleaned up");


      // Log.Debug($"[{GetType().Name}] Start rotation: {gameObject.transform.parent.rotation}");
      // gameObject.transform.parent.Rotate(0, 42, 0);
      // Log.Debug($"[{GetType().Name}] End rotation: {gameObject.transform.parent.rotation}");

      Head = gameObject.transform.Find("visual/Armature/Hips/Spine0/Spine1/Spine2/Head");
      Talker = gameObject;
      // Summon();
      // InvokeRepeating(_getPosition.Method.Name, 1, 3);
    }

    public override void OnEnable()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    //public bool Summon()
    //{
    //  Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    //  Odin.gameObject.transform.parent.localPosition = FindSpawnPoint();
    //  ReadSkill();
    //  return true;
    //}



    public override string GetHoverText()
    {
      // Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      StringBuilder stringBuilder = new StringBuilder($"<color=lightblue><b>{Name}</b></color>");
      // string s = string.Format("\n<color=lightblue><b>$op_crd:{0}</b></color>", OdinData.Credits);
      // string a = string.Format("\n[<color=yellow><b>$KEY_Use</b></color>] $op_use[<color=green><b>{0}</b></color>]", cskill);
      // string b = "\n[<color=yellow><b>1-8</b></color>]$op_offer";
      // b += String.Format("\n<color=yellow><b>[{0}]</b></color>$op_switch", Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString());
      // return Localization.instance.Localize(n + s + a + b);
      return Localization.instance.Localize(stringBuilder.ToString());
      // return $"<color=lightblue><b>Odin</b></color> is here";
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

    public override void SecondaryInteract(Humanoid user)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      // SwitchSkill();
    }

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

    //public void GetPosition()
    //{
    //  Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    //  // if (_odin.transform.position == Vector3.zero)
    //  // {
    //  //   LocationManager.Instance.GetStartPosition();
    //  //   return;
    //  // }

    //  Log.Debug("Client Stop Request odin position");
    //  CancelInvoke(_getPosition.Method.Name);
    //}

    public void SetPosition()
    {
      //Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      //Odin.transform.localPosition = Player.m_localPlayer.transform.localPosition + Vector3.forward * 4;
      //Odin.transform.localPosition = Odin.transform.localPosition + Vector3.down;
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
