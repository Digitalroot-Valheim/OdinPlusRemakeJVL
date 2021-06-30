using OdinPlusRemakeJVL.Common.Interfaces;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Common
{
  public abstract class AbstractOdinPlusMonoBehaviour : MonoBehaviour, ISpawnable
  {
    private string _name;

    public string Name
    {
      get => _name;
      private protected set => _name = Utils.Localize(value);
    }

    public GameObject GameObjectInstance { get; private protected set; }
    public Vector3 LocalPositionOffset { get; private protected set; }

    public void Spawn(Transform parent)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({parent.name})");
      GameObjectInstance.transform.SetParent(transform);
      if (LocalPositionOffset != Vector3.zero) 
      {
        GameObjectInstance.transform.localPosition = LocalPositionOffset;
      }
    }

    public void OnApplicationQuit()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      OnDestroy();
    }

    public virtual void OnDestroy()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (GameObjectInstance != null) Destroy(GameObjectInstance);
    }

    public void SetLocalPositionOffset(Vector3 vector3)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({vector3})");
      LocalPositionOffset = vector3;
    }
  }
}
