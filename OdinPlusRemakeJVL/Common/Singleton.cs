using JetBrains.Annotations;
using System;
using UnityEngine;

namespace OdinPlusRemakeJVL.Common
{
  /// <summary>
  /// Source: https://github.com/Digitalroot/digitalroot-valheim-mods/blob/main/Digitalroot.Valheim.Common/Core/Singleton.cs
  /// Permission granted to OdinPlus under 
  /// License: "GNU Affero General Public License v3.0"
  /// License ref: https://github.com/Digitalroot/digitalroot-valheim-mods/blob/main/LICENSE
  /// </summary>
  /// <typeparam name="TSingletonSubClass"></typeparam>
  public abstract class Singleton<TSingletonSubClass> where TSingletonSubClass : Singleton<TSingletonSubClass>, new()
  {
    public static TSingletonSubClass Instance => Nested._instance;

    [UsedImplicitly]
    private class Nested
    {
      static Nested()
      {
      }

      // ReSharper disable once InconsistentNaming
      internal static readonly TSingletonSubClass _instance = InstantiateInstance();

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
          throw;
        }

        return instance;
      }
    }
  }

  public abstract class MonoBehaviourSingleton<TSingletonSubClass> : MonoBehaviour, IMonoBehaviour where TSingletonSubClass : MonoBehaviourSingleton<TSingletonSubClass>, new()
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
          throw;
        }

        return instance;
      }
    }

    #region Unity Methods

    public virtual void OnApplicationQuit()
    {
      IsShuttingDown = true;
    }

    public virtual void OnDestroy()
    {
      IsShuttingDown = true;
    }

    public virtual void OnEnable()
    {
    }

    public virtual void OnDisable()
    {
    }

    public virtual void OnStart()
    {
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void OnCollisionEnter()
    {
    }

    public virtual void OnCollisionStay()
    {
    }

    public virtual void OnCollisionExit()
    {
    }

    public virtual void OnTriggerEnter()
    {
    }

    public virtual void OnTriggerStay()
    {
    }

    public virtual void OnTriggerExit()
    {
    }

    public virtual void OnMouseDown()
    {
    }

    public virtual void OnMouseUp()
    {
    }

    public virtual void OnMouseDrag()
    {
    }

    public virtual void OnMouseEnter()
    {
    }

    public virtual void OnMouseExit()
    {
    }

    public virtual void OnMouseOver()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void OnGUI()
    {
    }

    public virtual void OnDrawGizmos()
    {
    }

    public virtual void OnLevelWasLoaded()
    {
    }

    public virtual void Reset()
    {
    }

    public virtual void OnPreCull()
    {
    }

    public virtual void OnBecameVisible()
    {
    }

    public virtual void OnBecameInvisible()
    {
    }

    public virtual void OnWillRenderObject()
    {
    }

    public virtual void OnPreRender()
    {
    }

    public virtual void OnRenderObject()
    {
    }

    public virtual void OnPostRender()
    {
    }

    public virtual void OnRenderImage()
    {
    }

    public virtual void LateUpdate()
    {
    }

    #endregion
  }
}
