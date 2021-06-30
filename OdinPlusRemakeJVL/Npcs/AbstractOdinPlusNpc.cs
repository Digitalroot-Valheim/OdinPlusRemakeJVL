using OdinPlusRemakeJVL.Common;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public abstract class AbstractOdinPlusNpc : MonoBehaviour
  {
    public GameObject NpcInstance { get; private protected set; }

    private string _name;

    public string Name
    {
      get => _name;
      private protected set => _name = Common.Utils.Localize(value);
    }

    public virtual void OnApplicationQuit()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public virtual void OnDestroy()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (NpcInstance != null) Destroy(NpcInstance);
    }
  }
}
