using OdinPlusRemakeJVL.Common;
using System;
using System.Reflection;
using OdinPlusRemakeJVL.Common.Names;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal class ForceLoadCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.ForceLoad;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");

      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
