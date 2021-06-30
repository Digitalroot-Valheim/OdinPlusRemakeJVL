using System.Reflection;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  internal class OrnamentalGoblinShaman : ICreateable
  {
    public GameObject Create()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var prefab = PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalGoblinShaman);
      if (prefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.OrnamentalGoblinShaman}");
        prefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.OrnamentalGoblinShaman, ItemNames.GoblinShaman);
        if (prefab != null)
        {
          Object.DestroyImmediate(prefab.GetComponent<RandomAnimation>());
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
