using UnityEngine;

namespace OdinPlusRemakeJVL.Extensions
{
  internal static class GameObjectExtensions
  {
    public static GameObject FindGameObject(this GameObject parent, string name)
    {
      var components = parent.GetComponentsInChildren(typeof(Transform), true);
      foreach (var component  in components)
      {
        if (component.name == name)
        {
          return component.gameObject;
        }
      }
      return null;
    }
	}
}
