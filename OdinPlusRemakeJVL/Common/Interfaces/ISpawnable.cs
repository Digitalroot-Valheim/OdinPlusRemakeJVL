using UnityEngine;

namespace OdinPlusRemakeJVL.Common.Interfaces
{
  internal interface ISpawnable
  {
    Vector3 LocalPositionOffset { get; }
    void Spawn(Transform parent);
  }
}
