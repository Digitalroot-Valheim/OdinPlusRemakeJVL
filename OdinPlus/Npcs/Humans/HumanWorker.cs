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
      if (OdinData.GetKey(m_nview.GetZDO().GetString("npcname")))
      {
        Say("Thank you!");
        OdinData.AddCredits(10, true);
      }
      else
      {
        Say("Do you have something for me?");
      }
    }
  }
}
