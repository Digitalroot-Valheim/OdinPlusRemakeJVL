using Jotunn.Managers;
using OdinPlusRemakeJVL.Common;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  internal class OdinPot : AbstractNpc<OdinNpc>
  {
    public GameObject FirePit;
    public GameObject Cauldron;

    public void Awake()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var firePrefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.FirePit, ItemNames.FirePit);
      var cauldronPrefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.Cauldron, ItemNames.Cauldron);
      // Log.Trace($"[{GetType().Name}] firePrefab == null: {firePrefab == null}");
      // Log.Trace($"[{GetType().Name}] cauldronPrefab == null: {cauldronPrefab == null}");

      if (firePrefab == null) return;
      if (cauldronPrefab == null) return;

      Log.Trace($"[{GetType().Name}] Remove Prefab stuff we do not want.");
      Destroy(firePrefab.gameObject.transform.Find("PlayerBase").gameObject);
      firePrefab.transform.Find("_enabled_high").gameObject.SetActive(true);
      cauldronPrefab.transform.Find("HaveFire").gameObject.SetActive(true);
      // Cauldron.name = "$op_pot_name";
      Log.Trace($"[{GetType().Name}] Prefabs cleaned up");

      PrefabManager.Instance.AddPrefab(firePrefab);
      PrefabManager.Instance.AddPrefab(cauldronPrefab);
    }

    public override void Start()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      //var firePrefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.FirePit, ItemNames.FirePit);
      //var cauldronPrefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.Cauldron, ItemNames.Cauldron);
      //Log.Trace($"[{GetType().Name}] firePrefab == null: {firePrefab == null}");
      //Log.Trace($"[{GetType().Name}] cauldronPrefab == null: {cauldronPrefab == null}");

      //if (firePrefab == null) return;
      //if (cauldronPrefab == null) return;

      //Log.Trace($"[{GetType().Name}] Remove Prefab stuff we do not want.");
      //Destroy(firePrefab.gameObject.transform.Find("PlayerBase").gameObject);
      //firePrefab.transform.Find("_enabled_high").gameObject.SetActive(true);
      //cauldronPrefab.transform.Find("HaveFire").gameObject.SetActive(true);
      //// Cauldron.name = "$op_pot_name";
      //Log.Trace($"[{GetType().Name}] Prefabs cleaned up");

      //PrefabManager.Instance.AddPrefab(firePrefab);
      //PrefabManager.Instance.AddPrefab(cauldronPrefab);
    }

    public override void OnEnable()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public override void SecondaryInteract(Humanoid user)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public void Spawn(Transform parent)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      FirePit = Common.Utils.Spawn(CustomPrefabNames.FirePit, parent.position, parent);
      Cauldron = Common.Utils.Spawn(CustomPrefabNames.Cauldron, parent.position, parent);
    }
  }
}
