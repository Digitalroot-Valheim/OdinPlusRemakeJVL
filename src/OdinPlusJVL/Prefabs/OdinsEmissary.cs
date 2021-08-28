using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinPlusJVL.Prefabs
{
  internal class OrnamentalKeeper : AbstractCustomPrefab
  {
    /// <inheritdoc />
    internal OrnamentalKeeper()
      : base(CustomPrefabNames.OrnamentalKeeper, PrefabNames.Keeper)
    {
    }

    #region Overrides of AbstractCustomPrefab

    /// <inheritdoc />
    private protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
      if (prefab != null)
      {
        Object.DestroyImmediate(prefab.GetComponent<ZNetView>());
        Object.DestroyImmediate(prefab.GetComponent<ZSyncTransform>());
        Object.DestroyImmediate(prefab.GetComponent<Rigidbody>());
      }

      return prefab;
    }

    #endregion
  }
}
