using JetBrains.Annotations;
using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.GameObjects
{
  internal class OdinsFirePitCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject FirePit => GameObjectInstance;

    public OdinsFirePitCustomGameObject()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_fire_pit_name";
      CustomPrefabName = CustomPrefabNames.OrnamentalFirePit;
      AddMonoBehaviour<UnRemoveableCustomMonoBehaviour>();
    }
  }
}
