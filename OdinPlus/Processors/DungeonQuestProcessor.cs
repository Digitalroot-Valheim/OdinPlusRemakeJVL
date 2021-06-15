using OdinPlus.Common;
using OdinPlus.Quests;

namespace OdinPlus.Processors
{

	public class DungeonQuestProcessor : QuestProcessor
	{
		public DungeonQuestProcessor(Quest inq)
		{
			quest = inq;
		}
		public override void Place(LocationMarker lm)
		{
			var cinfo = lm.GetCtnInfo();
			LegacyChest.Place(cinfo.Pos, cinfo.Rot, 3,  quest.ID,quest.m_ownerName, quest.Key, false);
			base.Place(lm);
		}

	}
}