using JetBrains.Annotations;
using OdinPlus.Common;
using OdinPlus.Managers;
using OdinPlus.Processors;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlus.Quests
{
  public class LegacyChest : MonoBehaviour
  {
    private ZNetView _zNetView;
    public bool Placing;
    public bool m_sphy; // ToDo: What is this for? It causes DestroyImmediate to be called.

    //upd maybe make this private box??public bool isPublic = false;
    public string Id;
    public string OwnerName = "";
    private Container _container;

    [UsedImplicitly]
    private void Awake()
    {
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

      _zNetView = gameObject.GetComponent<ZNetView>();
      _container = gameObject.GetComponent<Container>();
      var zdo = _zNetView.GetZDO();
      if (Placing)
      {
        zdo.Set("QuestID", Id);
        zdo.Set("QuestSphy", m_sphy);
        zdo.Set("QuestOwner", OwnerName);
        return;
      }

      Id = zdo.GetString("QuestID", "public");
      m_sphy = zdo.GetBool("QuestSphy", true);
      OwnerName = zdo.GetString("QuestOwner", "public");
      if (!m_sphy)
      {
        DestroyImmediate(GetComponent<StaticPhysics>());
      }
      /* 			if (ZNet.instance.IsServer() && ZNet.instance.IsDedicated())
            {
              Destroy(gameObject);
            } */
    }

    [UsedImplicitly]
    private void Update()
    {
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");

      if (_container == null)
      {
        Log.Debug("_container is null");
        return;
      }

      if (_container?.GetInventory() == null)
      {
        Log.Debug("Cant find inv");
        return;
      }

      if (_container?.GetInventory()?.NrOfItems() == 0)
      {
        var ravenPrefab = NpcManager.RavenPrefab?.GetComponent<Raven>()?.m_despawnEffect?.m_effectPrefabs[0]?.m_prefab;
        if (ravenPrefab == null)
        {
          Log.Debug("ravenPrefab is null");
          return;
        }

        var go = GetComponent<GameObject>();

        if (go == null)
        {
          Log.Debug("gameobject is null");
          return;
        }

        Instantiate(ravenPrefab, go.transform.position, Quaternion.identity);
        ZNetScene.instance?.Destroy(gameObject);
      }
    }

    //HELP how to make a delegate here?//notice
    public void OnOpen(Humanoid user, bool hold)
    {
      try
      {
        Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (hold)
        {
          return;
        }

        if (user.GetHoverName() == OwnerName)
        {
          var quest = QuestManager.Instance.GetQuest(Id);
          if (quest != null)
          {
            //upd should select in base? yes!!!!!!!!!
            QuestProcessor.Create(quest).Finish();
            return;
          }
          //upd giveup without destroy?
        }

        string n = $"Hey you found the chest belong to <color=yellow><b>{OwnerName}</b></color>"; //trans
        DBG.InfoCT(n);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(user), user);
        e.Data.Add(nameof(hold), hold);
        throw;
      }
    }

    public void WatchMe()
    {
      Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      GameCamera.instance.transform.localPosition = transform.position + Vector3.forward * 1;
    }

    #region Static

    public static GameObject Place(Vector3 position, Quaternion rotation, float p_range, string p_id, string p_owner, int p_key, bool sphy = true)
    {
      try
      {
        Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}(Vector3, Quaternion, float, string, string, int, bool)");
        DestroyContainer(position, p_range);
        return Place(position, p_id, p_owner, p_key, rotation, sphy);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(position), JsonSerializationProvider.ToJson(position));
        e.Data.Add(nameof(rotation), JsonSerializationProvider.ToJson(rotation));
        e.Data.Add(nameof(p_range), p_range);
        e.Data.Add(nameof(p_id), p_id);
        e.Data.Add(nameof(p_owner), p_owner);
        e.Data.Add(nameof(p_key), p_key);
        e.Data.Add(nameof(sphy), sphy);
        throw new Exception($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}(Vector3, Quaternion, float, string, string, int, bool)", e);
      }
    }

    public static void DestroyContainer(Vector3 position, float p_range)
    {
      try
      {
        Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
        Collider[] array = Physics.OverlapBox(position, Vector3.one * p_range);
        foreach (var col in array)
        {
          Container ctn = col.GetComponent<Container>();
          Container ctn2 = col.transform?.parent?.GetComponent<Container>();
          Container ctn3 = col.transform?.parent?.parent?.GetComponent<Container>();
          if (ctn != null)
          {
            ctn.gameObject.GetComponent<ZNetView>().Destroy();
            Log.Debug("Find destroyable ctn");
            return;
          }

          if (ctn2 != null)
          {
            col.transform?.parent?.GetComponent<ZNetView>().Destroy();
            Log.Debug("Find destroyable ctn parent");
            return;
          }

          if (ctn3 != null)
          {
            ctn3?.GetComponent<ZNetView>().Destroy();
            Log.Debug("Find destroyable ctn grandparent");
            return;
          }
        }
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(position), JsonSerializationProvider.ToJson(position));
        e.Data.Add(nameof(p_range), p_range);
        throw new Exception($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}", e);
      }
    }

    public static GameObject Place(Vector3 position, string p_id, string p_owner, int p_key, bool sphy = true)
    {
      try
      {
        Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}(Vector3, string, string, int, bool)");
        return Place(position, p_id, p_owner, p_key, Quaternion.identity, sphy);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(position), JsonSerializationProvider.ToJson(position));
        e.Data.Add(nameof(p_id), p_id);
        e.Data.Add(nameof(p_owner), p_owner);
        e.Data.Add(nameof(p_key), p_key);
        e.Data.Add(nameof(sphy), sphy);
        throw new Exception($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}(Vector3, string, string, int, bool)", e);
      }
    }

    public static GameObject Place(Vector3 position, string p_id, string p_owner, int p_key, Quaternion rotation, bool sphy = true)
    {
      try
      {
        Log.Trace($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}(Vector3, string, string, int, Quaternion, bool)");
        GameObject chestGameObject = ZNetScene.instance.GetPrefab($"LegacyChest{p_key + 1}");

        if (chestGameObject == null)
        {
          Log.Error($"chestGameObject is null. Tried to get prefab: LegacyChest{p_key + 1}");
          Log.Error($"PrefabManager.IsInitialized: {PrefabManager.Instance.IsInitialized}");
          return null;
        }

        var chest = Instantiate(chestGameObject, position, rotation, OdinPlus.PrefabParent.transform);

        var lc = chest.GetComponent<LegacyChest>();
        lc.Placing = true;
        lc.Id = p_id;
        lc.m_sphy = sphy;
        lc.OwnerName = p_owner;

        if (!sphy)
        {
          DestroyImmediate(chest.GetComponent<StaticPhysics>());
        }

        Log.Debug($"Placed Chest at {position}");
        chest.transform.SetParent(OdinPlus.Root.transform);
        return chest;

      }
      catch (Exception e)
      {
        e.Data.Add(nameof(position), JsonSerializationProvider.ToJson(position));
        e.Data.Add(nameof(p_id), p_id);
        e.Data.Add(nameof(p_owner), p_owner);
        e.Data.Add(nameof(p_key), p_key);
        e.Data.Add(nameof(rotation), JsonSerializationProvider.ToJson(rotation));
        e.Data.Add(nameof(sphy), sphy);
        throw new Exception($"{Main.Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}(Vector3, string, string, int, Quaternion, bool)", e);
      }
    }

    #endregion Static
  }
}
