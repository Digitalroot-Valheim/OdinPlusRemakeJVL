using JetBrains.Annotations;
using OdinPlus.Common;
using OdinPlus.Items;
using OdinPlus.Managers;
using OdinPlus.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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

      OdinPlusData = new OdinPlusDataFile {Quests = new Dictionary<string, Quest>()};
      if (Main.ConfigEntryItemSellValue.Value == "")
      {
        return;
      }

      string[] l1 = Main.ConfigEntryItemSellValue.Value.Split(';');
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
      DBG.blogInfo($"AddKey({key})");
      OdinPlusData.BuzzKeys.Add(key);
    }

    public static void RemoveKey(string key)
    {
      DBG.blogInfo($"RemoveKey({key})");
      OdinPlusData.BuzzKeys.Remove(key);
    }

    public static bool GetKey(string key)
    {
      DBG.blogInfo($"GetKey({key})");
      if (OdinPlusData.BuzzKeys.Contains(key))
      {
        DBG.blogInfo($"Key Found");
        return true;
      }
      DBG.blogInfo($"Key Not Found");
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
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}()");
      if (DevTool.DisableSaving)
      {
        OdinPlus.Instance.IsLoaded = true;
        return;
      }

      #region Save

      QuestManager.Instance.Save();
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
#if DEBUG
        // DBG.blogInfo($"Data.Quests: {OdinPlusData?.Quests?.Count}");
        // DBG.blogInfo($"Data.Quests.Json: {JsonSerializationProvider.ToJson(OdinPlusData?.Quests)}");
        // DBG.blogInfo($"Data.Quests.ToList.Json: {JsonSerializationProvider.ToJson(OdinPlusData?.Quests?.ToList())}");
        //
        // DBG.blogInfo($"Data.Quests.ForEach: {OdinPlusData?.Quests?.Count}");
        // if (OdinPlusData.Quests != null)
        // {
        //   foreach (var q in OdinPlusData.Quests)
        //   {
        //     DBG.blogInfo($"Json: {q}");
        //   }
        // }
#endif
        string dat = JsonSerializationProvider.ToJson(OdinPlusData);
#if DEBUG
        DBG.blogInfo($"Json: {dat}");
#endif
        using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
        {
          binaryWriter.Write(dat);
          binaryWriter.Flush();
          binaryWriter.Close();
        }

        fileStream.Close();
      }

      #endregion Serialize

      Log.Debug("OdinDataSaved:" + name);
    }

    public static void LoadOdinData(string name)
    {
      // Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}()");

      Log.Debug("Starting loading data");
      if (DevTool.DisableSaving)
      {
        OdinPlus.Instance.IsLoaded = true;
        return;
      }

      #region Serial

      var file = new FileInfo(Path.Combine(Application.persistentDataPath, $"{name}.odinplus"));
      if (!file.Exists)
      {
        OdinPlus.Instance.IsLoaded = true;
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
      QuestManager.Instance.Load();
      LocationManager.BlackList = OdinPlusData.BlackList;
      LocationManager.RemoveBlackList();

      #endregion Load

      OdinPlus.Instance.IsLoaded = true;
      DBG.blogWarning("OdinDataLoaded:" + name);
    }

    #endregion Save And Load
  }
}
