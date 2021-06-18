using System.Text.RegularExpressions;
using OdinPlus.Common;
using OdinPlus.Quests;

namespace OdinPlus.Processors
{
  public class HuntQuestProcessor : QuestProcessor
  {
    public HuntQuestProcessor(Quest inq)
    {
      Quest = inq;
    }

    public override void Begin()
    {
      SetMonsterName();
      base.Begin();
    }

    public override void Place(LocationMarker locationMarker)
    {
      HuntTarget.Place(locationMarker.GetPosition(), Quest.locName, Quest.ID, Quest.m_ownerName, Quest.Key, Quest.Level);
      base.Place(locationMarker);
    }

    private void SetMonsterName()
    {
      var Key = Quest.Key;
      if (Key >= 5 || Key <= 0)
      {
        Key = 4.RollDice() + 1;
      }

      Quest.locName = QuestRef.HunterMonsterList[Key - 1];
      Quest.locName = Regex.Replace(Quest.locName, @"[_]", "");
    }
  }
}
