using OdinPlusRemakeJVL.Common;
using System;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal class ForceLoadCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.ForceLoad;

    public override void Run(string[] args)
    {
      try
      {

      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
