using OdinPlusRemakeJVL.Common;
using System.Reflection;
using OdinPlusRemakeJVL.Common.Interfaces;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public abstract class AbstractNpc<T> : MonoBehaviourSingleton<T>, Hoverable, Interactable, ISecondaryInteractable where T : AbstractNpc<T>, new()
  {
    private string _name;

    public string Name
    {
      get => _name;
      private protected set => _name = Localization.instance.Localize(value);
    }

    internal Transform Head { get; set; }

    internal GameObject Talker { get; set; }

    public virtual string GetHoverText()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      return Localization.instance.Localize($"<color=lightblue><b>{Name}</b></color>");
    }

    public virtual string GetHoverName()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      return Localization.instance.Localize(Name);
    }

    public virtual bool Interact(Humanoid user, bool hold)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      return !hold;
    }

    public virtual bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      return false;
    }

    public abstract void SecondaryInteract(Humanoid user);

    public abstract void Start();

    public abstract void OnEnable();

    private protected void Say(string msg)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name}, {msg})");
      msg = Localization.instance.Localize(msg);
      Log.Trace($"[{GetType().Name}.{MethodBase.GetCurrentMethod().Name}] Localization.instance == null : {Localization.instance == null}");
      Log.Trace($"[{GetType().Name}.{MethodBase.GetCurrentMethod().Name}] msg == null : {msg == null}");
      Log.Trace($"[{GetType().Name}.{MethodBase.GetCurrentMethod().Name}] Name == null : {Name == null}");
      var topic = Localization.instance.Localize(Name);
      Log.Trace($"[{GetType().Name}.{MethodBase.GetCurrentMethod().Name}] topic == null : {topic == null}");
      Log.Trace($"[{GetType().Name}.{MethodBase.GetCurrentMethod().Name}] Chat.instance == null : {Chat.instance == null}");
      Log.Trace($"[{GetType().Name}.{MethodBase.GetCurrentMethod().Name}] Talker == null : {Talker == null}");

      Chat.instance.SetNpcText(Talker, Vector3.up * 1.5f, 60f, 5, topic, msg, false);
    }
  }
}
