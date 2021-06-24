namespace OdinPlusRemakeJVL.Common
{
  public interface IMonoBehaviour
  {
    void FixedUpdate();
    void LateUpdate();
    void OnApplicationQuit();
    void OnBecameInvisible();
    void OnBecameVisible();
    void OnCollisionEnter();
    void OnCollisionExit();
    void OnCollisionStay();
    void OnDestroy();
    void OnDisable();
    void OnDrawGizmos();
    void OnEnable();
    void OnGUI();
    void OnLevelWasLoaded();
    void OnMouseDown();
    void OnMouseUp();
    void OnMouseDrag();
    void OnMouseEnter();
    void OnMouseExit();
    void OnMouseOver();
    void OnPostRender();
    void OnPreCull();
    void OnPreRender();
    void OnRenderImage();
    void OnRenderObject();
    void OnStart();
    void OnTriggerEnter();
    void OnTriggerExit();
    void OnTriggerStay();
    void OnWillRenderObject();
    void Reset();
    void Update();
  }
}
