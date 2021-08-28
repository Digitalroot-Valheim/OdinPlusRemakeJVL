using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using OdinPlusJVL.Common;
using OdinPlusJVL.ConsoleCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OdinPlusJVL.Managers
{
  public class HealthManager : Singleton<HealthManager>, IInitializeable
  {
    private readonly List<HealthCheckStatus> _healthCheckStatusList = new List<HealthCheckStatus>();

    public bool IsInitialized { get; private set; }

    public void Initialize()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      IsInitialized = true;
    }

    public bool PostInitialize()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      ConsoleCommandManager.Instance.AddConsoleCommand(new HealthCheckCommand(Instance));
      return true;
    }

    #region Event

    public delegate HealthCheckStatus HealthCheckEventHandler();

    public event HealthCheckEventHandler OnHealthCheck;

    public void HealthCheck()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (OnHealthCheck == null)
      {
        Log.Info(Main.Instance, "Nothing to Monitor");
        return;
      }

      _healthCheckStatusList.Clear();
      Log.Info(Main.Instance, "Requesting Health Check");
      Log.Info(Main.Instance, $"ZNetScene.instance.name = {ZNetScene.instance?.name}");
      Log.Info(Main.Instance, $"ObjectDB.instance.name = {ObjectDB.instance?.name}");
      foreach (var @delegate in OnHealthCheck.GetInvocationList().ToList())
      {
        HealthCheckEventHandler subscriber = (HealthCheckEventHandler)@delegate;
        try
        {
          _healthCheckStatusList.Add(subscriber());
        }
        catch (Exception e)
        {
          HandleDelegateError(subscriber.Method, e);
        }
      }

      Log.Info(Main.Instance, $"Health Check - Results ({_healthCheckStatusList.Count})");
      foreach (HealthCheckStatus healthCheckStatus in _healthCheckStatusList)
      {
        Log.Info(Main.Instance, $"| {healthCheckStatus.HealthStatus.ToString().PadRight(10)}| {healthCheckStatus.Name} ");
      }

      foreach (HealthCheckStatus healthCheckStatus in _healthCheckStatusList)
      {
        if (healthCheckStatus.Reasons.Count == 0) continue;
        Log.Info(Main.Instance, "------------------------------------------");
        foreach (var reason in healthCheckStatus.Reasons)
        {
          Log.Info(Main.Instance, $"| {healthCheckStatus.Name} | {reason}");
        }
        Log.Info(Main.Instance, "------------------------------------------");
      }
    }

    private static void HandleDelegateError(MethodInfo method, Exception exception)
    {
      Log.Error(Main.Instance, $"[{method}] {exception.Message}");
    }
    #endregion
  }

  public enum HealthStatus
  {
    Failed,
    Unhealthy,
    Healthy,
  }

  public class HealthCheckStatus
  {
    private readonly List<string> _reasons = new List<string>();
    public string Name { get; set; }
    public HealthStatus HealthStatus { get; set; }

    public string Reason
    {
      set => _reasons.Add(value);
    }

    public List<string> Reasons => _reasons;
  }
}
