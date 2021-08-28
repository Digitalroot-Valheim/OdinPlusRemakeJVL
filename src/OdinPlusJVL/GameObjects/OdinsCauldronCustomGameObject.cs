using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using OdinPlusJVL.Common.Names;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.GameObjects
{
  internal class OdinsCauldronCustomGameObject : AbstractCustomGameObject
  {
    [UsedImplicitly] public GameObject Cauldron => GameObjectInstance;

    public OdinsCauldronCustomGameObject()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      Name = "$op_pot_name";
      CustomPrefabName = CustomPrefabNames.OrnamentalCauldron;
      AddMonoBehaviour<UnRemoveableCustomMonoBehaviour>();
      AddMonoBehaviour<OdinsCauldronCustomMonoBehaviour>();
    }
  }
}
