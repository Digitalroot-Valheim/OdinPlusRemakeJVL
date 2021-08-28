using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;
using System;
using System.Reflection;

namespace OdinPlusJVL.ConsoleCommands
{
  internal class OdinHereCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.OdinHere;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        LocationManager.Instance.GetStartPosition();
        // OdinNpc.Instance.SetPosition();
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }
  }
}
