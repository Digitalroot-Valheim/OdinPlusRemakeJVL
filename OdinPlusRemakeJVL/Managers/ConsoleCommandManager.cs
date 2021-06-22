using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jotunn.Managers;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.ConsoleCommands;

namespace OdinPlusRemakeJVL.Managers
{
  internal class ConsoleCommandManager : AbstractManager<ConsoleCommandManager>
  {
    private readonly Dictionary<string, AbstractOdinPlusConsoleCommand> _consoleCommandDictionary = new Dictionary<string, AbstractOdinPlusConsoleCommand>();

    protected override bool OnPostInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnPostInitialize()) return false;
        AddConsoleCommands();
        return true;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return false;
      }
    }

    public override bool HasDependencyError()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      return false;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = GetType().Name;
        if (!_consoleCommandDictionary.Any())
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _consoleCommandDictionary.Any(): false";
        }
        return healthCheckStatus;
      }
      catch (Exception e)
      {
        Log.Error(e);
        healthCheckStatus.HealthStatus = HealthStatus.Failed;
        healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: {e.Message}";
        return healthCheckStatus;
      }
    }

    private void AddConsoleCommands()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var kvpConsoleCommand in _consoleCommandDictionary) 
      {
        CommandManager.Instance.AddConsoleCommand(kvpConsoleCommand.Value);
      }
    }

    public void AddConsoleCommand(AbstractOdinPlusConsoleCommand consoleCommand)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _consoleCommandDictionary.Add(consoleCommand.Name, consoleCommand);
    }
  }
}
