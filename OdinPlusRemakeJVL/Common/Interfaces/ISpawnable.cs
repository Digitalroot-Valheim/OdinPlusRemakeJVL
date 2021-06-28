using UnityEngine;

namespace OdinPlusRemakeJVL.Common.Interfaces
{
  internal interface ISpawnable
  {
    void Spawn(GameObject prefab, Vector3 location, Transform parent);
  }
}
