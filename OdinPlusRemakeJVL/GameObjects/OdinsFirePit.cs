using System;
using System.Reflection;
using JetBrains.Annotations;
using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using OdinPlusRemakeJVL.Managers;
using UnityEngine;

namespace OdinPlusRemakeJVL.GameObjects
{
  internal class OdinsFirePit : AbstractCustomMonoBehaviour
  {
    [UsedImplicitly] public GameObject FirePit => GameObjectInstance;

    [UsedImplicitly]
    public void Awake()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Name = "$op_fire_pit_name";
        GameObjectInstance = Instantiate(PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalFirePit));
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
