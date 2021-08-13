using OdinPlus.Common;
using OdinPlus.Data;
using OdinPlus.Managers;
using UnityEngine;

namespace OdinPlus.Npcs.Humans
{
  public class HumanMessenger : QuestVillager
  {
    protected override void Awake()
    {
      base.Awake();
      ChoiceList = new string[2] {"$op_talk", "$op_human_quest_take"};
    }

    public override void Choice0()
    {
      Say("I need some help, can you take a message for me?");
    }

    public void Choice1()
    {
      if (!IsQuestReady())
      {
        return;
      }

      var key = HumanVis.NPCnames.GetRandomElement();
      OdinData.AddKey(key);
      PlaceRandom(key);
      string n = $"Thanks, you can find <color=yellow><b>{key}</b></color> near our village";
      Say(n);
      ResetQuestCooldown();
    }

    private void PlaceRandom(string key)
    {
      foreach (var item in LocationMarker.MarkList.Values)
      {
        var dis = Utils.DistanceXZ(item.GetPosition(), transform.position);
        if (dis > 100)
        {
          PlaceQuestHuman(key, item.GetPosition());
          break;
        }
      }
    }

    private void PlaceQuestHuman(string key, Vector3 pos)
    {
      var pgo = ZNetScene.instance.GetPrefab("WorkerNPCHuman");
      var go = Instantiate(pgo, PrefabManager.Instance.Root.transform);
      go.GetComponent<HumanVis>().m_name = key;
      float y;
      ZoneSystem.instance.FindFloor(pos, out y);
      pos = new Vector3(pos.x, y + 2, pos.z);
      go.transform.localPosition = pos;
      go.transform.SetParent(transform.parent.parent);
      DBG.blogWarning("Place Quest Worker at " + pos);
    }
  }
}
