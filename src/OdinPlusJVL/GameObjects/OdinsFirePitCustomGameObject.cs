using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.GameObjects
{
  internal class OdinsFirePitCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject FirePit => GameObjectInstance;

    public OdinsFirePitCustomGameObject()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_fire_pit_name";
      CustomPrefabName = CustomPrefabNames.OrnamentalFirePit;
      AddMonoBehaviour<UnRemoveableCustomMonoBehaviour>();
    }
  }
}
