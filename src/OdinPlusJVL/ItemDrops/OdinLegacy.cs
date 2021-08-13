using Digitalroot.Valheim.Common;
using Jotunn.Entities;
using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;
using System.Reflection;

namespace OdinPlusJVL.ItemDrops
{
  internal static class OdinLegacy
  {
    public static void Create()
    {
      Log.Trace(Main.Instance, $"OdinPlusRemakeJVL.Items.OdinLegacy.{MethodBase.GetCurrentMethod().Name}()");
      CustomItem customItem = new CustomItem(ItemDropNames.OdinLegacy, ItemDropNames.TrophyGoblinShaman);
      Jotunn.Managers.ItemManager.Instance.AddItem(customItem);

      var itemDrop = customItem.ItemDrop;
      itemDrop.m_itemData.m_shared.m_name = $"$op_{ItemDropNames.OdinLegacy}_name";
      itemDrop.m_itemData.m_shared.m_description = $"$op_{ItemDropNames.OdinLegacy}_desc";
      itemDrop.m_itemData.m_shared.m_icons[0] = SpriteManager.Instance.GetSprite(ItemDropNames.OdinLegacy);
      itemDrop.m_itemData.m_shared.m_itemType = ItemDrop.ItemData.ItemType.None;
      itemDrop.m_itemData.m_shared.m_maxStackSize = 100;
      itemDrop.m_itemData.m_shared.m_maxQuality = 5;
    }
  }
}
