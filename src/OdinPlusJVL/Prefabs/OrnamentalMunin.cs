using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Prefabs
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
    private protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
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
