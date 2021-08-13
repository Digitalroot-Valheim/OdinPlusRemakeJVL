using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Prefabs
{
  /// <summary>
  /// A new prefab of the GoblinShaman with it's default hostile behaviors removed.
  /// </summary>
  internal class OrnamentalGoblinShaman : AbstractCustomPrefab
  {
    /// <inheritdoc />
    internal OrnamentalGoblinShaman()
      : base(CustomPrefabNames.OrnamentalGoblinShaman, PrefabNames.GoblinShaman)
    {
    }

    /// <inheritdoc />
    private protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
      if (prefab != null)
      {
        Object.DestroyImmediate(prefab.GetComponent<RandomAnimation>());
      }

      return prefab;
    }
  }
}
