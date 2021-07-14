using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  /// <summary>
  /// A new prefab of the FirePit that is always burning and does not require wood. Some default behaviors have been removed.
  /// </summary>
  internal class OrnamentalFirePit : AbstractCustomPrefab
  {
    /// <inheritdoc />
    internal OrnamentalFirePit()
      : base(CustomPrefabNames.OrnamentalFirePit, PrefabNames.FirePit)
    {
    }

    /// <inheritdoc />
    private protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");

      if (prefab != null)
      {
        Object.DestroyImmediate(prefab.gameObject.transform.Find("PlayerBase").gameObject);
        Object.DestroyImmediate(prefab.GetComponent<WearNTear>());
        Object.DestroyImmediate(prefab.GetComponent<Fireplace>());
        prefab.transform.Find("_enabled_high").gameObject.SetActive(true);
        prefab.transform.Find("_enabled").gameObject.SetActive(true);

        foreach (var item in prefab.GetComponentsInChildren<Aoe>())
        {
          Object.DestroyImmediate(item);
        }
      }

      return prefab;
    }
  }
}
