using OdinPlusRemakeJVL.Common;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public abstract class AbstractNpc<T> : MonoBehaviourSingleton<T>, Hoverable, Interactable, ISecondaryInteractable where T : AbstractNpc<T>, new()
  {
    public string Name { get; private protected set; }
    private protected Transform Head { get; set; }

    private protected GameObject Talker { get; set; }

    public static bool IsInstantiated() => Instance == null;

    public virtual string GetHoverText()
    {
      return Jotunn.Managers.LocalizationManager.Instance.TryTranslate($"<color=lightblue><b>{Name}</b></color>");
    }

    public virtual string GetHoverName()
    {
      return Jotunn.Managers.LocalizationManager.Instance.TryTranslate(Name);
    }

    public virtual bool Interact(Humanoid user, bool hold)
    {
      if (hold)
      {
        return false;
      }

      return true;
    }

    public virtual bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      return false;
    }

    public abstract void SecondaryInteract(Humanoid user);

    private protected void Say(string msg)
    {
      msg = Localization.instance.Localize(msg);
      var topic = Localization.instance.Localize(Name);
      Chat.instance.SetNpcText(Talker, Vector3.up * 1.5f, 60f, 5, topic, msg, false);
    }
  }
}
