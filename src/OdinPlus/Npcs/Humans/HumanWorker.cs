using OdinPlus.Common;
using OdinPlus.Data;

namespace OdinPlus.Npcs.Humans
{
  public class HumanWorker : HumanVillager
  {
    protected override void Awake()
    {
      base.Awake();
      ChoiceList = new string[2] {"$op_talk", "$op_human_message_hand"};
    }

    public override void Choice0()
    {
      Say("Do you have something for me?"); //trans
    }

    public void Choice1()
    {
      var name = m_nview.GetZDO().GetString("npcname");
      if (OdinData.GetKey(name))
      {
        Say("Thank you!");
        OdinData.AddCredits(10, true);
        OdinData.RemoveKey(name);
        var znv = m_talker.GetComponent<ZNetView>();
        DBG.blogInfo($"Trying to remove {name}");
        if (znv == null)
        {
          DBG.blogInfo($"Unable to remove znv is null.");
        }
        else
        {
          DBG.blogInfo($"Removing {name}.");
          znv.Destroy();
        }
      }
      else
      {
        Say("Do you have something for me?");
      }
    }
  }
}
