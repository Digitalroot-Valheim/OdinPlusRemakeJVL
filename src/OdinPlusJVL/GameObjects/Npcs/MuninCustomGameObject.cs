using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.GameObjects.Npcs
{
  internal class MuninCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject Munin => GameObjectInstance;

    public MuninCustomGameObject()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_munin_name";
      CustomPrefabName = CustomPrefabNames.OrnamentalMunin;
      AddMonoBehaviour<MuninCustomMonoBehaviour>();
    }
  }
}
