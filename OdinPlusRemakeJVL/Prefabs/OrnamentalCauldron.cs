using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  internal class OrnamentalCauldron : ICreateable
  {
    public GameObject Create()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var prefab = PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalCauldron);

      if (prefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.OrnamentalCauldron}");
        prefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.OrnamentalCauldron, ItemNames.Cauldron);
        if (prefab != null)
        {
          Object.DestroyImmediate(prefab.GetComponent<WearNTear>());
          Object.DestroyImmediate(prefab.GetComponent<CraftingStation>());
          prefab.transform.Find("HaveFire").gameObject.SetActive(true);
          prefab.GetComponent<Piece>().m_canBeRemoved = false;
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
