using JetBrains.Annotations;
using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.GameObjects.Npcs
{
  internal class OdinCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject Odin => GameObjectInstance;

    public OdinCustomGameObject()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_god";
      CustomPrefabName = CustomPrefabNames.OrnamentalOdin;
      AddMonoBehaviour<OdinCustomMonoBehaviour>();
    }
  }
}
