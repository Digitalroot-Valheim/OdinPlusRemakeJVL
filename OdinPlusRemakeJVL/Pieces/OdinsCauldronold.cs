using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Interfaces;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace OdinPlusRemakeJVL.Pieces
{
  internal class OdinsCauldronOld : AbstractOdinPlusMonoBehaviour, ITalkable, Hoverable, Interactable, ISpawnable
  {
    public Transform Head { get; set; }
    public GameObject Talker { get; set; }
    public GameObject Cauldron => GameObjectInstance;

    public void Awake()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        Name = "$op_pot_name";
        CreateCauldron();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }

    public bool Interact(Humanoid user, bool hold)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      return false;
    }

    public string GetHoverName()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      return "GetHoverName";
    }

    public string GetHoverText()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      StringBuilder stringBuilder = new StringBuilder($"<color=lightblue><b>{Name}</b></color>");
      // string s = string.Format("\n<color=lightblue><b>$op_crd:{0}</b></color>", OdinData.Credits);
      // string a = string.Format("\n[<color=yellow><b>$KEY_Use</b></color>] $op_use[<color=green><b>{0}</b></color>]", cskill);
      // string b = "\n[<color=yellow><b>1-8</b></color>]$op_offer";
      // b += String.Format("\n<color=yellow><b>[{0}]</b></color>$op_switch", Main.KeyboardShortcutSecondInteractKey.Value.MainKey.ToString());
      // return Localization.instance.Localize(n + s + a + b);
      return Common.Utils.Localize(stringBuilder.ToString());
      // return $"<color=lightblue><b>Odin</b></color> is here";
    }

    public void Spawn(Transform parent)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      GameObjectInstance = Common.Utils.Spawn(CustomPrefabNames.Cauldron, parent.position, parent);
    }

    public void Say(string msg)
    {
      throw new System.NotImplementedException();
    }

    public bool UseItem(Humanoid user, ItemDrop.ItemData item)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      return false;
    }

    private void CreateCauldron()
    {
      var cauldronPrefab = PrefabManager.Instance.GetPrefab(CustomPrefabNames.Cauldron);

      if (cauldronPrefab == null)
      {
        Log.Trace($"[{GetType().Name}] Creating {CustomPrefabNames.Cauldron}");
        cauldronPrefab = PrefabManager.Instance.CreateClonedPrefab(CustomPrefabNames.Cauldron, ItemNames.Cauldron);
        if (cauldronPrefab != null)
        {
          cauldronPrefab.transform.Find("HaveFire").gameObject.SetActive(true);
          DestroyImmediate(cauldronPrefab.GetComponent<CraftingStation>());
          DestroyImmediate(cauldronPrefab.GetComponent<WearNTear>());
          cauldronPrefab.GetComponent<Piece>().m_canBeRemoved = false;
          PrefabManager.Instance.AddPrefab(cauldronPrefab);
        }
      }

      if (cauldronPrefab == null)
      {
        Log.Error($"[{GetType().Name}] Error with prefabs.");
        Log.Error($"[{GetType().Name}] cauldronPrefab == null: true");
      }
    }
  }
}
