using System.Reflection;
using OdinPlus.Common;
using OdinPlus.Quests;
using UnityEngine;

namespace OdinPlus.Processors
{
  public class TreasureQuestProcessor : QuestProcessor
  {
    public TreasureQuestProcessor(Quest quest)
    {
      Quest = quest;
    }

    public override void Place(LocationMarker locationMarker)
    {
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      var pos = locationMarker.GetPosition();
      float y = 0f;
      float x = 4f;
      float z = 3.999f;
      if (Quest.Key == 0)
      {
        y = 0;
        x = 2f;
        z = 1.999f;
      }

      pos += new Vector3(x.RollDice(), y, z.RollDice());
      LegacyChest.Place(pos, Quest.ID, Quest.m_ownerName, Quest.Key);
      Log.Debug($"Client Placed LegacyChest at: {pos}");
      base.Place(locationMarker);
    }
  }
}
