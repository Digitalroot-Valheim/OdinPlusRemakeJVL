using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using OdinPlus.Common;
using OdinPlus.Data;
using OdinPlus.Processors;
using OdinPlus.Quests;
using UnityEngine;

namespace OdinPlus.Managers
{
  public class QuestManager : MonoBehaviour
  {
    #region Variable

    #region Data

    public Dictionary<string, Quest> MyQuests = new Dictionary<string, Quest>();
    private Quest _waitQuest;
    public string[] BuzzKeys = new string[0];

    #endregion Data

    #region CFG

    private static readonly string[] RefKeys = {"defeated_eikthyr", "defeated_gdking", "defeated_bonemass", "defeated_moder", "defeated_goblinking"};
    public static readonly int MaxLevel = 3;

    #endregion CFG

    #region In

    public bool IsMain = false;
    public int Level = 1;
    public int GameKey;

    #endregion In

    #region interal

    public static QuestManager Instance { get; private set; }
    QuestProcessor _questProcessor;

    #endregion interal

    #endregion Variable

    #region Main

    [UsedImplicitly]
    private void Awake()
    {
      Instance = this;
      MyQuests = new Dictionary<string, Quest>();
      Main.RegisterRpcAction = (Action) Delegate.Combine(Main.RegisterRpcAction, (Action) RegisterRpc);
    }

    [UsedImplicitly]
    private void Update()
    {
      CheckPlace();
    }

    private void CheckPlace()
    {
      var lmList = LocationMarker.MarkList;
      foreach (var item in MyQuests.Keys)
      {
        if (lmList.ContainsKey(item))
        {
          var lm = lmList[item];
          var quest = MyQuests[item];
          QuestProcessor.Create(quest).Place(lm);
          return;
          //HELP Do i need yield return? but will Lead to a new questprocser(Multi-thread)
        }
      }
    }

    public void Clear()
    {
      MyQuests.Clear();
    }

    #endregion Main

    #region Rpc

    public void RegisterRpc()
    {
      MyQuests = new Dictionary<string, Quest>();
      ZRoutedRpc.instance.Register("RPC_CreateQuestSucceed", new Action<long, string, Vector3>(RPC_CreateQuestSucceed));
      ZRoutedRpc.instance.Register("RPC_CreateQuestFailed", RPC_CreateQuestFailed);
      DBG.blogWarning("QuestManager rpc reged");
    }

    public void RPC_CreateQuestSucceed(long sender, string id, Vector3 pos)
    {
      CancelWaitError();
      var quest = _waitQuest;
      quest.ID = id;
      quest.m_realPostion = pos;
      _questProcessor.Begin();
      DBG.blogWarning($"Client :Create Quest {id} {quest.locName} at {pos}");
    }

    public void RPC_CreateQuestFailed(long sender)
    {
      CancelWaitError();
      DBG.InfoCT("Try Agian,the dice is broken");
      DBG.blogError($"Cannot Place Quest :  {_waitQuest.locName}");
      _waitQuest = null;
    }

    #endregion Rpc

    #region Feature

    public void CreateMuninQuest()
    {
    }

    public bool CanCreateQuest()
    {
      if (_waitQuest != null)
      {
        DBG.InfoCT("$op_quest_failed_wait");
        return false;
      }

      return true;
    }

    public void CreateRandomQuest()
    {
      QuestType[] a = {QuestType.Treasure};
      switch (CheckKey())
      {
        case 0:
          a = new[] {QuestType.Search, QuestType.Treasure};
          break;
        case 1:
          a = new[] {QuestType.Treasure, QuestType.Dungeon, QuestType.Search};
          break;
        case 2:
          a = new[] {QuestType.Treasure, QuestType.Hunt, QuestType.Dungeon, QuestType.Search};
          break;
        case 3:
          a = new[] {QuestType.Treasure, QuestType.Dungeon, QuestType.Hunt, QuestType.Search};
          break;
        case 4:
          a = new[] {QuestType.Treasure, QuestType.Dungeon, QuestType.Hunt, QuestType.Search};
          break;
        case 5:
          a = new[] {QuestType.Treasure, QuestType.Dungeon, QuestType.Hunt, QuestType.Search};
          break;
      }

      int l = a.Length;
      if (1f.RollDice() < 0.1)
      {
        CreateQuest(QuestType.Search);
        return;
      }

      DBG.blogWarning("Dice Rolled");
      Instance.CreateQuest(a[l.RollDice()]);
    }

    public Quest CreateQuest(QuestType type)
    {
      return CreateQuest(type, Game.instance.GetPlayerProfile().GetCustomSpawnPoint());
    }

    public Quest CreateQuest(QuestType type, Vector3 pos)
    {
      if (MyQuests == null)
      {
        MyQuests = new Dictionary<string, Quest>();
      }

      //upd multiple overloads
      _waitQuest = new Quest();
      _waitQuest.m_type = type;
      GameKey = CheckKey();
      _waitQuest.Key = GameKey;
      _waitQuest.m_realPostion = pos;
      //upd ismain?
      //hack LEVEL
      _questProcessor = QuestProcessor.Create(_waitQuest);
      _questProcessor.Init();
      return _waitQuest;
    }

    public bool GiveUpQuest(int ind)
    {
      foreach (var quest in MyQuests.Values)
      {
        if (quest.m_index == ind)
        {
          quest.Giveup();
          MyQuests.Remove(quest.ID);
          UpdateQuestList();
          DBG.blogInfo("Client give up quest" + ind);
          return true;
        }
      }

      return false;
    }

    public Quest GetQuest(string p_id)
    {
      foreach (var id in MyQuests.Keys)
      {
        if (id == p_id)
        {
          return MyQuests[id];
        }
      }

      return null;
    }

    #endregion Feature

    #region Tool

    public int CheckKey()
    {
      int result = 0;
      var keys = ZoneSystem.instance.GetGlobalKeys();
      foreach (var item in RefKeys)
      {
        if (keys.Contains(item))
        {
          result += 1;
        }
      }

      GameKey = result;
      return result;
    }

    public bool HasQuest()
    {
      return MyQuests.Count != 0;
    }

    public int Count()
    {
      if (MyQuests == null)
      {
        return 0;
      }

      return MyQuests.Count;
    }

    public void PrintQuestList()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (var quest in MyQuests.Values)
      {
        stringBuilder.AppendLine(quest.PrintData());
      }

      Tweakers.QuestTopicHugin("Quest List", stringBuilder.ToString());
    }

    public void UpdateQuestList()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (var quest in MyQuests.Values)
      {
        stringBuilder.AppendLine(quest.PrintData());
      }

      Tweakers.addHints(stringBuilder.ToString());
    }

    private void ShowWaitError()
    {
      DBG.InfoCT("There maybe something wrong with the server,please try again later");
    }

    public void CancelWaitError()
    {
      CancelInvoke("ShowWaitError");
    }

    #endregion Tool

    #region SaveLoad

    public void Save()
    {
#if DEBUG
      //DBG.blogInfo("QuestManager.Save()");

      //DBG.blogInfo($"OdinData.Data.BlackList: {OdinData.OdinPlusData.BlackList.Count}");
      //foreach (string s in OdinData.OdinPlusData.BlackList)
      //{
      //  DBG.blogInfo(s);
      //}

      //DBG.blogInfo($"OdinData.Data.BuzzKeys: {OdinData.OdinPlusData.BuzzKeys.Count}");
      //foreach (string s in OdinData.OdinPlusData.BuzzKeys)
      //{
      //  DBG.blogInfo(s);
      //}

      //DBG.blogInfo($"OdinData.Data.Credits: {OdinData.OdinPlusData.Credits}");
      //DBG.blogInfo($"OdinData.Data.QuestCount: {OdinData.OdinPlusData.QuestCount}");
      //DBG.blogInfo($"OdinData.Data.Quests: {OdinData.OdinPlusData.Quests.Count}");
      //foreach (var quest in OdinData.OdinPlusData.Quests)
      //{
      //  DBG.blogInfo(JsonSerializationProvider.ToJson(quest));
      //}

      //DBG.blogInfo($"OdinData.Data.Quests: {OdinData.OdinPlusData.SearchTaskList.Count}");
      //foreach (var kvp in OdinData.OdinPlusData.SearchTaskList)
      //{
      //  DBG.blogInfo($"key: {kvp.Key}, value: {kvp.Value}");
      //}

      //DBG.blogInfo($"OdinData.Data.hasTroll: {OdinData.OdinPlusData.hasTroll}");
      //DBG.blogInfo($"OdinData.Data.hasWolf: {OdinData.OdinPlusData.hasWolf}");

#endif

      OdinData.OdinPlusData.Quests.Clear();
#if DEBUG
      //DBG.blogInfo($"MyQuests.Values: {MyQuests.Values.Count}");
#endif
      foreach (var kvp in MyQuests)
      {
        OdinData.OdinPlusData.Quests.Add(kvp.Key, kvp.Value);
#if DEBUG
        //DBG.blogInfo($"key: {kvp.Key}, value: {JsonSerializationProvider.ToJson(kvp.Value)}");
#endif
      }
    }

    public void Load()
    {
      foreach (var quest in OdinData.OdinPlusData.Quests)
      {
        MyQuests.Add(quest.Value.ID, quest.Value);
      }
    }

    #endregion SaveLoad
  }
}
