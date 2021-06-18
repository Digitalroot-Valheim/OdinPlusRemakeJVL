using OdinPlus.Common;
using OdinPlus.Quests;

namespace OdinPlus.Processors
{

	public class DungeonQuestProcessor : QuestProcessor
	{
		public DungeonQuestProcessor(Quest inq)
		{
			Quest = inq;
		}
		public override void Place(LocationMarker locationMarker)
		{
			var cinfo = locationMarker.GetCtnInfo();
			LegacyChest.Place(cinfo.Pos, cinfo.Rot, 3,  Quest.ID,Quest.m_ownerName, Quest.Key, false);
			base.Place(locationMarker);
		}

	}
}