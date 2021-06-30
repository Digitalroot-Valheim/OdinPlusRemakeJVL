using OdinPlusRemakeJVL.Common;
using System.Reflection;
using OdinPlusRemakeJVL.Common.Names;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
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
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
      if (prefab != null)
      {
        Object.DestroyImmediate(prefab.GetComponent<RandomAnimation>());
      }

      return prefab;
    }
  }
}
