using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.GameObjects.Npcs
{
  internal class OdinsEmissaryCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject OdinsEmissary => GameObjectInstance;
    
    public OdinsEmissaryCustomGameObject()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_odin_emissary";
      CustomPrefabName = CustomPrefabNames.OrnamentalKeeper;
      AddMonoBehaviour<OdinsEmissaryCustomMonoBehaviour>();
    }
  }
}
