using OdinPlusRemakeJVL.Common;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Behaviours
{
  public abstract class AbstractCustomMonoBehaviour : MonoBehaviour
  {
    protected void Say(GameObject talker, string topic, string msg)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name}, {topic}: {Common.Utils.Utils.Localize(msg)})");
      Chat.instance?.SetNpcText(talker, Vector3.up * 3f, 60f, 8, Common.Utils.Utils.Localize(topic), Common.Utils.Utils.Localize(msg), false);
    }
  }
}
