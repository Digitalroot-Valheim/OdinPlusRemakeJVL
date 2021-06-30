using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Pieces
{
  internal class OdinsCauldron : AbstractOdinPlusPiece
  {
    public GameObject Cauldron => PieceInstance;

    public void Start()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public void Awake()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Name = "$op_pot_name";
        PieceInstance = Instantiate(PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalCauldron));
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
