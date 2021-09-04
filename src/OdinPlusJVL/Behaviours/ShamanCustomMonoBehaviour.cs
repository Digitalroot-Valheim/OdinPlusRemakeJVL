using Digitalroot.Valheim.Common;
using JetBrains.Annotations;
using OdinPlusJVL.Common.Interfaces;
using OdinPlusJVL.Common.Names;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OdinPlusJVL.Behaviours
{
  public class ShamanCustomMonoBehaviour : AbstractCustomMonoBehaviour, ITalkable, Hoverable, Interactable
  {
    private Animator _animator;

    [UsedImplicitly]
    public void Awake()
    {
      _animator = gameObject.GetComponentInChildren<Animator>();
      TalkingBehaviour = new TalkableMonoBehaviour(gameObject, "$op_shaman_name", _animator, 2.5f, 20f, 10f, 10f);
    }

    #region Implementation of ITalkable

    /// <inheritdoc />
    public TalkableMonoBehaviour TalkingBehaviour { get; set; }

    /// <inheritdoc />
    public void Say(string msg
      , string topic = null
      , string animationTriggerName = null
      , bool showName = true
      , bool longTimeout = false
      , bool large = false)
      => TalkingBehaviour.Say(msg, topic, animationTriggerName, showName, longTimeout, large);

    #endregion

    #region Implementation of Hoverable

    /// <inheritdoc />
    public string GetHoverText()
    {
      StringBuilder n = new StringBuilder($"<color=lightblue><b>$op_shaman_name</b></color>")
          // .Append($"\n<color=green><b>Credits:{OdinData.Credits}</b></color>")
          // .Append($"\n[<color=yellow><b>$KEY_Use</b></color>] $op_buy")
          .Append("\n[<color=yellow><b>1-8</b></color>]$op_shaman_offer")
        // .Append($"\n<color=yellow><b>[{Plugin.KeyboardShortcutSecondInteractKey.Value.MainKey}]</b></color>$op_shaman_use")
        ;
      return Localization.instance.Localize(n.ToString());
    }

    /// <inheritdoc />
    public string GetHoverName() => gameObject.name;

    #endregion

    #region Implementation of Interactable

    /// <inheritdoc />
    public bool Interact(Humanoid user, bool hold)
    {
      if (hold)
      {
        return false;
      }

      return true;
    }

    /// <inheritdoc />
    public bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      //var name = item.m_dropPrefab.name;
      //if (GoodsList.ContainsKey(name))
      //{
      //  var gd = GoodsList[name];
      //  if (item.m_stack >= gd.Value)
      //  {
      //    var goodItemData = OdinItemManager.Instance.GetItemData(gd.Good);
      //    if (user.GetInventory().AddItem(goodItemData))
      //    {
      //      user.GetInventory().RemoveItem(item, gd.Value);
      //      Say(goodItemData.m_shared.m_description);
      //      return true;
      //    }
      //    DBG.InfoCT("$op_inventory_full");
      //    return true;
      //  }
      //  Say("$op_shaman_notenough");
      //  return true;
      //}
      // _animator.SetBool(AnimationBooleans.Talk1, true);
      Say("$op_shaman_no");
      DoRandomTalkAnimation();
      // _animator.SetBool(AnimationBooleans.Talk1, false);
      return true;
    }

    #endregion

    private void DoRandomTalkAnimation()
    {
      var animationBooleans = AnimationBooleans.AllNames.ToArray();
      var count = animationBooleans.Length;
      var rnd = Random.Range(0, --count);
      Log.Trace(Main.Instance, $"count: {animationBooleans.Length}, rnd: {rnd}");
      var a = animationBooleans[rnd];

      _animator.SetBool(a, true);
      System.Timers.Timer timer = new(250);
      timer.Elapsed += (sender, args) =>
      {
        timer.Stop();
        _animator.SetBool(a, false);
        timer.Dispose();
      };
      timer.Start();
    }

    private static class AnimationBooleans
    {
      public static string Talk1 = nameof(Talk1).ToLower();
      public static string Talk2 = nameof(Talk2).ToLower();
      public static string Talk3 = nameof(Talk3).ToLower();
      public static string NodYes = "talk5";
      internal static readonly IEnumerable<string> AllNames = Digitalroot.Valheim.Common.Utils.AllNames(typeof(AnimationBooleans));

      // internal static string Talk4 = nameof(Talk4).ToLower(); // Does nothing
      // internal static string Idle = "idlebool"; // Do not use
      // internal static string Yell = nameof(Yell).ToLower(); // Same as AnimationTriggers.Emote6
    }

    // ReSharper disable once IdentifierTypo
    private static class AnimationTriggers
    {
      internal static string Dance = "Emote1";
      internal static string Cheer = "Emote2";
      internal static string Flex = "Emote3";
      internal static string Point = "Emote4";
      internal static string Yell = "Emote6";
      internal static readonly IEnumerable<string> AllNames = Digitalroot.Valheim.Common.Utils.AllNames(typeof(AnimationTriggers));
      // internal static string Emote5 = nameof(Emote5); // Same as Dance
    }
  }
}
