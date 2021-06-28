using JetBrains.Annotations;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Common
{

  public abstract class MonoBehaviourSingleton<TSingletonSubClass> : MonoBehaviour where TSingletonSubClass : MonoBehaviourSingleton<TSingletonSubClass>, new()
  {
    public static TSingletonSubClass Instance => Nested._instance;

    // ReSharper disable once StaticMemberInGenericType
    protected static bool IsShuttingDown;

    [UsedImplicitly]
    private class Nested
    {
      static Nested()
      {
      }

      // ReSharper disable once InconsistentNaming
      internal static readonly TSingletonSubClass _instance = IsShuttingDown ? null : InstantiateInstance();

      private static TSingletonSubClass InstantiateInstance()
      {
        TSingletonSubClass instance;
        try
        {
          instance = new TSingletonSubClass();
        }
        catch (Exception ex)
        {
          Log.Error($"Failed while initializing singleton of type: {typeof(TSingletonSubClass).FullName}: {ex.Message}");
          Log.Error(ex);
          throw;
        }

        return instance;
      }
    }

    #region Unity Methods

    public virtual void OnApplicationQuit()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      IsShuttingDown = true;
    }

    public virtual void OnDestroy()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      IsShuttingDown = true;
    }

    #endregion
  }
}
