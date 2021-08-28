using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.GameObjects.Npcs
{
  internal class OdinCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject Odin => GameObjectInstance;
    
    public OdinCustomGameObject()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_god";
      CustomPrefabName = CustomPrefabNames.OrnamentalOdin;
      // AddMonoBehaviour<OdinCustomMonoBehaviour>();
    }
  }
}
