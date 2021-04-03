using System;
using System.Collections.Generic;
using UnityEngine;

namespace OdinPlus
{
	public class LocationMarker : MonoBehaviour
	{
		#region Var
		public static Dictionary<string, LocationMarker> MarkList = new Dictionary<string, LocationMarker>();
		public static GameObject Prefab;

		#region Setting
		public string ID = "";
		public string owner = "";
		#endregion Setting
		#region Internal
		private ZNetView m_nview;
		private LocationProxy m_locationProxy;
		#endregion Internal
		#region Out
		private List<Container> m_container = new List<Container>();
		private Vector3 m_pos;

		#endregion Out

		#endregion Var
		#region Mono

		#endregion Mono
		private void Awake()
		{
			//add
			m_nview = GetComponent<ZNetView>();
			if (m_nview.GetZDO() == null)
			{
				DBG.blogWarning("Mark Report zdo null");
				return;
			}
			ID = ZoneSystem.instance.GetZone(transform.position).Pak();
			if (m_nview.GetZDO().GetBool("Used", false))
			{
				ZNetScene.instance.Destroy(gameObject);
				//add remove list
			}
			if (m_nview.IsOwner())
			{
				DBG.blogWarning("I am Owner");
			}
			else
			{
				DBG.blogWarning("I am not owner");
			}
			m_locationProxy = transform.parent.parent.GetComponent<LocationProxy>();
			m_pos = m_locationProxy.transform.position;
			DBG.blogWarning("Mark report zdo get");
			MarkList.Add(ID, this);
		}
		private void Start()
		{
			DBG.blogWarning("Start");
		}
		private void OnDestroy()
		{
			MarkList.Remove(ID);
		}
		public void Used()
		{
			m_nview.GetZDO().Set("Used", true);
		}

		#region Start
		public static void CreatePrefab()
		{
			GameObject go = new GameObject("LocMark");
			go.transform.SetParent(OdinPlus.PrefabParent.transform);
			go.AddComponent<LocationMarker>();
			go.AddComponent<ZNetView>();
			PrefabManager.PrefabList.Add(go.name, go);
			Prefab = go;
		}
		public static void HackLoctaions()
		{
			var locPar = GameObject.Find("/_Locations").transform;
			for (int i = 0; i < locPar.childCount; i++)
			{
				var par = locPar.GetChild(i);
				for (int k = 0; k < par.childCount; k++)
				{
					var go = Instantiate(Prefab, par.GetChild(k));
					go.name = ("LocMark");
					DBG.blogWarning(go.transform.parent.name+" Hacked");
				}
			}
		}
		#endregion Start
	}
}

