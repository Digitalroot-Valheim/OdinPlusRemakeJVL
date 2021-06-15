using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OdinPlus.Common;
using OdinPlus.Managers;
using OdinPlus.Quests;
using UnityEngine;

namespace OdinPlus.Data
{
  public class OdinData : MonoBehaviour
	{
		#region Var
		#region serialization

    #endregion serialization
		#region interl
		public static int Credits;
		public static Dictionary<string, int> ItemSellValue = new Dictionary<string, int>();
		public static DataTable Data;
		#endregion interl

		#region GameCfgData
		public static Dictionary<string, int> MeadsValue = new Dictionary<string, int>
    {
{"ExpMeadS",5},
{"ExpMeadM",10},
{"ExpMeadL",20},
{"WeightMeadS",20},
{"WeightMeadM",30},
{"WeightMeadL",40},
{"InvisibleMeadS",30},
{"InvisibleMeadM",60},
{"InvisibleMeadL",90},
{"PickaxeMeadS",20},
{"PickaxeMeadM",30},
{"PickaxeMeadL",60},
{"BowsMeadS",20},
{"BowsMeadM",30},
{"BowsMeadL",60},
{"SwordsMeadS",20},
{"SwordsMeadM",30},
{"SwordsMeadL",60},
{"SpeedMeadsL",20},
{"AxeMeadS",20},
{"AxeMeadM",30},
{"AxeMeadL",60}
		};
		#endregion GameCfgData

		#endregion Var

		#region Mono
		private void Awake()
		{
			if (DevTool.DisableSaving)
			{
				Credits = 1000;
			}
			Data = new DataTable();
			Data.Quests = new Dictionary<string, Quest>();
			if (Plugin.CFG_ItemSellValue.Value == "") { return; }
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
		#endregion Mono

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
				MessageHud.instance.ShowBiomeFoundMsg(n, true);//trans
			}

		}
		#endregion Credits

		#region Feature
		public static void AddKey(string key)
		{
			Data.BuzzKeys.Add(key);
		}
		public static void RemoveKey(string key)
		{
			Data.BuzzKeys.Remove(key);
		}
		public static bool GetKey(string key)
		{
			if (Data.BuzzKeys.Contains(key))
			{
				return true;
			}
			return false;
		}
		#endregion Feature

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
			Data.Credits = Credits;
			#endregion Save

			#region Serialize
			var file = new FileInfo(Path.Combine(Application.persistentDataPath, $"{name}.odinplus"));
			if (file.Exists)
      {
        file.CopyTo($"{file.FullName}.bak", true);
      }

      using (FileStream fileStream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write))
      {
				DBG.blogInfo($"Data.Quests: {Data?.Quests?.Count}");
				DBG.blogInfo($"Data.Quests.Json: {JsonSerializationProvider.ToJson(Data?.Quests)}");
				DBG.blogInfo($"Data.Quests.ToList.Json: {JsonSerializationProvider.ToJson(Data?.Quests?.ToList())}");

				DBG.blogInfo($"Data.Quests.ForEach: {Data?.Quests?.Count}");
				if (Data.Quests != null)
        {
					foreach (var q  in Data.Quests)
          {
            DBG.blogInfo($"Json: {q}");
					}
				}

				string dat = JsonSerializationProvider.ToJson(Data);
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
          Data = JsonSerializationProvider.FromJson<DataTable>(str);
				}
        fileStream.Close();
			}
				//BinaryFormatter formatter = new BinaryFormatter();
				//Data = (DataTable)formatter.Deserialize(fileStream);

			#endregion Serial

			#region Load
			Credits = Data.Credits;
			QuestManager.instance.Load();
			LocationManager.BlackList = Data.BlackList;
			LocationManager.RemoveBlackList();
			#endregion Load

			OdinPlus.m_instance.isLoaded = true;
			DBG.blogWarning("OdinDataLoaded:" + name);
		}
		#endregion Save And Load

	}

  public class DataTable
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
