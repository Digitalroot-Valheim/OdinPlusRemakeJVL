using UnityEngine;

namespace OdinPlusRemakeJVL.Common.Interfaces
{
  public interface ITalkable
  {
    Transform Head { get; set; }
    GameObject Talker { get; set; }

    void Say(string msg);
  }
}
