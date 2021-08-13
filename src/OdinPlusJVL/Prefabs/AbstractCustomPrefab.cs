using Digitalroot.Valheim.Common;
using OdinPlusJVL.Managers;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Prefabs
{
  /// <summary>
  /// Abstract Class for creating custom prefabs.
  /// </summary>
  internal abstract class AbstractCustomPrefab : Common.Interfaces.ICreateable
  {
    /// <summary>
    /// Name of the new prefab
    /// </summary>
    private readonly string _newPrefabName;

    /// <summary>
    /// Name of the source prefab
    /// </summary>
    private readonly string _sourcePrefabName;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="name">Name of the new prefab</param>
    /// <param name="basename">Name of the source prefab</param>
    private protected AbstractCustomPrefab(string name, string basename)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({name}, {basename})");
      _newPrefabName = name;
      _sourcePrefabName = basename;
    }

    /// <summary>
    /// Creates a new prefab from an existing prefab.
    /// </summary>
    /// <returns>The new prefab.</returns>
    public GameObject Create()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      if (string.IsNullOrEmpty(_newPrefabName))
      {
        throw new ArgumentException($"[{GetType().Name}] {nameof(_newPrefabName)} is null or empty");
      }

      if (string.IsNullOrEmpty(_sourcePrefabName))
      {
        throw new ArgumentException($"[{GetType().Name}] {nameof(_sourcePrefabName)} is null or empty");
      }

      var prefab = PrefabManager.Instance.GetPrefab(_newPrefabName);
      if (prefab == null)
      {
        Log.Trace(Main.Instance, $"[{GetType().Name}] Creating {_newPrefabName}");
        prefab = PrefabManager.Instance.CreateClonedPrefab(_newPrefabName, _sourcePrefabName);
      }

      if (prefab == null)
      {
        Log.Error(Main.Instance, $"[{GetType().Name}] Error with prefabs.");
        Log.Error(Main.Instance, $"[{GetType().Name}] prefab == null: true");
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
