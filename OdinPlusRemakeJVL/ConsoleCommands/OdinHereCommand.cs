using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;
using OdinPlusRemakeJVL.Npcs;
using System;
using System.Reflection;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal class OdinHereCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.OdinHere;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        LocationManager.Instance.GetStartPosition();
        // OdinNpc.Instance.SetPosition();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
