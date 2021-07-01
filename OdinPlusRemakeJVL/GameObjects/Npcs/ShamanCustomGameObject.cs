using JetBrains.Annotations;
using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.GameObjects.Npcs
{
  internal class ShamanCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject Shaman => GameObjectInstance;

    public ShamanCustomGameObject()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_shaman";
      CustomPrefabName = CustomPrefabNames.OrnamentalGoblinShaman;
      AddMonoBehaviour<ShamanCustomMonoBehaviour>();
    }
  }
}
