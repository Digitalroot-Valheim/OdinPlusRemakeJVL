using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Prefabs
{
  /// <summary>
  /// A new prefab of the Cauldron with it's default behaviors removed.
  /// </summary>
  internal class OrnamentalCauldron : AbstractCustomPrefab
  {
    /// <inheritdoc />
    internal OrnamentalCauldron()
      : base(CustomPrefabNames.OrnamentalCauldron, PrefabNames.Cauldron)
    {
    }

    /// <inheritdoc />
    private protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
      if (prefab != null)
      {
        Object.DestroyImmediate(prefab.GetComponent<WearNTear>());
        Object.DestroyImmediate(prefab.GetComponent<CraftingStation>());
      }

      return prefab;
    }
  }
}
