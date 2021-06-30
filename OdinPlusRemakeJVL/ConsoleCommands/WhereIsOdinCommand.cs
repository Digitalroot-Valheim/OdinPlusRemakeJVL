using System;
using System.Reflection;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Common.Names;
using OdinPlusRemakeJVL.Common.Utils;
using OdinPlusRemakeJVL.Managers;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal class WhereIsOdinCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.WhereOdin;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var position = OdinsCampManager.Instance.GetOdinCampLocation();
        var msg = $"{position.x},{position.y},{position.z}";
        GuiUtils.PrintToCenterOfScreen(msg);
        ConsoleCommandManager.Instance.WriteToConsole($"Odin's Camp is at (x,y,z) {msg}");
        Log.Trace($"{GetType().Name}: Odin's Camp is at (x,y,z) {msg}");


        position = OdinsCampManager.Instance.GetOdinNpcLocation();
        msg = $"{position.x},{position.y},{position.z}";
        ConsoleCommandManager.Instance.WriteToConsole($"Odin is at (x,y,z) {msg}");
        Log.Trace($"{GetType().Name}: Odin is at (x,y,z) {msg}");
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
