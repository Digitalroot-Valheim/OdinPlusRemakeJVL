using HarmonyLib;
using OdinPlus.Common;
using OdinPlus.Data;
using OdinPlus.Managers;
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

    private void Awake_d()
		{
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
			Instance = this;
			Root = gameObject;

			PrefabParent = new GameObject("OdinPlusPrefabs");
			PrefabParent.SetActive(false);
			PrefabParent.transform.SetParent(Root.transform);
			PrefabParent.AddComponent<LocationMarker>();
			
			Root.AddComponent<OdinData>();
			Root.AddComponent<QuestManager>();
			
			// Root.AddComponent<StatusEffectsManager>();
		}

    #region Patch
		public static void Initialize()
		{
			// Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}()");
			if (ZNetScene.instance == null || ZNetScene.instance.m_prefabs == null || ZNetScene.instance.m_prefabs.Count <= 0)
      {
        Log.Debug("ZNetScene.instance is null");
        return;
      }
			ResourceAssetManager.Instance.Initialize();
      InitAssets();
			Root.AddComponent<LocationManager>();
			Root.AddComponent<PetManager>();
      StatusEffectsManager.Instance.Initialize();
			FxAssetManager.Instance.Initialize();
			OdinMeadsManager.Instance.Initialize();
      OdinItemManager.Instance.Initialize();
			PrefabManager.Instance.Initialize();

      ResourceAssetManager.Instance.PostInitialize();
      StatusEffectsManager.Instance.PostInitialize();
			FxAssetManager.Instance.PostInitialize();
      OdinMeadsManager.Instance.PostInitialize();
      OdinItemManager.Instance.PostInitialize();
      PrefabManager.Instance.PostInitialize();
			IsInitialized = true;
		}

  //  public static void PreObjectDBHook(ObjectDB objectDB)
		//{
  //    Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}(ObjectDB.name={objectDB.name})");
		//	StatusEffectsManager.Instance.Register(objectDB);
		//}

		public static void PostObjectDBHook()
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (!Util.IsObjectDBReady())
      {
        Log.Debug("ObjectDB is not ready - Skipping");
        return;
      }
			ValRegister(ObjectDB.instance);
		}
		public static void PreZNetSceneHook(ZNetScene zNetScene)
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}(ZNetScene.name={zNetScene.name})");
      if (ZNetScene.instance == null || ZNetScene.instance.m_prefabs == null || ZNetScene.instance.m_prefabs.Count <= 0)
      {
        Log.Debug("ZNetScene.instance is null");
        return;
      }
			ValRegister(zNetScene);
      // PrefabManager.Instance.PostInitialize();
		}
		public static void PostZNetSceneHook()
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
			if (!ZnsInitialized)
			{
				if (!PetManager.isInit)
				{
					PetManager.Init();
				}
				HumanManager.Init();

				ZnsInitialized = true;
			}

			ValRegister();
		}
		public static void PostZone()
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
			LocationManager.Init();
      OdinItemManager.Instance.PostInitialize();
			InitNPC();
			if (ZNet.instance != null && ZNet.instance.IsDedicated() && ZNet.instance.IsServer())
			{
				OdinData.LoadOdinData(ZNet.instance.GetWorldName());
			}
		}
		public static void InitNPC()
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
			Root.AddComponent<NpcManager>();
			IsNPCInitialized = true;
		}
		public static void Clear()
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
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
		public static void InitAssets()
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
			OdinCreditIcon = ObjectDB.instance.GetItemPrefab(OdinPlusItem.HelmetOdin).GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			OdinSEIcon.Add(OdinCreditIcon);
			TrollHeadIcon = ObjectDB.instance.GetItemPrefab(OdinPlusItem.TrophyFrostTroll).GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			WolfHeadIcon = ObjectDB.instance.GetItemPrefab(OdinPlusItem.TrophyWolf).GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			CoinsIcon = ObjectDB.instance.GetItemPrefab(OdinPlusItem.Coins).GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			OdinLegacyIcon = Util.LoadSpriteFromTexture(Util.LoadTextureRaw(Util.GetResource(Assembly.GetCallingAssembly(), "OdinPlus.Resources.OdinLegacy.png")), 100f);
			//AddIcon("explarge", 0);
			AddValIcon(OdinPlusItem.MeadTasty, 0);
		}
		public static void AddIcon(string name, int list)
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}({name}, {list})");
			Sprite a = Util.LoadSpriteFromTexture(Util.LoadTextureRaw(Util.GetResource(Assembly.GetCallingAssembly(), "OdinPlus.Resources." + name + ".png")), 100f);
			OdinMeadsIcons.Add(name, a);
		}
		public static void AddValIcon(string name, int list)
		{
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}({name}, {list})");
			Sprite a = ObjectDB.instance.GetItemPrefab(name).GetComponent<ItemDrop>().m_itemData.m_shared.m_icons[0];
			OdinMeadsIcons.Add(name, a);
		}
		#endregion Assets

		#region Feature
		public static void ValRegister(ObjectDB objectDB)
		{
      // Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}(ObjectDB.name={objectDB.name})");

			// ToDo: Refactor how this loads into ObjectDB so the hash does not need to be injected. objectDB.UpdateItemHashes();
			var m_itemByHash = Traverse.Create(objectDB).Field<Dictionary<int, GameObject>>("m_itemByHash").Value;
			foreach (var item in odbRegList)
			{
				m_itemByHash.Add(item.Key, item.Value);
				objectDB.m_items.Add(item.Value);
			}
			
			Log.Debug("Register to ObjectDB");
		}
		public static void ValRegister(ZNetScene zNetScene)
		{
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}(ZNetScene.name={zNetScene.name})");
			if (ZNetScene.instance == null || ZNetScene.instance.m_prefabs == null || ZNetScene.instance.m_prefabs.Count <= 0)
      {
        Log.Debug("ZNetScene.instance is null");
        return;
      }
			foreach (var item in odbRegList.Values)
			{
        if (zNetScene.m_prefabs.Contains(item)) continue;
				zNetScene.m_prefabs.Add(item);
			}
      Log.Debug("Register objectDB to zNetScene");
		}
		public static void ValRegister()
		{
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}()");
			var m_namedPrefabs = Traverse.Create(ZNetScene.instance).Field<Dictionary<int, GameObject>>("m_namedPrefabs").Value;
			foreach (var item in znsRegList)
			{
				ZNetScene.instance.m_prefabs.Add(item.Value);
				m_namedPrefabs.Add(item.Key, item.Value);
			}
			IsRegistered = true;
      Log.Debug("Register zNetScene");
		}
		public static void OdinPreRegister(Dictionary<string, GameObject> list, string name)
		{
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}(Dictionary<string, GameObject>, {name})");
			foreach (var item in list)
			{
        if (odbRegList.ContainsKey(item.Key.GetStableHashCode())) continue;
				odbRegList.Add(item.Key.GetStableHashCode(), item.Value);
			}
      Log.Debug("Register " + name + " for ODB");
		}
		public static void OdinPostRegister(Dictionary<string, GameObject> list)
		{
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}(Dictionary<string, GameObject>)");
			foreach (var item in list)
			{
				znsRegList.Add(item.Key.GetStableHashCode(), item.Value);
			}
		}
		public static void PostRegister(GameObject gameObject)
		{
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}(GameObject.name={gameObject.name})");
			znsRegList.Add(gameObject.name.GetStableHashCode(), gameObject);
		}
		public static void PreRegister(GameObject gameObject)
		{
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}(GameObject.name={gameObject.name})");
			odbRegList.Add(gameObject.name.GetStableHashCode(), gameObject);
		}
		public static void UnRegister()
		{
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}()");
			ObjectDB.instance.m_items.RemoveList(odbRegList);
      ObjectDB.instance.m_itemByHash.RemoveList(odbRegList);
      ZNetScene.instance.m_prefabs.RemoveList(odbRegList);
      ZNetScene.instance.m_namedPrefabs.RemoveList(odbRegList);
      ZNetScene.instance.m_prefabs.RemoveList(znsRegList);
      ZNetScene.instance.m_namedPrefabs.RemoveList(znsRegList);
			StatusEffectsManager.Instance.UnRegister();

			IsRegistered = false;
			Instance.IsLoaded = false;
      Log.Debug("UnRegister all list");
		}
		#endregion Feature
		#region Debug
		public void Reset()
		{
      Log.Trace($"OdinPlus.OdinPlus.{MethodBase.GetCurrentMethod().Name}()");
			InitAssets();
			// Root.AddComponent<StatusEffectsManager>();
			// Root.AddComponent<OdinMeadsManager>();
			// Root.AddComponent<OdinItemManager>();
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