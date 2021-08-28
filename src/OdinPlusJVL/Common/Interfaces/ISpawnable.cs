using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;
using UnityEngine;

namespace OdinPlusJVL.Common.Interfaces
{
  /// <summary>
  /// Contract for creating Spawnable GameObjects
  /// </summary>
  internal interface ISpawnable
  {
    /// <summary>
    /// Local position relative to the parent.
    /// </summary>
    [UsedImplicitly]
    Vector3 LocalPositionOffset { [UsedImplicitly] get; }

    /// <summary>
    /// Local rotation relative to the parent.
    /// </summary>
    [UsedImplicitly]
    Quaternion LocalRotationOffset { [UsedImplicitly] get; }

    /// <summary>
    /// Local rotation to apply once spawned
    /// </summary>
    [UsedImplicitly]
    Vector3 LocalRotation { [UsedImplicitly] get; }

    /// <summary>
    /// Add a Custom MonoBehaviour to a Spawnable
    /// </summary>
    /// <typeparam name="T">Type of AbstractCustomMonoBehaviour</typeparam>
    [UsedImplicitly]
    void AddMonoBehaviour<T>() where T : AbstractCustomMonoBehaviour;

    /// <summary>
    /// Called when Destroying a Spawnable
    /// </summary>
    void OnDestroy();

    /// <summary>
    /// Sets the position to spawn relative to the parent
    /// Set to Vector3.zero for no offset.
    /// </summary>
    /// <param name="vector3">Position to spawn relative to the parent.</param>
    [UsedImplicitly]
    void SetLocalPositionOffset(Vector3 vector3);

    /// <summary>
    /// Sets the rotation to spawn relative to the parent
    /// Set to Quaternion.identity for no offset.
    /// </summary>
    /// <param name="quaternion">Position to rotation relative to the parent.</param>
    [UsedImplicitly]
    void SetLocalRotationOffset(Quaternion quaternion);

    /// <summary>
    /// Local rotation to apply once spawned
    /// Set to Vector3.zero for no rotation.
    /// </summary>
    /// <param name="vector3">How much to rotate</param>
    [UsedImplicitly]
    void SetLocalRotation(Vector3 vector3);

    /// <summary>
    /// Called when a parent wants to spawn an instance of a Spawnable.
    /// Spawnable is made a child of the parent. If parent is unknown
    /// pass the base Root GameObject.
    /// </summary>
    /// <param name="parent">Transform for the parent requesting the spawn</param>
    void Spawn([NotNull] Transform parent);
  }
}
