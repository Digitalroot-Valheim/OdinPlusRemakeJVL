using JetBrains.Annotations;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Pieces
{
  internal class OdinsFirePit : AbstractOdinPlusMonoBehaviour
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
