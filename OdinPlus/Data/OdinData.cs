using JetBrains.Annotations;
using OdinPlus.Common;
using OdinPlus.Items;
using OdinPlus.Managers;
using OdinPlus.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace OdinPlus.Data
{
  public class OdinData : MonoBehaviour
  {
    public static int Credits;

    public static Dictionary<string, int> ItemSellValue = new Dictionary<string, int>();

    public static OdinPlusDataFile OdinPlusData;

    public static Dictionary<string, MeadInfo> Meads;

    [UsedImplicitly]
    private void Awake()
    {
      if (DevTool.DisableSaving)
      {
        Credits = 1000;
      }

      BuildMeads(); // Create Meads



      OdinPlusData = new OdinPlusDataFile();
      OdinPlusData.Quests = new Dictionary<string, Quest>();
      if (Plugin.CFG_ItemSellValue.Value == "")
      {
        return;
      }

      string[] l1 = Plugin.CFG_ItemSellValue.Value.Split(';');
      for (int i = 0; i < l1.Length; i++)
      {
        string[] c = l1[i].Split(':');
        if (c.Length == 0)
        {
          continue;
        }

        try
        {
          ItemSellValue.Add(c[0], int.Parse(c[1]));
        }
        catch (Exception e)
        {
          DBG.blogWarning("CFG Error,Check Your ItemSellValue");
          DBG.blogWarning(e);
        }
      }
    }

    public static void AddKey(string key)
    {
      OdinPlusData.BuzzKeys.Add(key);
    }

    public static void RemoveKey(string key)
    {
      OdinPlusData.BuzzKeys.Remove(key);
    }

    public static bool GetKey(string key)
    {
      if (OdinPlusData.BuzzKeys.Contains(key))
      {
        return true;
      }

      return false;
    }

    private static void BuildMeads()
    {
      if (Meads != null) return;
      
      Meads = new Dictionary<string, MeadInfo>();
      foreach (var meadInfo in Items.MeadFactory.GetMeadList())
      {
        Meads.Add(meadInfo.FullName, meadInfo);
      }
    }

    #region Credits

    public static void AddCredits(int val, Transform m_head)
    {
      AddCredits(val);
      Player.m_localPlayer.m_skillLevelupEffects.Create(m_head.position, m_head.rotation, m_head);
    }

    public static bool RemoveCredits(int s)
    {
      if (Credits - s < 0)
      {
        return false;
      }

      Credits -= s;
      return true;
    }

    public static void AddCredits(int val)
    {
      Credits += val;
    }

    public static void AddCredits(int val, bool _notice)
    {
      AddCredits(val);
      if (_notice)
      {
        string n = String.Format("Odin Credits added : <color=lightblue><b>[{0}]</b></color>", Credits);
        MessageHud.instance.ShowBiomeFoundMsg(n, true); //trans
      }
    }

    #endregion Credits

    #region Save And Load

    public static void SaveOdinData(string name)
    {
      DBG.blogInfo("OdinData.SaveOdinData()");
      if (DevTool.DisableSaving)
      {
        OdinPlus.m_instance.isLoaded = true;
        return;
      }

      #region Save

      QuestManager.instance.Save();
      OdinPlusData.Credits = Credits;

      #endregion Save

      #region Serialize

      var file = new FileInfo(Path.Combine(Application.persistentDataPath, $"{name}.odinplus"));
      if (file.Exists)
      {
        file.CopyTo($"{file.FullName}.bak", true);
      }

      using (FileStream fileStream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
      {
        DBG.blogInfo($"Data.Quests: {OdinPlusData?.Quests?.Count}");
        DBG.blogInfo($"Data.Quests.Json: {JsonSerializationProvider.ToJson(OdinPlusData?.Quests)}");
        DBG.blogInfo($"Data.Quests.ToList.Json: {JsonSerializationProvider.ToJson(OdinPlusData?.Quests?.ToList())}");

        DBG.blogInfo($"Data.Quests.ForEach: {OdinPlusData?.Quests?.Count}");
        if (OdinPlusData.Quests != null)
        {
          foreach (var q in OdinPlusData.Quests)
          {
            DBG.blogInfo($"Json: {q}");
          }
        }

        string dat = JsonSerializationProvider.ToJson(OdinPlusData);
        DBG.blogInfo($"Json: {dat}");
        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
        {
          binaryWriter.Write(dat);
          binaryWriter.Flush();
          binaryWriter.Close();
        }

        //BinaryFormatter formatter = new BinaryFormatter();
        //formatter.Serialize(fileStream, Data);
        fileStream.Close();
      }

      #endregion Serialize

      DBG.blogWarning("OdinDataSaved:" + name);
    }

    public static void LoadOdinData(string name)
    {
      DBG.blogWarning("Starting loading data");
      if (DevTool.DisableSaving)
      {
        OdinPlus.m_instance.isLoaded = true;
        return;
      }

      #region Serial

      var file = new FileInfo(Path.Combine(Application.persistentDataPath, $"{name}.odinplus"));
      if (!file.Exists)
      {
        OdinPlus.m_instance.isLoaded = true;
        Credits = 100;
        DBG.blogWarning($"Profile not exists: {name}");
        return;
      }

      using (FileStream fileStream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
      {
        using (BinaryReader binaryReader = new BinaryReader(fileStream))
        {
          var str = binaryReader.ReadString();
          OdinPlusData = JsonSerializationProvider.FromJson<OdinPlusDataFile>(str);
        }

        fileStream.Close();
      }
      //BinaryFormatter formatter = new BinaryFormatter();
      //Data = (DataTable)formatter.Deserialize(fileStream);

      #endregion Serial

      #region Load

      Credits = OdinPlusData.Credits;
      QuestManager.instance.Load();
      LocationManager.BlackList = OdinPlusData.BlackList;
      LocationManager.RemoveBlackList();

      #endregion Load

      OdinPlus.m_instance.isLoaded = true;
      DBG.blogWarning("OdinDataLoaded:" + name);
    }

    #endregion Save And Load
  }
}
