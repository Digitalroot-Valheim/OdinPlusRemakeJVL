using OdinPlusRemakeJVL.Common;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  /// <summary>
  /// A new prefab of Odin with it's default behaviors removed.
  /// </summary>
  internal class OrnamentalOdin : AbstractCustomPrefab
  {
    /// <inheritdoc />
    internal OrnamentalOdin() 
      : base(CustomPrefabNames.OrnamentalOdin, PrefabNames.Odin)
    {
    }

    /// <inheritdoc />
    protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
      if (prefab != null)
      {
        Object.DestroyImmediate(prefab.GetComponent<ZNetView>());
        Object.DestroyImmediate(prefab.GetComponent<ZSyncTransform>());
        Object.DestroyImmediate(prefab.GetComponent<Odin>());
        Object.DestroyImmediate(prefab.GetComponent<Rigidbody>());

        foreach (var item in prefab.GetComponentsInChildren<Aoe>())
        {
          Object.DestroyImmediate(item);
        }

        foreach (var item in prefab.GetComponentsInChildren<EffectArea>())
        {
          Object.DestroyImmediate(item);
        }
      }

      return prefab;
    }
  }
}
