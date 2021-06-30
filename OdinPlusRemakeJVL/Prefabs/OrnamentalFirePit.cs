using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  internal class OrnamentalFirePit : ICreateable
  {
    public GameObject Create()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var prefab = PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalFirePit);

      if (prefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.OrnamentalFirePit}");
        prefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.OrnamentalFirePit, ItemNames.FirePit);
        if (prefab != null)
        {

          Object.DestroyImmediate(prefab.gameObject.transform.Find("PlayerBase").gameObject);
          Object.DestroyImmediate(prefab.GetComponent<WearNTear>());
          Object.DestroyImmediate(prefab.GetComponent<Fireplace>());
          prefab.transform.Find("_enabled_high").gameObject.SetActive(true);
          prefab.transform.Find("_enabled").gameObject.SetActive(true);
          prefab.GetComponent<Piece>().m_canBeRemoved = false;

          foreach (var item in prefab.GetComponentsInChildren<Aoe>())
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
