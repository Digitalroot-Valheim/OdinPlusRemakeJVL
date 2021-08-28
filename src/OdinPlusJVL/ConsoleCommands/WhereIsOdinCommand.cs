using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;
using OdinPlusJVL.OdinsCamp;
using System;
using System.Reflection;

namespace OdinPlusJVL.ConsoleCommands
{
  internal class WhereIsOdinCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.WhereOdin;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var position = OdinsCampManager.Instance.GetOdinCampLocation();
        var msg = $"{position.x},{position.y},{position.z}";
        GuiUtils.PrintToCenterOfScreen(msg);
        ConsoleCommandManager.Instance.WriteToConsole($"Odin's Camp is at (x,y,z) {msg}");
        Log.Trace(Main.Instance, $"{GetType().Name}: Odin's Camp is at (x,y,z) {msg}");
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }
  }
}
