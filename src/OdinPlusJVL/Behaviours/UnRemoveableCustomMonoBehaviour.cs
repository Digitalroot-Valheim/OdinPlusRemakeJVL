using JetBrains.Annotations;

namespace OdinPlusJVL.Behaviours
{
  /// <inheritdoc />
  public class UnRemoveableCustomMonoBehaviour : AbstractCustomMonoBehaviour
  {
    [UsedImplicitly]
    public void Start() => gameObject.GetComponent<Piece>().m_canBeRemoved = false;
  }
}
