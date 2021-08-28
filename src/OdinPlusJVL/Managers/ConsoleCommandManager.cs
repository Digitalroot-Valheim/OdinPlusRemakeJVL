using Digitalroot.Valheim.Common;
using Jotunn.Managers;
using OdinPlusJVL.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OdinPlusJVL.Managers
{
  internal class ConsoleCommandManager : AbstractManager<ConsoleCommandManager>
  {
    private readonly Dictionary<string, AbstractOdinPlusConsoleCommand> _consoleCommandDictionary = new Dictionary<string, AbstractOdinPlusConsoleCommand>();

    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;
        
        AddConsoleCommand(new GotoOdinCommand());
        AddConsoleCommand(new OdinHereCommand());
        AddConsoleCommand(new WhereAmICommand());
        AddConsoleCommand(new WhereIsStartTemple());
        AddConsoleCommand(new WhereIsOdinCommand());

        return true;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        return false;
      }
    }

    protected override bool OnPostInitialize()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnPostInitialize()) return false;
        AddConsoleCommands();
        return true;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        return false;
      }
    }

    public override bool HasDependencyError()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      return false;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
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
        Log.Error(Main.Instance, e);
        healthCheckStatus.HealthStatus = HealthStatus.Failed;
        healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: {e.Message}";
        return healthCheckStatus;
      }
    }

    public void WriteToConsole(string msg)
    {
      Console.instance.Print(msg);
    }

    private void AddConsoleCommands()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var kvpConsoleCommand in _consoleCommandDictionary) 
      {
        CommandManager.Instance.AddConsoleCommand(kvpConsoleCommand.Value);
      }
    }

    public void AddConsoleCommand(AbstractOdinPlusConsoleCommand consoleCommand)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({consoleCommand.Name})");
      _consoleCommandDictionary.Add(consoleCommand.Name, consoleCommand);
    }
  }
}
