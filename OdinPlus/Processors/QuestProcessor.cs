using OdinPlus.Common;
using OdinPlus.Managers;
using OdinPlus.Quests;

namespace OdinPlus.Processors
{
  public class QuestProcessor
  {
    protected Quest Quest;

    public static QuestProcessor Create(Quest inq)
    {
      switch (inq.m_type)
      {
        case QuestType.Dungeon:
          return new DungeonQuestProcessor(inq);
        case QuestType.Treasure:
          return new TreasureQuestProcessor(inq);
        case QuestType.Hunt:
          return new HuntQuestProcessor(inq);
        case QuestType.Search:
          return new SearchQuestProcessor(inq);
        default:
          return new QuestProcessor(inq);
      }
    }

    public void SetQuest(Quest inq)
    {
      Quest = inq;
    }

    public QuestProcessor(Quest inq)
    {
      Quest = inq;
    }

    protected QuestProcessor()
    {
    }

    public virtual void Init()
    {
      var list1 = QuestRef.LocDic[Quest.GetQuestType()];
      var list2 = list1[Quest.Key];
      Quest.locName = list2.GetRandomElement();
      QuestManager.Instance.Invoke("ShowWaitError", 10);
      ZRoutedRpc.instance.InvokeRoutedRPC("RPC_ServerFindLocation", Quest.locName, Quest.m_realPostion);
    }

    public virtual void Begin()
    {
      QuestManager.Instance.MyQuests.Add(Quest.ID, Quest);
      Quest.m_ownerName = Player.m_localPlayer.GetPlayerName();
      Quest.Begin();
    }

    public virtual void Place(LocationMarker locationMarker)
    {
      locationMarker.Used();
    }

    public virtual void Finish()
    {
      Quest.Finish();
    }
  }
}
