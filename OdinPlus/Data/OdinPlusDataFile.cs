using System.Collections.Generic;
using OdinPlus.Quests;
using UnityEngine;

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

    // public List<Quest> Quests = new List<Quest>();
    public List<string> BuzzKeys = new List<string>();
  }
}
