using Digitalroot.Valheim.Common;
using OdinPlusJVL.Behaviours;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Prefabs
{
  /// <summary>
  /// A new prefab of the GoblinShaman with it's default hostile behaviors removed.
  /// </summary>
  internal class OrnamentalGoblinShaman : AbstractCustomPrefab
  {
    /// <inheritdoc />
    internal OrnamentalGoblinShaman()
      : base(PrefabNames.OrnamentalGoblinShaman, Digitalroot.Valheim.Common.Names.PrefabNames.GoblinShaman)
    {
    }

    /// <inheritdoc />
    private protected override GameObject OnCreate(GameObject prefab)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({prefab?.name})");
      if (prefab != null)
      {
        ZDO zdo = prefab.GetComponent<ZNetView>().GetZDO();
        Object.DestroyImmediate(prefab.GetComponent<RandomAnimation>());
        Object.DestroyImmediate(prefab.GetComponent<MonsterAI>());
        Object.DestroyImmediate(prefab.GetComponent<CharacterDrop>());
        Object.DestroyImmediate(prefab.GetComponent<Humanoid>());
        Object.DestroyImmediate(prefab.GetComponent<Rigidbody>());
        Object.DestroyImmediate(prefab.GetComponent<FootStep>());
        // Object.DestroyImmediate(prefab.GetComponent<VisEquipment>());
        Object.DestroyImmediate(prefab.GetComponent<ZSyncAnimation>());
        Object.DestroyImmediate(prefab.GetComponent<ZNetView>());
        Object.DestroyImmediate(prefab.GetComponent<ZSyncTransform>());

        foreach (var comp in prefab.GetComponents<Component>())
        {
          if (!(comp is Transform) && !(comp is ShamanCustomMonoBehaviour) && !(comp is CapsuleCollider))
          {
            Log.Trace(Main.Instance, $"[{GetType().Name}] DestroyImmediate({comp.name} - {comp.GetType().Name})");
            Object.DestroyImmediate(comp);
          }
        }

        // ZNetScene.instance.m_instances.Remove(zdo);
        // ZDOMan.instance.DestroyZDO(zdo);
      }

      return prefab;
    }
  }
}
