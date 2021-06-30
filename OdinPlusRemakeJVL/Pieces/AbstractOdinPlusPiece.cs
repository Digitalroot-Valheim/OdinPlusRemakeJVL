using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Pieces
{
  public abstract class AbstractOdinPlusPiece : AbstractOdinPlusMonoBehaviour, ISpawnable
  {
    public GameObject PieceInstance { get; private protected set; }

    public Vector3 LocalPositionOffset { get; private protected set; }

    public void Spawn(Transform parent)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({parent.name})");
      PieceInstance.transform.SetParent(transform);
      if (PieceInstance.transform.position != parent.position)
      {
        // Log.Trace($"[{GetType().Name}] PieceInstance.transform.position != parent.position");
        // Log.Trace($"[{GetType().Name}] parent.position : {parent.position}");
        // Log.Trace($"[{GetType().Name}] PieceInstance.transform.position : {PieceInstance.transform.position}");
        PieceInstance.transform.localPosition = LocalPositionOffset;
      }
    }

    public void SetLocalPositionOffset(Vector3 vector3)
    {
      LocalPositionOffset = vector3;
    }

    public virtual void OnDestroy()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (PieceInstance != null) Destroy(PieceInstance);
    }

    public override void OnApplicationQuit()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      OnDestroy();
    }
  }
}
