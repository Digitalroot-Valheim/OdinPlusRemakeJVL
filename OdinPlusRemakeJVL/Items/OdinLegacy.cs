using System.Reflection;
using Jotunn.Entities;
using Jotunn.Managers;
using OdinPlusRemakeJVL.Managers;
using Log = OdinPlusRemakeJVL.Common.Log;

namespace OdinPlusRemakeJVL.Items
{
  internal static class OdinLegacy
  {
    public static void Create()
    {
      Log.Trace($"OdinPlusRemakeJVL.Items.OdinLegacy.{MethodBase.GetCurrentMethod().Name}()");
      CustomItem customItem = new CustomItem(OdinPlusItem.OdinLegacy, OdinPlusItem.TrophyGoblinShaman);
      ItemManager.Instance.AddItem(customItem);

      var itemDrop = customItem.ItemDrop;
      itemDrop.m_itemData.m_shared.m_name = $"$op_{OdinPlusItem.OdinLegacy}_name";
      itemDrop.m_itemData.m_shared.m_description = $"$op_{OdinPlusItem.OdinLegacy}_desc";
      itemDrop.m_itemData.m_shared.m_icons[0] = SpriteManager.Instance.GetSprite(OdinPlusItem.OdinLegacy);
      itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.None;
      itemDrop.m_itemData.m_shared.m_maxStackSize = 100;
      itemDrop.m_itemData.m_shared.m_maxQuality = 5;
    }
  }
}
