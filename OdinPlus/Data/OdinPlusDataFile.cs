using OdinPlus.Quests;
using System.Collections.Generic;
using OdinPlus.Common;

namespace OdinPlus.Data
{
  public class OdinPlusDataFile
  {
    public int Credits = 100;
    public bool hasWolf = false;
    public bool hasTroll = false;
    public List<string> BlackList = new List<string>();
    public int QuestCount = 0;
    public Dictionary<string, int> SearchTaskList = new Dictionary<string, int>();
    public Dictionary<string, Quest> Quests = new Dictionary<string, Quest>();
    public List<string> BuzzKeys = new List<string>();
  }

  public class OdinPlusRemakeDataFile
  {
    public int Credits = 100;
    public bool HasWolf;
    public bool HasTroll;
    public List<string> UsedLocationsList = new List<string>();
    public int QuestCounter;
    public Dictionary<string, int> SearchQuestItemsList = new Dictionary<string, int>();
    public Dictionary<string, Quest> ActiveQuests = new Dictionary<string, Quest>();
    public List<string> VillagerQuestTargetNames = new List<string>();

    public OdinPlusRemakeDataFile ConvertFrom(OdinPlusDataFile odinPlusDataFile)
    {
      Credits = odinPlusDataFile.Credits;
      HasWolf = odinPlusDataFile.hasWolf;
      HasTroll = odinPlusDataFile.hasTroll;
      UsedLocationsList.AddRange(odinPlusDataFile.BlackList);
      QuestCounter = odinPlusDataFile.QuestCount;
      odinPlusDataFile.SearchTaskList.Copy(SearchQuestItemsList);
      return null;
    }
  }
}
