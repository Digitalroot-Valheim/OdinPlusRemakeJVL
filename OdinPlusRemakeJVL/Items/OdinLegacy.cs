using System.Reflection;
using Jotunn.Entities;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using Log = OdinPlusRemakeJVL.Common.Log;

namespace OdinPlusRemakeJVL.Items
{
  internal static class OdinLegacy
  {
    public static void Create()
    {
      Log.Trace($"OdinPlusRemakeJVL.Items.OdinLegacy.{MethodBase.GetCurrentMethod().Name}()");
      CustomItem customItem = new CustomItem(ItemNames.OdinLegacy, ItemNames.TrophyGoblinShaman);
      Jotunn.Managers.ItemManager.Instance.AddItem(customItem);

      var itemDrop = customItem.ItemDrop;
      itemDrop.m_itemData.m_shared.m_name = $"$op_{ItemNames.OdinLegacy}_name";
      itemDrop.m_itemData.m_shared.m_description = $"$op_{ItemNames.OdinLegacy}_desc";
      itemDrop.m_itemData.m_shared.m_icons[0] = SpriteManager.Instance.GetSprite(ItemNames.OdinLegacy);
      itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.None;
      itemDrop.m_itemData.m_shared.m_maxStackSize = 100;
      itemDrop.m_itemData.m_shared.m_maxQuality = 5;
    }
  }
}
