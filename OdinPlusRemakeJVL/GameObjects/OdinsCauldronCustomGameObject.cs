using JetBrains.Annotations;
using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.GameObjects
{
  internal class OdinsCauldronCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject Cauldron => GameObjectInstance;

    public OdinsCauldronCustomGameObject()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_pot_name";
      CustomPrefabName = CustomPrefabNames.OrnamentalCauldron;
      AddMonoBehaviour<UnRemoveableCustomMonoBehaviour>();
      AddMonoBehaviour<OdinsCauldronCustomMonoBehaviour>();
    }
  }
}
