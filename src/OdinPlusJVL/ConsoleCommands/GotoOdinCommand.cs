using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;
using System;
using System.Reflection;

namespace OdinPlusJVL.ConsoleCommands
{
  internal class GotoOdinCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.GotoOdin;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var player = Player.m_localPlayer.transform.position;
        var odin = OdinsCampManager.Instance.GetOdinCampLocation();
        Log.Trace(Main.Instance, $"[{GetType().Name}] Moving player from ({player.x},{player.y},{player.z}) to ({odin.x},{odin.y},{odin.z})");
        Player.m_localPlayer.transform.position = odin;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }
  }
}
