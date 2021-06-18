using HarmonyLib;
using OdinPlus.Common;
using OdinPlus.Data;
using OdinPlus.Items;
using OdinPlus.Managers;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

//||X||may be setup NPC somewhere else 
//||X||Hi i am the manager ,spwan npc Somewhere Else;
namespace OdinPlus
{

  public class OdinPlus : MonoBehaviour
	{
    public static bool IsInitialized;
		public static bool IsNPCInitialized;
		public static bool IsPetInitialized;
		public static bool IsRegistered;
		public bool IsLoaded;
		public static OdinPlus Instance;
		public static bool ZnsInitialized;

    public static List<string> traderNameList = new List<string>();
		//public static List<int> preRegList = new List<int>();
		public static Dictionary<int, GameObject> odbRegList = new Dictionary<int, GameObject>();
		public static Dictionary<int, GameObject> znsRegList = new Dictionary<int, GameObject>();

    public static GameObject Root;
		public static GameObject PrefabParent;

    public static Sprite OdinCreditIcon;
		public static List<Sprite> OdinMeadsIcon = new List<Sprite>();
		public static Dictionary<string, Sprite> OdinMeadsIcons = new Dictionary<string, Sprite>();
		public static List<Sprite> OdinSEIcon = new List<Sprite>();
		public static Sprite TrollHeadIcon;
		public static Sprite WolfHeadIcon;
		public static Sprite CoinsIcon;
		public static Sprite OdinLegacyIcon;

    #region Mono
		private void Awake()
		{
			Instance = this;
			Root = this.gameObject;

			PrefabParent = new GameObject("OdinPlusPrefabs");
			PrefabParent.SetActive(false);
			PrefabParent.transform.SetParent(Root.transform);
			PrefabParent.AddComponent<LocationMarker>();
			
			Root.AddComponent<OdinData>();
			Root.AddComponent<QuestManager>();

			
			Root.AddComponent<StatusEffectsManager>();
		}
		#endregion Mono

		#region Patch
		public static void Initialize()
		{
      if (ZNetScene.instance == null || ZNetScene.instance.m_prefabs == null || ZNetScene.instance.m_prefabs.Count <= 0)
      {
        Log.Debug("ZNetScene.instance is null");
        return;
      }
			initAssets();
			Root.AddComponent<LocationManager>();
			Root.AddComponent<OdinMeads>();
			Root.AddComponent<OdinItem>();
			Root.AddComponent<PetManager>();
			// Root.AddComponent<PrefabManager>();
			// Root.AddComponent<FxAssetManager>();
      FxAssetManager.Instance.Initialize();
			PrefabManager.Instance.Initialize();



			IsInitialized = true;
		}

    public static void PreObjectDBHook(ObjectDB odb)
		{
			StatusEffectsManager.Register(odb);
		}

		public static void PostObjectDBHook()
		{
			ValRegister(ObjectDB.instance);
		}
		public static void PreZNetSceneHook(ZNetScene zns)
		{
      if (zns == null || zns.m_prefabs == null || zns.m_prefabs.Count <= 0)
      {
        return;
      }
			ValRegister(zns);
      PrefabManager.Instance.PostInitialize();
		}
		public static void PostZNetSceneHook()
		{
			if (!ZnsInitialized)
			{
        FxAssetManager.Instance.PostInitialize();

				if (!PetManager.isInit)
				{
					PetManager.Init();
				}

				PrefabManager.Instance.PostInitialize();

				HumanManager.Init();
				ZnsInitialized = true;
			}

			ValRegister();
		}
		public static void PostZone()
		{

			LocationManager.Init();
			InitNPC();
			if (ZNet.instance != null && ZNet.instance.IsDedicated() && ZNet.instance.IsServer())
			{
				OdinData.LoadOdinData(ZNet.instance.GetWorldName());
			}
		}
		public static void InitNPC()
		{
			Root.AddComponent<NpcManager>();
			IsNPCInitialized = true;
		}
		public static void Clear()
		{
			PetManager.Clear();
			QuestManager.Instance.Clear();
			LocationManager.Clear();
			Destroy(Root.GetComponent<NpcManager>());
			IsNPCInitialized = false;
		}
		#endregion Patch

		#region Tool


		#endregion Tool

		#region Assets
		public static void initAssets()
		{
			OdinCreditIcon = ObjectDB.instance.GetItemPrefab("HelmetOdin").GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			OdinSEIcon.Add(OdinCreditIcon);
			TrollHeadIcon = ObjectDB.instance.GetItemPrefab("TrophyFrostTroll").GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			WolfHeadIcon = ObjectDB.instance.GetItemPrefab("TrophyWolf").GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			CoinsIcon = ObjectDB.instance.GetItemPrefab("Coins").GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			OdinLegacyIcon = Util.LoadSpriteFromTexture(Util.LoadTextureRaw(Util.GetResource(Assembly.GetCallingAssembly(), "OdinPlus.Resources.OdinLegacy.png")), 100f);
			//AddIcon("explarge", 0);
			AddValIcon("MeadTasty", 0);
		}
		public static void AddIcon(string name, int list)
		{
			Sprite a = Util.LoadSpriteFromTexture(Util.LoadTextureRaw(Util.GetResource(Assembly.GetCallingAssembly(), "OdinPlus.Resources." + name + ".png")), 100f);
			OdinMeadsIcons.Add(name, a);
		}
		public static void AddValIcon(string name, int list)
		{
			Sprite a = ObjectDB.instance.GetItemPrefab(name).GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			OdinMeadsIcons.Add(name, a);
		}
		#endregion Assets

		#region Feature
		public static void ValRegister(ObjectDB odb)
		{

			var m_itemByHash = Traverse.Create(odb).Field<Dictionary<int, GameObject>>("m_itemByHash").Value;
			foreach (var item in odbRegList)
			{
				m_itemByHash.Add(item.Key, item.Value);
				odb.m_items.Add(item.Value);
			}
			Log.Debug("Register to ODB");
		}
		public static void ValRegister(ZNetScene zns)
		{
      if (zns == null || zns.m_prefabs == null || zns.m_prefabs.Count <= 0)
      {
        return;
      }
			foreach (var item in odbRegList.Values)
			{
				zns.m_prefabs.Add(item);
			}
      Log.Debug("Register odb to zns");
		}
		public static void ValRegister()
		{
			var m_namedPrefabs = Traverse.Create(ZNetScene.instance).Field<Dictionary<int, GameObject>>("m_namedPrefabs").Value;
			foreach (var item in znsRegList)
			{
				ZNetScene.instance.m_prefabs.Add(item.Value);
				m_namedPrefabs.Add(item.Key, item.Value);
			}
			IsRegistered = true;
      Log.Debug("Register zns");
		}
		public static void OdinPreRegister(Dictionary<string, GameObject> list, string name)
		{
			foreach (var item in list)
			{
				odbRegList.Add(item.Key.GetStableHashCode(), item.Value);
			}
      Log.Debug("Register " + name + " for ODB");
		}
		public static void OdinPostRegister(Dictionary<string, GameObject> list)
		{
			foreach (var item in list)
			{
				znsRegList.Add(item.Key.GetStableHashCode(), item.Value);
			}
		}
		public static void PostRegister(GameObject go)
		{
			znsRegList.Add(go.name.GetStableHashCode(), go);
		}
		public static void PreRegister(GameObject go)
		{
			odbRegList.Add(go.name.GetStableHashCode(), go);
		}
		public static void UnRegister()
		{
			var odb = ObjectDB.instance;
			var zns = ZNetScene.instance;
			var m_itemByHash = Traverse.Create(odb).Field<Dictionary<int, GameObject>>("m_itemByHash").Value;
			var m_namedPrefabs = Traverse.Create(ZNetScene.instance).Field<Dictionary<int, GameObject>>("m_namedPrefabs").Value;
			odb.m_items.RemoveList<int, GameObject>(odbRegList);
			m_itemByHash.RemoveList<int, GameObject>(odbRegList);
			zns.m_prefabs.RemoveList<int, GameObject>(odbRegList);
			m_namedPrefabs.RemoveList<int, GameObject>(odbRegList);
			zns.m_prefabs.RemoveList<int, GameObject>(znsRegList);
			m_namedPrefabs.RemoveList<int, GameObject>(znsRegList);
			foreach (var item in StatusEffectsManager.SElist.Values)
			{
				odb.m_StatusEffects.Remove(item);
			}
			IsRegistered = false;
			Instance.IsLoaded = false;
      Log.Warning("UnRegister all list");
		}
		#endregion Feature
		#region Debug
		public void Reset()
		{
			initAssets();
			Root.AddComponent<StatusEffectsManager>();
			Root.AddComponent<OdinMeads>();
			Root.AddComponent<OdinItem>();
			Root.AddComponent<PetManager>();
			// Root.AddComponent<PrefabManager>();
			Root.AddComponent<QuestManager>();
			Root.AddComponent<LocationManager>();
			// Root.AddComponent<FxAssetManager>();
			//Plugin.RegisterRpcAction();

			IsInitialized = true;

			//PostObjectDBHook();
			var m_namedPrefabs = Traverse.Create(ZNetScene.instance).Field<Dictionary<int, GameObject>>("m_namedPrefabs").Value;
			foreach (var item in odbRegList)
			{
				ZNetScene.instance.m_prefabs.Add(item.Value);
				m_namedPrefabs.Add(item.Key, item.Value);
			}
			//PostZone();
			PostZNetSceneHook();
			
			NpcManager.RavenPrefab = Tutorial.instance.m_ravenPrefab.transform.Find("Munin").gameObject;
			//InitNPC();
			IsLoaded = true;
		}
		#endregion Debug

	}
}