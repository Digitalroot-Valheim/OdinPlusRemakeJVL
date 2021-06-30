using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Common
{
  public abstract class AbstractOdinPlusMonoBehaviour : MonoBehaviour
  {
    private string _name;

    public string Name
    {
      get => _name;
      private protected set => _name = Utils.Localize(value);
    }

    public virtual void OnApplicationQuit()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }
  }
}
