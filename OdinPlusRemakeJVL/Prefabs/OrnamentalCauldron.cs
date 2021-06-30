using OdinPlusRemakeJVL.Common;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  /// <summary>
  /// A new prefab of the Cauldron with it's default crafting station behaviors removed.
  /// </summary>
  internal class OrnamentalCauldron : AbstractCustomPrefab
  {
    /// <inheritdoc />
    internal OrnamentalCauldron()
      : base(CustomPrefabNames.OrnamentalCauldron, PrefabNames.Cauldron)
    {
    }

    /// <inheritdoc />
    protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
      if (prefab != null)
      {
        // Object.DestroyImmediate(prefab.GetComponent<WearNTear>());
        // Object.DestroyImmediate(prefab.GetComponent<CraftingStation>());
        prefab.transform.Find("HaveFire").gameObject.SetActive(true);
        prefab.GetComponent<Piece>().m_canBeRemoved = false;
      }

      return prefab;
    }
  }
}
