using OdinPlus.Common;
using OdinPlus.Data;
using UnityEngine;

namespace OdinPlus.Npcs
{
  public abstract class OdinNpc : MonoBehaviour, Hoverable, Interactable, IOdinInteractable
  {
    public string m_name;
    public Transform m_head;
    public GameObject m_talker;

    public virtual void Say(string text)
    {
      text = Localization.instance.Localize(text);
      var tname = Localization.instance.Localize(m_name);
      Chat.instance.SetNpcText(m_talker, Vector3.up * 1.5f, 60f, 5, tname, text, false);
    }

    public virtual bool Interact(Humanoid user, bool hold)
    {
      if (hold)
      {
        return false;
      }

      return true;
    }

    public virtual void SecondaryInteract(Humanoid user)
    {
    }

    public virtual string GetHoverText()
    {
      string n = $"<color=lightblue><b>{m_name}</b></color>";
      n += $"\n<color=green><b>Credits:{OdinData.Credits}</b></color>";
      return Localization.instance.Localize(n);
    }

    public virtual string GetHoverName()
    {
      return Localization.instance.Localize(m_name);
    }

    public virtual bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      return false;
    }
  }
}
