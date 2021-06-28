using System;
using System.Reflection;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using UnityEngine;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal class WhereIsStartTemple : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.WhereIsStartTemple;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (ZoneSystem.instance.FindClosestLocation("StartTemple", Vector3.zero, out var startTemple))
        {
          var msg = $"{startTemple.m_position.x},{startTemple.m_position.y},{startTemple.m_position.z}";
          GuiUtils.PrintToCenterOfScreen(msg);
          ConsoleCommandManager.Instance.WriteToConsole($"(x,y,z) {msg}");
          Log.Trace($"[{GetType().Name}] {msg}");
        }
        else
        {
          Log.Trace($"[{GetType().Name}] Unable to find Location StartTemple");
          Log.Trace($"[{GetType().Name}] Available Locations:");

          foreach (var zoneLocation in ZoneSystem.instance.m_locations)
          {
            Log.Trace($"[{GetType().Name}] {zoneLocation.m_prefabName}" );
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
