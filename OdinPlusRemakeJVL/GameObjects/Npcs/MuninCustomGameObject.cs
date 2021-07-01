using JetBrains.Annotations;
using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.GameObjects.Npcs
{
  internal class MuninCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject Munin => GameObjectInstance;

    public MuninCustomGameObject()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_munin_name";
      CustomPrefabName = CustomPrefabNames.OrnamentalMunin;
      AddMonoBehaviour<MuninCustomMonoBehaviour>();
    }
  }
}
