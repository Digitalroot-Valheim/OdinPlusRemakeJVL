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

    public override string Name => Common.ConsoleCommandNames.HealthCheck;

    public override void Run(string[] args)
    {
      _healthManager.HealthCheck();
    }
  }
}
