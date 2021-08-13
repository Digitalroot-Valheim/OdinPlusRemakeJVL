using Digitalroot.Valheim.Common;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Behaviours
{
  public abstract class AbstractCustomMonoBehaviour : MonoBehaviour
  {
    protected void Say(GameObject talker, string topic, string msg)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name}, {topic}: {Digitalroot.Valheim.Common.Utils.Localize(msg)})");
      Chat.instance?.SetNpcText(talker, Vector3.up * 3f, 60f, 8, Digitalroot.Valheim.Common.Utils.Localize(topic), Digitalroot.Valheim.Common.Utils.Localize(msg), false);
    }
  }
}
