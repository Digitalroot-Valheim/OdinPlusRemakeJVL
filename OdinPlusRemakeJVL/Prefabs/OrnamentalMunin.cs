using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  internal class OrnamentalMunin : ICreateable
  {
    public GameObject Create()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var prefab = PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalMunin);
      if (prefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.OrnamentalMunin}");
        prefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.OrnamentalMunin, ItemNames.Munin);
        if (prefab != null)
        {
          Object.DestroyImmediate(prefab.transform.Find("exclamation").gameObject);
          Object.DestroyImmediate(prefab.transform.GetComponentInChildren<Light>());
          Object.DestroyImmediate(prefab.GetComponent<Raven>());
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
