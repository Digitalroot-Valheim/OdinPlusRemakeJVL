using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  internal class OrnamentalOdin : ICreateable
  {
    public GameObject Create()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var prefab = PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalOdin);

      if (prefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.OrnamentalOdin}");
        prefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.OrnamentalOdin, ItemNames.Odin);
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
      }

      if (prefab == null)
      {
        Log.Error($"[{GetType().Name}] Error with prefabs.");
        Log.Error($"[{GetType().Name}] prefab == null: true");
      }

      return prefab;
    }
  }
}
