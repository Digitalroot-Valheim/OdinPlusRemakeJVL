using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Prefabs
{
  /// <summary>
  /// Abstract Class for creating custom prefabs.
  /// </summary>
  internal abstract class AbstractCustomPrefab : ICreateable
  {
    /// <summary>
    /// Name of the new prefab
    /// </summary>
    protected readonly string NewPrefabName;

    /// <summary>
    /// Name of the source prefab
    /// </summary>
    protected readonly string SourcePrefabName;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="name">Name of the new prefab</param>
    /// <param name="basename">Name of the source prefab</param>
    private protected AbstractCustomPrefab(string name, string basename)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      NewPrefabName = name;
      SourcePrefabName = basename;
    }

    /// <summary>
    /// Creates a new prefab from an existing prefab.
    /// </summary>
    /// <returns>The new prefab.</returns>
    public GameObject Create()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      if (string.IsNullOrEmpty(NewPrefabName))
      {
        throw new ArgumentException($"[{GetType().Name}] NewPrefabName is null");
      }

      if (string.IsNullOrEmpty(SourcePrefabName))
      {
        throw new ArgumentException($"[{GetType().Name}] SourcePrefabName is null");
      }

      var prefab = PrefabManager.Instance.GetPrefab(NewPrefabName);
      if (prefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {NewPrefabName}");
        prefab = PrefabManager.Instance.CreateClonedPrefab(NewPrefabName, SourcePrefabName);
      }

      if (prefab == null)
      {
        Log.Error($"[{GetType().Name}] Error with prefabs.");
        Log.Error($"[{GetType().Name}] prefab == null: true");
      }

      return OnCreate(prefab);
    }

    /// <summary>
    /// Decorator method used to clean up the new prefab.
    /// </summary>
    /// <param name="prefab"></param>
    /// <returns></returns>
    private protected abstract GameObject OnCreate(GameObject prefab);
  }
}
