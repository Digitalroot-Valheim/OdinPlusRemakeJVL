using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using System;
using System.Reflection;
using OdinPlusRemakeJVL.Common.Names;
using OdinPlusRemakeJVL.Common.Utils;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal class WhereAmICommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.WhereAmI;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var position = Common.Utils.Utils.GetLocalPlayersPosition();
        var msg = $"{position.x},{position.y},{position.z}";
        GuiUtils.PrintToCenterOfScreen(msg);
        ConsoleCommandManager.Instance.WriteToConsole($"(x,y,z) {msg}");
        Log.Trace($"{GetType().Name}: {msg}");
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
