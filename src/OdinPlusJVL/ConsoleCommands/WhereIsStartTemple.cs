using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.ConsoleCommands
{
  internal class WhereIsStartTemple : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.WhereIsStartTemple;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (ZoneSystem.instance.FindClosestLocation("StartTemple", Vector3.zero, out var startTemple))
        {
          var msg = $"{startTemple.m_position.x},{startTemple.m_position.y},{startTemple.m_position.z}";
          GuiUtils.PrintToCenterOfScreen(msg);
          ConsoleCommandManager.Instance.WriteToConsole($"(x,y,z) {msg}");
          Log.Trace(Main.Instance, $"[{GetType().Name}] {msg}");
        }
        else
        {
          Log.Trace(Main.Instance, $"[{GetType().Name}] Unable to find Location StartTemple");
          Log.Trace(Main.Instance, $"[{GetType().Name}] Available Locations:");

          foreach (var zoneLocation in ZoneSystem.instance.m_locations)
          {
            Log.Trace(Main.Instance, $"[{GetType().Name}] {zoneLocation.m_prefabName}" );
          }
        }
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }
  }
}
