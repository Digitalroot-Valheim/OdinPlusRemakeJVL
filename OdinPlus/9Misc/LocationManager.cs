using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HarmonyLib;

namespace OdinPlus
{
	internal class LocationManager : MonoBehaviour
	{
		private static Dictionary<Vector2i, ZoneSystem.LocationInstance> m_locationInstances = new Dictionary<Vector2i, ZoneSystem.LocationInstance>();
		public static List<string> BlackList = new List<string>();
		public static LocationManager instance;
		public static bool rpc = false;

		#region Mono
		private void Awake()
		{
			instance = this;
		}
		public static void Init()
		{
			instance.initRPC();
			if (ZNet.instance.IsServer())
			{
				BlackList = OdinData.Data.BlackList;
				GetValDictionary();
			}
		}
		#endregion Mono

		#region Init
		public static void GetValDictionary()
		{
			var a = Traverse.Create(ZoneSystem.instance).Field<Dictionary<Vector2i, ZoneSystem.LocationInstance>>("m_locationInstances").Value;
			foreach (var item in a)
			{
				m_locationInstances.Add(item.Key, item.Value);
			}
		}
		public static void RemoveBlackList()
		{
			foreach (var item in BlackList)
			{
				m_locationInstances.Remove(Tweakers.Pak(item));
			}
		}
		public static void Clear()
		{
			BlackList.Clear();
			m_locationInstances.Clear();
		}
		#endregion Init

		#region Feature
		public static void Remove(string id)
		{
			BlackList.Add(id);
			m_locationInstances.Remove(Tweakers.Pak(id));
		}

		public static bool GetLocationInstance(string id, out ZoneSystem.LocationInstance li)
		{
			var a = Traverse.Create(ZoneSystem.instance).Field<Dictionary<Vector2i, ZoneSystem.LocationInstance>>("m_locationInstances").Value;
			var key = Tweakers.Pak(id);
			if (a.ContainsKey(key))
			{
				li = a[key];
				return true;
			}
			li = default(ZoneSystem.LocationInstance);
			return false;
		}
		public static bool FindClosestLocation(string name, Vector3 point, out string id)
		{

			float num = 999999f;
			id = "0_0";
			bool result = false;
			foreach (var item in m_locationInstances)
			{
				float num2 = Vector3.Distance(item.Value.m_position, point);
				if (item.Value.m_location.m_prefabName == name && num2 < num)
				{
					num = num2;
					id = Tweakers.DepakVector2i(item.Key);
					result = true;
				}
			}
			return result;
		}
		public static bool FindClosestLocation(string name, Vector3 point, out Vector3 pos)
		{

			float num = 999999f;

			bool result = false;
			foreach (var item in m_locationInstances)
			{
				float num2 = Vector3.Distance(item.Value.m_position, point);
				if (item.Value.m_location.m_prefabName == name && num2 < num)
				{
					pos = item.Value.m_position;
					num = num2;
					result = true;
				}
			}
			pos = Vector3.zero;
			return result;
		}
		#endregion Feature

		#region Tool
		public static GameObject FindDungeon(Vector3 pos)
		{
			var loc = Location.GetLocation(pos);
			if (loc == null)
			{
				return null;
			}
			var dunPos = loc.transform.Find("Interior").transform.position;
			Collider[] array = Physics.OverlapBox(dunPos, new Vector3(60, 60, 60));
			DungeonGenerator comp;
			foreach (var item in array)
			{
				var c = item.transform;
				while (c.transform.parent != null)
				{
					if (c.TryGetComponent<DungeonGenerator>(out comp))
					{
						if (c.name.Contains("Clone"))
						{
							return c.gameObject;
						}
					}
					c = c.transform.parent;
				}

			}
			return null;
		}
		#endregion Tool

		#region RPC
		public void initRPC()
		{
			if (rpc)
			{
				return;
			}
			ZRoutedRpc.instance.Register<Vector3>("RPC_SetStartPos", new Action<long, Vector3>(this.RPC_SetStartPos));
			rpc = true;
			if (ZNet.instance.IsServer())
			{
				ZRoutedRpc.instance.Register("Rpc_GetStartPos", new Action<long>(this.Rpc_GetStartPos));
				return;
			}

		}
		public static void GetStartPos()
		{
			ZRoutedRpc.instance.InvokeRoutedRPC("Rpc_GetStartPos", new object[] { });
		}
		private void Rpc_GetStartPos(long sender)
		{
			DBG.blogWarning("Server got odin postion request");
			ZoneSystem.LocationInstance temp;
			ZoneSystem.instance.FindClosestLocation("StartTemple", Vector3.zero, out temp);
			ZRoutedRpc.instance.InvokeRoutedRPC(sender, "RPC_SetStartPos", new object[] { temp.m_position });
		}
		private void RPC_SetStartPos(long sender, Vector3 pos)
		{
			DBG.blogWarning("client  got odin postion " + pos);
			NpcManager.Root.transform.localPosition = pos + new Vector3(-6, 0, -8);
		}

		private void RPC_ClientInitDungeon(long sender, string name, Vector3 pos, string Id, int Key)
		{
			AddDungeonChest(name, pos, Id, Key);
		}
		private void AddDungeonChest(string name, Vector3 pos, string Id, int Key)
		{
			var DungeonRoot = new GameObject();
			if (name == "GoblinCamp2")
			{
				var dunPos = pos;
				if (!AddChest(dunPos, Id, Key))
				{
					var Reward = Instantiate(ZNetScene.instance.GetPrefab("LegacyChest" + (Key + 1).ToString()), dunPos, Quaternion.identity);
					Reward.GetComponent<LegacyChest>().ID = Id;
					DBG.blogWarning("Placed LegacyChest at Dungeon camp: " + Reward.transform.position);
					return;
				}
				return;
			}
			else
			{
				DungeonRoot = LocationManager.FindDungeon(pos);
			}
			if (DungeonRoot == null)
			{
				return;
			}
			if (!AddChest(DungeonRoot.transform.position, Id, Key))
			{
				Room[] array = DungeonRoot.GetComponentsInChildren<Room>();
				if (array.Length == 0) { return; }

				var array2 = array.Where(c => c.m_endCap != true).ToArray();

				if (array2 == null)
				{
					var a = array[array.Length.RollDice()];
					AddChest(a, Id, Key);
					return;
				}
				AddChest(array2[array2.Length.RollDice()], Id, Key);
				return;
			}
		}
		private bool AddChest(Vector3 pos, string Id, int Key)
		{
			Collider[] array = Physics.OverlapBox(pos, new Vector3(60, 60, 60));
			Container comp;
			foreach (var item in array)
			{
				var ci = item.transform;
				while (ci.transform.parent != null)
				{
					if (ci.TryGetComponent<Container>(out comp))
					{
						if (ci.name.Contains("Clone"))
						{
							var Reward = Instantiate(ZNetScene.instance.GetPrefab("LegacyChest" + (Key + 1).ToString()), comp.transform.position, Quaternion.identity);
							comp.GetInventory().RemoveAll();
							comp.GetComponent<ZNetView>().Destroy();
							Reward.GetComponent<LegacyChest>().ID = Id;

							DBG.blogWarning("Placed LegacyChest at Dungeon ctn: " + Reward.transform.position);
							return true;
						}
					}
					ci = ci.transform.parent;
				}
			}
			DBG.blogWarning("Cant Find Chest in dungeon");
			return false;
		}

		private void AddChest(Room room, string Id, int Key)
		{
			var y = room.GetComponentInChildren<RoomConnection>().transform.localPosition.y;
			var x = room.m_size.x / 2;
			var z = room.m_size.z / 2;
			var pos = new Vector3(0, y + 0.2f, 0) + room.transform.position;
			var Reward = Instantiate(ZNetScene.instance.GetPrefab("LegacyChest" + (Key + 1).ToString()));
			Reward.transform.localPosition = pos;
			Reward.GetComponent<LegacyChest>().ID = Id;
			DBG.blogWarning("Placed LegacyChest at Dungeon room: " + Reward.transform.localPosition);
			return;
		}
		#endregion RPC


	}
}