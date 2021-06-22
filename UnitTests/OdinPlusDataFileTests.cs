using NUnit.Framework;
using OdinPlus.Common;
using OdinPlus.Data;
using OdinPlus.Quests;
using System;
using System.Collections.Generic;
// using UnityEngine;

namespace UnitTests
{
  [TestFixture]
  public class OdinPlusDataFileTests
  {
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void OdinPlusDataFileToJsonTest()
    {
      var odinPlusDataFile = new OdinPlusDataFile
      {
        Credits = 500,
        BlackList = new List<string> {"BlackListA", "BlackListB"},
        BuzzKeys = new List<string> {"BuzzKeysA", "BuzzKeysB"},
        QuestCount = 10,
        SearchTaskList = new Dictionary<string, int> {{"Five", 5}, {"Six", 6}},
        hasTroll = true,
        hasWolf = false,
        Quests = new Dictionary<string, Quest>()
      };

      Assert.That(odinPlusDataFile, Is.Not.Null);
      Assert.That(odinPlusDataFile.BlackList.Count, Is.EqualTo(2));
      Assert.That(odinPlusDataFile.BuzzKeys.Count, Is.EqualTo(2));
      Assert.That(odinPlusDataFile.SearchTaskList.Count, Is.EqualTo(2));

      var quest = new Quest
      {
        Key = 1,
        ID = "ID",
        QuestName = "QuestName",
        HintStart = "HintStart",
        HintTarget = "HintTarget",
        Level = 2,
        hasPIN = true,
        isMain = true,
        locName = "locName",
        m_index = 3,
        m_message = "m_message",
        m_ownerName = "m_ownerName",
        m_range = 4f,
        m_type = QuestType.Treasure,
        // m_pinPosition = new Vector3(1f, 2f, 3f),
        // m_realPostion = new Vector3(4f, 5f, 6f)
      };

      Assert.That(odinPlusDataFile.Quests, Is.Not.Null);
      Assert.That(quest, Is.Not.Null);
      odinPlusDataFile.Quests.Add("UnitTestQuest", quest);

      Assert.That(odinPlusDataFile.Quests.Count, Is.EqualTo(1));

      var json = JsonSerializationProvider.ToJson(odinPlusDataFile);

      Assert.That(json, Is.Not.Null);
      Assert.That(json, Is.Not.Empty);
      Console.WriteLine(JsonSerializationProvider.ToJson(odinPlusDataFile, true));

      // ReSharper disable once IdentifierTypo
      var opdf = JsonSerializationProvider.FromJson<OdinPlusDataFile>(json);
      Assert.That(opdf, Is.Not.Null);
      Assert.That(opdf.Credits, Is.EqualTo(odinPlusDataFile.Credits));
      Assert.That(opdf.QuestCount, Is.EqualTo(odinPlusDataFile.QuestCount));
      Assert.That(opdf.hasTroll, Is.EqualTo(odinPlusDataFile.hasTroll));
      Assert.That(opdf.hasWolf, Is.EqualTo(odinPlusDataFile.hasWolf));
      Assert.That(opdf.BlackList, Is.Not.Empty);
      Assert.That(opdf.BlackList, Has.Exactly(odinPlusDataFile.BlackList.Count).Items);

      foreach (var value in odinPlusDataFile.BlackList)
      {
        Assert.That(opdf.BlackList, Does.Contain(value));
      }

      foreach (var value in odinPlusDataFile.BuzzKeys)
      {
        Assert.That(opdf.BuzzKeys, Does.Contain(value));
      }

      foreach (var value in odinPlusDataFile.SearchTaskList.Keys)
      {
        Assert.That(opdf.SearchTaskList.Keys, Does.Contain(value));
        Assert.That(opdf.SearchTaskList[value], Is.EqualTo(odinPlusDataFile.SearchTaskList[value]));
      }

      Assert.That(opdf.Quests, Has.Exactly(odinPlusDataFile.Quests.Count).Items);

      foreach (var value in opdf.Quests.Keys)
      {
        Assert.That(opdf.Quests.Keys, Does.Contain(value));
        Assert.That(opdf.Quests[value].Key, Is.EqualTo(odinPlusDataFile.Quests[value].Key));
        Assert.That(opdf.Quests[value].ID, Is.EqualTo(odinPlusDataFile.Quests[value].ID));
        Assert.That(opdf.Quests[value].QuestName, Is.EqualTo(odinPlusDataFile.Quests[value].QuestName));
        Assert.That(opdf.Quests[value].HintStart, Is.EqualTo(odinPlusDataFile.Quests[value].HintStart));
        Assert.That(opdf.Quests[value].HintTarget, Is.EqualTo(odinPlusDataFile.Quests[value].HintTarget));
        Assert.That(opdf.Quests[value].Level, Is.EqualTo(odinPlusDataFile.Quests[value].Level));
        Assert.That(opdf.Quests[value].hasPIN, Is.EqualTo(odinPlusDataFile.Quests[value].hasPIN));
        Assert.That(opdf.Quests[value].isMain, Is.EqualTo(odinPlusDataFile.Quests[value].isMain));
        Assert.That(opdf.Quests[value].locName, Is.EqualTo(odinPlusDataFile.Quests[value].locName));
        Assert.That(opdf.Quests[value].m_index, Is.EqualTo(odinPlusDataFile.Quests[value].m_index));
        Assert.That(opdf.Quests[value].m_message, Is.EqualTo(odinPlusDataFile.Quests[value].m_message));
        Assert.That(opdf.Quests[value].m_ownerName, Is.EqualTo(odinPlusDataFile.Quests[value].m_ownerName));
        Assert.That(opdf.Quests[value].m_range, Is.EqualTo(odinPlusDataFile.Quests[value].m_range));
        Assert.That(opdf.Quests[value].m_type, Is.EqualTo(odinPlusDataFile.Quests[value].m_type));
        Assert.That(opdf.Quests[value].m_chain, Has.Exactly(odinPlusDataFile.Quests[value].m_chain.Count).Items);
        // Assert.That(opdf.Quests[value].m_pinPosition, Is.EqualTo(odinPlusDataFile.Quests[value].m_pinPosition));
        // Assert.That(opdf.Quests[value].m_pinPosition.x, Is.EqualTo(odinPlusDataFile.Quests[value].m_pinPosition.x));
        // Assert.That(opdf.Quests[value].m_pinPosition.y, Is.EqualTo(odinPlusDataFile.Quests[value].m_pinPosition.y));
        // Assert.That(opdf.Quests[value].m_pinPosition.z, Is.EqualTo(odinPlusDataFile.Quests[value].m_pinPosition.z));
        // Assert.That(opdf.Quests[value].m_realPostion, Is.EqualTo(odinPlusDataFile.Quests[value].m_realPostion));
        // Assert.That(opdf.Quests[value].m_realPostion.x, Is.EqualTo(odinPlusDataFile.Quests[value].m_realPostion.x));
        // Assert.That(opdf.Quests[value].m_realPostion.y, Is.EqualTo(odinPlusDataFile.Quests[value].m_realPostion.y));
        // Assert.That(opdf.Quests[value].m_realPostion.z, Is.EqualTo(odinPlusDataFile.Quests[value].m_realPostion.z));
      }
    }
  }
}
