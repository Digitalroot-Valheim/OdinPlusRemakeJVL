using OdinPlusRemakeJVL.Common;
using System.Reflection;
using OdinPlusRemakeJVL.Common.Names;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  /// <summary>
  /// A new prefab of Munin with it's default behaviors removed.
  /// </summary>
  internal class OrnamentalMunin : AbstractCustomPrefab
  {
    /// <inheritdoc />
    internal OrnamentalMunin() 
      : base(CustomPrefabNames.OrnamentalMunin, PrefabNames.Munin)
    {
    }

    /// <inheritdoc />
    protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
      if (prefab != null)
      {
        Object.DestroyImmediate(prefab.transform.Find("exclamation").gameObject);
        Object.DestroyImmediate(prefab.transform.GetComponentInChildren<Light>());
        Object.DestroyImmediate(prefab.GetComponent<Raven>());
      }

      return prefab;
    }
  }
}
