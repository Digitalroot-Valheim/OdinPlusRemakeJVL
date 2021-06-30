using OdinPlusRemakeJVL.Behaviours;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using System.Reflection;
using UnityEngine;

namespace OdinPlusRemakeJVL.Npcs
{
  public abstract class AbstractNpc : AbstractCustomMonoBehaviour, ITalkable, Hoverable, Interactable
  {
    public Transform Head { get; set; }
    public GameObject Talker { get; set; }
    public abstract string GetHoverName();
    public abstract string GetHoverText();
    public abstract bool Interact(Humanoid user, bool hold);
    public abstract bool UseItem(Humanoid user, ItemDrop.ItemData item);

    public void Say(string msg)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name}, {Common.Utils.Utils.Localize(msg)})");
      Chat.instance?.SetNpcText(Talker, Vector3.up * 3f, 60f, 8, Common.Utils.Utils.Localize(Name), Common.Utils.Utils.Localize(msg), false);
    }
  }
}
