using System.Collections.Generic;
using OdinPlus.Managers;
using UnityEngine;

namespace OdinPlus.Items
{
	class OdinMeads : MonoBehaviour
	{
		//private static Dictionary<string, GameObject> MeadList = new Dictionary<string, GameObject>()
		private static GameObject MeadTasty;
		public static Dictionary<string, GameObject> MeadList = new Dictionary<string, GameObject>();
		private static GameObject Root;
		private void Awake()
		{
			Root = new GameObject("MeadList");
			Root.transform.SetParent(OdinPlus.PrefabParent.transform);
			Root.SetActive(false);

			var objectDB = ObjectDB.instance;
			MeadTasty = objectDB.GetItemPrefab("MeadTasty");

			//notice Init Here

			InitValMead();
			InitBuzzMead();

			OdinPlus.OdinPreRegister(MeadList, nameof(MeadList));
		}

		public static void InitValMead()
		{
			foreach (var item in StatusEffectsManager.ValDataList.Keys)
			{
				CreateValMeadPrefab(item);
			}
		}
		public static void CreateValMeadPrefab(string name)
		{
			GameObject go = Instantiate(MeadTasty, Root.transform);
			go.name = name;
			var id = go.GetComponent<ItemDrop>().m_itemData.m_shared;
			id.m_name = "$op_" + name + "_name";
			id.m_icons[0] = ResourceAssetManager.OdinMeadsIcons[name];
			id.m_description = "$op_" + name + "_desc";
			id.m_consumeStatusEffect = StatusEffectsManager.SElist[name];
			MeadList.Add(name, go);
		}
		public static void InitBuzzMead()
		{
			foreach (var item in StatusEffectsManager.BuzzList.Keys)
			{
				CreateValMeadPrefab(item);
			}
		}
	}
}
