using Digitalroot.Valheim.Common;
using OdinPlusJVL.Behaviours;
using OdinPlusJVL.Common.Interfaces;
using OdinPlusJVL.Managers;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OdinPlusJVL.GameObjects
{
  /// <summary>
  /// Base class to encapsulate methods common to Custom GameObjects
  /// </summary>
  internal abstract class AbstractCustomGameObject : ISpawnable
  {
    /// <summary>
    /// Collection of Custom MonoBehaviours to add to a CustomGameObject
    /// </summary>
    private readonly List<Type> _customMonoBehaviours = new List<Type>();

    /// <summary>
    /// Backing field for Name
    /// </summary>
    private string _name;

    /// <summary>
    /// Localized name of the CustomGameObject
    /// Localization tokens are Localized
    /// </summary>
    public string Name
    {
      get => _name;
      private protected set => _name = Digitalroot.Valheim.Common.Utils.Localize(value);
    }

    /// <summary>
    /// Name of the Custom Prefab to Instantiate.
    /// </summary>
    private protected string CustomPrefabName { get; set; }

    /// <summary>
    /// Instance
    /// </summary>
    private protected GameObject GameObjectInstance { get; set; }

    /// <inheritdoc />
    public Vector3 LocalPositionOffset { get; private set; }

    /// <inheritdoc />
    public Quaternion LocalRotationOffset { get; private set; }

    /// <inheritdoc />
    public Vector3 LocalRotation { get; private set; }


    public void AddMonoBehaviour<T>() where T : AbstractCustomMonoBehaviour
    {
      _customMonoBehaviours.Add(typeof(T));
    }

    /// <inheritdoc />
    public virtual void OnDestroy()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (GameObjectInstance != null) Object.Destroy(GameObjectInstance);
    }

    /// <inheritdoc />
    public void SetLocalPositionOffset(Vector3 vector3)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({vector3})");
      LocalPositionOffset = vector3;
    }

    /// <inheritdoc />
    public void SetLocalRotationOffset(Quaternion quaternion)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({quaternion})");
      LocalRotationOffset = quaternion;
    }

    /// <inheritdoc />
    public void SetLocalRotation(Vector3 vector3)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({vector3})");
      LocalRotation = vector3;
    }

    /// <inheritdoc />
    public void Spawn(Transform parent)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({parent.name})");

        if (string.IsNullOrEmpty(CustomPrefabName))
        {
          throw new ArgumentNullException($"{GetType().Name} {nameof(CustomPrefabName)} is null or empty");
        }

        Log.Trace(Main.Instance, $"{GetType().Name} Getting prefab for {CustomPrefabName}");
        GameObjectInstance = Object.Instantiate(PrefabManager.Instance.GetPrefab(CustomPrefabName));
        GameObjectInstance.name = _name;
        GameObjectInstance.transform.SetParent(parent.transform);
        if (LocalPositionOffset != Vector3.zero)
        {
          GameObjectInstance.transform.localPosition = LocalPositionOffset;
        }

        if (LocalRotationOffset != Quaternion.identity)
        {
          GameObjectInstance.transform.rotation = LocalRotationOffset;
        }

        if (LocalRotation != Vector3.zero)
        {
          GameObjectInstance.transform.Rotate(LocalRotation.x, LocalRotation.y, LocalRotation.z);
        }

        foreach (var behaviour in _customMonoBehaviours)
        {
          GameObjectInstance.AddComponent(behaviour);
        }
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }
  }
}
