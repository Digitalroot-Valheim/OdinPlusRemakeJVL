namespace OdinPlus.Npcs.Humans
{
	public class QuestVillager : HumanVillager
	{
		protected virtual void Start()
		{
			EXCobj.SetActive(IsQuestReady());
		}   
	}
}
