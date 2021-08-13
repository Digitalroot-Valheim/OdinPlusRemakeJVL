using UnityEngine;

public class MyCustomMonoBehaviour : MonoBehaviour
{
  public void Update()
  {
    gameObject.transform.Rotate(0, 5f, 0);
  }
}
