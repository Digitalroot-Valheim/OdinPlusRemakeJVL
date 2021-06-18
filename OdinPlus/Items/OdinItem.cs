using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using OdinPlus.Common;
using OdinPlus.Managers;
using UnityEngine;

namespace OdinPlus.Items
{
  class OdinItem : MonoBehaviour
  {
    private const string Namespace = "OdinPlus.Managers";
    //private static Dictionary<string, GameObject> MeadList = new Dictionary<string, GameObject>()
    private static GameObject MeadTasty;
    private static GameObject TrophyGoblinShaman;

    public static Dictionary<string, Sprite> PetItemList = new Dictionary<string, Sprite>
    {
      {OdinPlusItem.ScrollTroll, OdinPlus.TrollHeadIcon},
      {OdinPlusItem.ScrollWolf, OdinPlus.WolfHeadIcon}
    };

    public static Dictionary<string, GameObject> ObjectList = new Dictionary<string, GameObject>();

    public static GameObject Root;

    [UsedImplicitly]
    private void Awake()
    {
      Log.Trace($"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      Root = new GameObject("ObjectList");
      Root.transform.SetParent(OdinPlus.PrefabParent.transform);
      Root.SetActive(false);

      var objectDB = ObjectDB.instance;
      MeadTasty = objectDB.GetItemPrefab(OdinPlusItem.MeadTasty);
      TrophyGoblinShaman = objectDB.GetItemPrefab(OdinPlusItem.TrophyGoblinShaman);

      InitLegacy();
      InitPetItem();

      OdinPlus.OdinPreRegister(ObjectList, nameof(ObjectList));
    }

    private void InitPetItem()
    {
      Log.Trace($"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      foreach (var pet in PetItemList)
      {
        CreatePetItemPrefab(pet.Key, pet.Value);
      }
    }

    private void CreatePetItemPrefab(string iName, Sprite icon)
    {
      Log.Trace($"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      GameObject go = Instantiate(MeadTasty, Root.transform);
      go.name = iName;

      var id = go.GetComponent<ItemDrop>().m_itemData.m_shared;
      id.m_name = $"$op_{iName}_name";
      id.m_icons[0] = icon;
      id.m_description = $"$op_{iName}_desc";

      id.m_maxStackSize = 1;
      id.m_consumeStatusEffect = StatusEffectsManager.SElist[iName];

      go.GetComponent<ItemDrop>().m_itemData.m_quality = 4;
      id.m_maxQuality = 5;

      ObjectList.Add(iName, go);
    }

    private static void InitLegacy()
    {
      Log.Trace($"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      string name = OdinPlusItem.OdinLegacy;
      GameObject go = Instantiate(TrophyGoblinShaman, Root.transform);
      go.name = OdinPlusItem.OdinLegacy;
      var id = go.GetComponent<ItemDrop>().m_itemData.m_shared;
      id.m_name = $"$op_{name}_name";
      id.m_icons[0] = OdinPlus.OdinLegacyIcon;
      id.m_description = $"$op_{name}_desc";
      id.m_itemType = ItemDrop.ItemData.ItemType.None;

      id.m_maxStackSize = 10;
      id.m_maxQuality = 5;

      ObjectList.Add(name, go);
    }

    public static ItemDrop.ItemData GetItemData(string name)
    {
      Log.Trace($"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      return ObjectList[name].GetComponent<ItemDrop>().m_itemData;
    }

    public static GameObject GetObject(string name)
    {
      Log.Trace($"{Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      return ObjectList[name];
    }
  }
}
