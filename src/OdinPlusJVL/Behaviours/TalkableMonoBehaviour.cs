using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Behaviours
{
  public class TalkableMonoBehaviour : MonoBehaviour
  {
    private readonly string _name;
    private readonly float _textOffset;
    private readonly float _textCullDistance;
    private readonly float _dialogVisibleTime;
    private readonly float _longDialogVisibleTime;
    private readonly GameObject _talker;

    public TalkableMonoBehaviour(GameObject talker, string name, float textOffset, float textCullDistance, float dialogVisibleTime, float longDialogVisibleTime)
    {
      _name = name;
      _textOffset = textOffset;
      _textCullDistance = textCullDistance;
      _dialogVisibleTime = dialogVisibleTime;
      _longDialogVisibleTime = longDialogVisibleTime;
      _talker = talker;
    }

    public void Say(string msg, [CanBeNull] string topic, bool showName = true, bool longTimeout = false, bool large = false)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({Digitalroot.Valheim.Common.Utils.Localize(msg)}, {topic ?? string.Empty}, {showName}, {longTimeout}, {large})");
      if (!string.IsNullOrEmpty(topic))
      {
        msg = $"<color=orange>{topic}</color>\n{msg}";
      }

      Chat.instance.SetNpcText(_talker
        , Vector3.up * _textOffset
        , _textCullDistance
        , longTimeout ? _longDialogVisibleTime : _dialogVisibleTime
        , showName ? Digitalroot.Valheim.Common.Utils.Localize(_name) : string.Empty
        , Digitalroot.Valheim.Common.Utils.Localize(msg), large);
    }
  }
}
