using JetBrains.Annotations;

namespace OdinPlusRemakeJVL.Behaviours
{
  /// <inheritdoc />
  public class UnRemoveableCustomMonoBehaviour : AbstractCustomMonoBehaviour
  {
    [UsedImplicitly]
    public void Start() => gameObject.GetComponent<Piece>().m_canBeRemoved = false;
  }
}
