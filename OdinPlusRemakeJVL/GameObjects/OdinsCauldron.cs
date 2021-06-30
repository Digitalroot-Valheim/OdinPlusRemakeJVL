using System;
using System.Reflection;
using JetBrains.Annotations;
using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Common.Names;
using OdinPlusRemakeJVL.Managers;
using UnityEngine;

namespace OdinPlusRemakeJVL.GameObjects
{
  internal class OdinsCauldron : AbstractCustomMonoBehaviour, ITalkable, Hoverable
  {
    [UsedImplicitly] public GameObject Cauldron => GameObjectInstance;

    public Transform Head { get; set; }

    public GameObject Talker { get; set; }

    [UsedImplicitly]
    public void Awake()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Name = "$op_pot_name";
        GameObjectInstance = Instantiate(PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalCauldron));
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    public void Say(string msg)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name}, {Common.Utils.Utils.Localize(msg)})");
      Chat.instance?.SetNpcText(Talker, Vector3.up * 3f, 60f, 8, Common.Utils.Utils.Localize(Name), Common.Utils.Utils.Localize(msg), false);
    }

    public string GetHoverText()
    {
      return $"<color=lightblue><b>{Name}</b></color>";
    }

    public string GetHoverName() => Name;
  }
}
