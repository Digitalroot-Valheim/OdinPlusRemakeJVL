using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OdinPlusRemakeJVL.Common.Names;
using OdinPlusRemakeJVL.Managers;
using System.IO;
using UnityEngine;

namespace OdinPlusRemakeJVL.Common.Utils
{
  public static class Utils
  {
    internal static IEnumerable<string> AllNames(Type type)
    {
      var f = type.GetFields().Where(f1=> f1.FieldType == typeof(string));
      // var m = type.GetMembers();
      // var p = type.GetProperties();
      foreach (var fieldInfo in f)
      {
        yield return fieldInfo.GetValue(null).ToString();
      }
    }

    // Source: EpicLoot
    public static bool IsObjectDBReady()
    {
      Log.Trace($"OdinPlusRemakeJVL.Common.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      // Hack, just making sure the built-in items and prefabs have loaded
      return ObjectDB.instance != null && ObjectDB.instance.m_items.Count != 0 && ObjectDB.instance.GetItemPrefab("Amber") != null;
    }
    
    public static bool IsZNetSceneReady()
    {
      Log.Trace($"OdinPlusRemakeJVL.Common.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      // Log.Trace($"ZNetScene.instance != null : {ZNetScene.instance != null}");
      // Log.Trace($"ZNetScene.instance?.m_prefabs != null : {ZNetScene.instance?.m_prefabs != null}");
      // Log.Trace($"ZNetScene.instance?.m_prefabs?.Count > 0 : {ZNetScene.instance?.m_prefabs?.Count}");
      return ZNetScene.instance != null && ZNetScene.instance?.m_prefabs != null && ZNetScene.instance?.m_prefabs?.Count > 0;
    }

    public static bool IsZNetReady()
    {
      Log.Trace($"OdinPlusRemakeJVL.Common.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      Log.Trace($"ZNet.instance != null : {ZNet.instance != null}");
      return ZNet.instance != null;
    }
    
    public static Vector3 GetGroundHeight(int x, int z) => GetGroundHeight(new Vector3Int(x, 500, z));

    public static Vector3 GetGroundHeight(float x, float z) => GetGroundHeight(new Vector3(x, 500, z));

    public static Vector3 GetGroundHeight(Vector3Int vector3) => new Vector3(vector3.x, ZoneSystem.instance.GetGroundHeight(vector3), vector3.z);

    public static Vector3 GetGroundHeight(Vector3 vector3) => new Vector3(vector3.x, ZoneSystem.instance.GetGroundHeight(vector3), vector3.z);

    public static Vector3 GetLocalPlayersPosition() => Player.m_localPlayer.transform.position;

    public static Vector3 GetStartTemplesPosition()
    {
      if (ZoneSystem.instance.FindClosestLocation(LocationNames.StartTemple, Vector3.zero, out ZoneSystem.LocationInstance locationInstance))
      {
        Log.Trace($"[GetStartTemplesPosition] StartTemple at {locationInstance.m_position}");
        return locationInstance.m_position;
      }
      Log.Error($"[GetStartTemplesPosition] Can't find StartTemple");
      
      return Vector3.zero;
    }

    public static GameObject Spawn(string prefabName, Vector3 location, Transform parent)
    {
      var prefab = PrefabManager.Instance.GetPrefab(prefabName);
      if (prefab == null) return null;
      var instance = UnityEngine.Object.Instantiate(prefab, location, Quaternion.identity, parent);
      // PrefabManager.Instance.RegisterToZNetScene(instance);
      return instance;
    }

    public static GameObject Spawn(GameObject prefab, Vector3 location, Transform parent)
    {
      Log.Trace($"OdinPlusRemakeJVL.Common.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}({prefab.name}, {location}, {parent.name})");
      var instance = UnityEngine.Object.Instantiate(prefab, location, Quaternion.identity, parent);
      // PrefabManager.Instance.RegisterToZNetScene(instance);
      return instance;
    }

    public static string Localize(string value)
    {
      return Localization.instance.Localize(value);
    }

    public static DirectoryInfo AssemblyDirectory
    {
      get
      {
        string codeBase = Assembly.GetExecutingAssembly().CodeBase;
        UriBuilder uri = new UriBuilder(codeBase);
        return new DirectoryInfo(Uri.UnescapeDataString(uri.Path));
      }
    }
  }
}
