using JetBrains.Annotations;
using OdinPlusJVL.Behaviours;

namespace OdinPlusJVL.Common.Interfaces
{
  public interface ITalkable
  {
    [NotNull] [UsedImplicitly] TalkableMonoBehaviour TalkingBehaviour { get; set; }

    void Say(string msg, [CanBeNull] string topic = null, bool showName = true, bool longTimeout = false, bool large = false);
  }
}
