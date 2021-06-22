using System;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;

namespace OdinPlusRemakeJVL.ConsoleCommands
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
        _healthManager.HealthCheck();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
