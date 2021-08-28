using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using System;
using System.Reflection;

namespace OdinPlusJVL.ConsoleCommands
{
  internal class ForceLoadCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.ForceLoad;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }
  }
}
