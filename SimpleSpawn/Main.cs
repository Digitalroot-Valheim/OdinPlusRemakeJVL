using BepInEx;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Reflection;
using UnityEngine;

namespace SimpleSpawn
{
  [BepInPlugin(Guid, Name, Version)]
  public class Main : BaseUnityPlugin
  {
    public const string Version = "1.0.0";
    public const string Name = "Digitalroot Plug-in Simple Spawner";
    public const string Guid = "digitalroot.mods.simplespawner";
    public const string Namespace = "SimpleSpawn";
    private Harmony _harmony;
    public static Main Instance;
    public Main()
    {
      Instance = this;
    }

    [UsedImplicitly]
    private void Awake()
    {
      _harmony = Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), Guid);
    }

    [UsedImplicitly]
    private void OnDestroy()
    {
      _harmony?.UnpatchSelf();
    }
  }

  [HarmonyPatch(typeof(Game))]
  public static class PatchZoneSystem
  {
    [HarmonyPostfix]
    [HarmonyPatch("SpawnPlayer")]
    [HarmonyPriority(Priority.Normal)]
    [UsedImplicitly]
    public static void PostfixLoad(Vector3 spawnPoint)
    {
      try
      {
        ZLog.Log("************* Simple Spawner *************");
        // GameObject prefab = ZNetScene.instance.GetPrefab("Troll");
        // Character component2 = UnityEngine.Object.Instantiate<GameObject>(prefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up, Quaternion.identity).GetComponent<Character>();
        // var x = new TrollSpawner();
        // x.Invoke("SpawnTrolls", 5f);

        if (Player.m_localPlayer == null)
        {
          ZLog.LogWarning("Player is null");
          // return;
        }

        ZLog.LogWarning("Trying to spawn Troll");
        GameObject prefab = ZNetScene.instance.GetPrefab("fire_pit");
        ZLog.LogWarning($"prefab == null: {prefab == null}");
        UnityEngine.Object.Instantiate(prefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up, Quaternion.identity);
        ZLog.LogWarning("Troll should have spawned");
      }
      catch (Exception e)
      {
        ZLog.LogError(e);
      }
    }
  }

  public class TrollSpawner : MonoBehaviour
  {
    public void SpawnTrolls()
    {
      ZLog.Log("************* Simple Spawner *************");
      GameObject prefab = ZNetScene.instance.GetPrefab("Troll");
      Character component2 = UnityEngine.Object.Instantiate<GameObject>(prefab, Player.m_localPlayer.transform.position + Player.m_localPlayer.transform.forward * 2f + Vector3.up, Quaternion.identity).GetComponent<Character>();
    }
  }
}
