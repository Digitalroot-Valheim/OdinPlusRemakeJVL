using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using System;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal class WhereAmICommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.WhereAmI;

    public override void Run(string[] args)
    {
      try
      {
        var position = Common.Utils.GetLocalPlayersPosition();
        var msg = $"{position.x},{position.y},{position.z}";
        GuiUtils.PrintToCenterOfScreen(msg);
        ConsoleCommandManager.Instance.WriteToConsole($"(x,y,z) {msg}");
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
