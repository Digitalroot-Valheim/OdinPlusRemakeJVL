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

      var firePrefab  = PrefabManager.Instance.GetPrefab(CustomPrefabNames.FirePit);
      var cauldronPrefab = PrefabManager.Instance.GetPrefab(CustomPrefabNames.FirePit);

      if (firePrefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.FirePit}");
        firePrefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.FirePit, ItemNames.FirePit);
        if (firePrefab != null)
        {
          Destroy(firePrefab.gameObject.transform.Find("PlayerBase").gameObject);
          firePrefab.transform.Find("_enabled_high").gameObject.SetActive(true);
          DestroyImmediate(firePrefab.GetComponent<WearNTear>());
          DestroyImmediate(firePrefab.GetComponent<Fireplace>());
          firePrefab.GetComponent<Piece>().m_canBeRemoved = false;

          foreach (var item in firePrefab.GetComponentsInChildren<Aoe>())
          {
            DestroyImmediate(item);
          }

          PrefabManager.Instance.AddPrefab(firePrefab);
        }
      }

      if (cauldronPrefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.Cauldron}");
        cauldronPrefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.Cauldron, ItemNames.Cauldron);
        if (cauldronPrefab != null)
        {
          cauldronPrefab.transform.Find("HaveFire").gameObject.SetActive(true);
          DestroyImmediate(cauldronPrefab.GetComponent<CraftingStation>());
          DestroyImmediate(cauldronPrefab.GetComponent<WearNTear>());
          cauldronPrefab.GetComponent<Piece>().m_canBeRemoved = false;

          PrefabManager.Instance.AddPrefab(cauldronPrefab);
        }
      }

      if (firePrefab == null || cauldronPrefab == null)
      {
        Log.Error($"[{GetType().Name}] Error with prefabs.");
        Log.Error($"[{GetType().Name}] firePrefab == null: {firePrefab == null}");
        Log.Error($"[{GetType().Name}] cauldronPrefab == null: {cauldronPrefab == null}");
      }
    }

    public override void Start()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public override void OnEnable()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public override void OnDestroy()
    {
      base.OnDestroy();
      if (FirePit != null) Destroy(FirePit);
      if (Cauldron != null) Destroy(Cauldron);
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
      // Cauldron.name = "$op_pot_name";
    }
  }
}
