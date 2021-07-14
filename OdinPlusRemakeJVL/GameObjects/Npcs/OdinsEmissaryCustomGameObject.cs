using JetBrains.Annotations;
using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.GameObjects.Npcs
{
  internal class OdinsEmissaryCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject OdinsEmissary => GameObjectInstance;
    
    public OdinsEmissaryCustomGameObject()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_odin_emissary";
      CustomPrefabName = CustomPrefabNames.OrnamentalKeeper;
      AddMonoBehaviour<OdinsEmissaryCustomMonoBehaviour>();
    }
  }
}
