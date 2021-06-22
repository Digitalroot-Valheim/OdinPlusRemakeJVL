﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OdinPlusRemakeJVL.Common
{
  public static class Utils
  {
    internal static IEnumerable<string> AllNames(System.Type type)
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
      Log.Trace($"OdinPlusRemakeJVL.Common.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      // Hack, just making sure the built-in items and prefabs have loaded
      return ObjectDB.instance != null && ObjectDB.instance.m_items.Count != 0 && ObjectDB.instance.GetItemPrefab("Amber") != null;
    }


    public static bool IsZNetSceneReady()
    {
      Log.Trace($"OdinPlusRemakeJVL.Common.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}");
      // Log.Trace($"ZNetScene.instance != null : {ZNetScene.instance != null}");
      // Log.Trace($"ZNetScene.instance?.m_prefabs != null : {ZNetScene.instance?.m_prefabs != null}");
      // Log.Trace($"ZNetScene.instance?.m_prefabs?.Count > 0 : {ZNetScene.instance?.m_prefabs?.Count}");
      return ZNetScene.instance != null && ZNetScene.instance?.m_prefabs != null && ZNetScene.instance?.m_prefabs?.Count > 0;
    }
  }
}
