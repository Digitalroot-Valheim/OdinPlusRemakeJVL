namespace OdinPlusRemakeJVL.Common.Interfaces
{
  public interface IMonoBehaviour
  {
    void FixedUpdate();
    void LateUpdate();
    void OnApplicationFocus();
    void OnApplicationPause();
    void OnApplicationQuit();
    void OnBecameInvisible();
    void OnBecameVisible();
    void OnCollisionEnter();
    void OnCollisionExit();
    void OnCollisionStay();
    void OnConnectedToServer();
    void OnControllerColliderHit();
    void OnDestroy();
    void OnDisable();
    void OnDisconnectedFromServer();
    void OnDrawGizmos();
    void OnDrawGizmosSelected();
    void OnEnable();
    void OnFailedToConnect();
    void OnFailedToConnectToMasterServer();
    void OnGUI();
    void OnMasterServerEvent();
    void OnMouseDown();
    void OnMouseUp();
    void OnMouseUpAsButton();
    void OnMouseDrag();
    void OnMouseEnter();
    void OnMouseExit();
    void OnMouseOver();
    void OnNetworkInstantiate();
    void OnParticleCollision();
    void OnParticleSystemStopped();
    void OnParticleTrigger();
    void OnParticleUpdateJobScheduled();
    void OnPlayerConnected();
    void OnPlayerDisconnected();
    void OnPostRender();
    void OnPreCull();
    void OnPreRender();
    void OnRenderObject();
    void OnSerializeNetworkView();
    void OnServerInitialized();
    void OnTransformChildrenChanged();
    void OnTransformParentChanged();
    void OnTriggerEnter();
    void OnTriggerExit();
    void OnTriggerStay();
    void OnValidate();
    void OnWillRenderObject();
    void Reset();
    void Update();
    void Start();
  }
}
