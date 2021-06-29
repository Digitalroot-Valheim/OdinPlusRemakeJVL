using OdinPlusRemakeJVL.Common;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public abstract class AbstractNpc<T> : MonoBehaviour, Hoverable, Interactable where T : AbstractNpc<T>, new()
  {
    private string _name;

    public string Name
    {
      get => _name;
      private protected set => _name = Common.Utils.Localize(value);
    }

    internal Transform Head { get; set; }

    internal GameObject Talker { get; set; }

    public virtual string GetHoverText()
    {
      // Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      return Common.Utils.Localize($"<color=lightblue><b>{Name}</b></color>");
    }

    public virtual string GetHoverName()
    {
      // Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      return Common.Utils.Localize(Name);
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

    public virtual void OnApplicationQuit()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public virtual void OnDestroy()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }

    public abstract void SecondaryInteract(Humanoid user);

    public abstract void Start();

    public abstract void OnEnable();

    private protected void Say(string msg)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name}, {msg})");
      msg = Common.Utils.Localize(msg);
      var topic = Common.Utils.Localize(Name);
      Chat.instance?.SetNpcText(Talker, Vector3.up * 3f, 60f, 8, topic, msg, false);
    }
  }
}
