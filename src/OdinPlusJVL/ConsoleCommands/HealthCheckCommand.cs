using Digitalroot.Valheim.Common;
using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;
using System;
using System.Reflection;

namespace OdinPlusJVL.ConsoleCommands
{
  internal class HealthCheckCommand : AbstractOdinPlusConsoleCommand
  {
    private readonly HealthManager _healthManager;

    public HealthCheckCommand(HealthManager healthManager)
    {
      _healthManager = healthManager;
    }

    public override string Name => ConsoleCommandNames.HealthCheck;

    public override void Run(string[] args)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        _healthManager.HealthCheck();
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
      }
    }
  }
}
