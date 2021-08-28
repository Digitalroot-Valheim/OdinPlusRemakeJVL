using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.GameObjects.Npcs
{
  internal class ShamanCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject Shaman => GameObjectInstance;

    public ShamanCustomGameObject()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_shaman";
      CustomPrefabName = CustomPrefabNames.OrnamentalGoblinShaman;
      AddMonoBehaviour<ShamanCustomMonoBehaviour>();
    }
  }
}
