using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Pieces
{
  internal class OdinsFirePit : AbstractOdinPlusPiece
  {
    public GameObject FirePit => PieceInstance;

    public void Start()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public void Awake()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Name = "$op_fire_pit_name";
        PieceInstance = Instantiate(PrefabManager.Instance.GetPrefab(CustomPrefabNames.OrnamentalFirePit));
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
