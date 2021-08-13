using UnityEngine;

namespace OdinPlusJVL.Common.EventArgs
{
  class OnSpawnedPlayerEventArgs : System.EventArgs
  {
    public Vector3 Position { get; }

    public OnSpawnedPlayerEventArgs(Vector3 position)
    {
      Position = position;
    }
  }
}
