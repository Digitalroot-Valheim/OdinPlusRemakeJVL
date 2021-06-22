using OdinPlusRemakeJVL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OdinPlusRemakeJVL.ConsoleCommands;

namespace OdinPlusRemakeJVL.Managers
{
  public class HealthManager : Singleton<HealthManager>, IAbstractManager
  {
    private readonly List<HealthCheckStatus> _healthCheckStatusList = new List<HealthCheckStatus>();

    public bool IsInitialized { get; private set; }

    public void Initialize()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      IsInitialized = true;
    }

    public bool PostInitialize()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      ConsoleCommandManager.Instance.AddConsoleCommand(new HealthCheckCommand(Instance));
      return true;
    }

    #region Event

    public delegate HealthCheckStatus HealthCheckEventHandler();

    public event HealthCheckEventHandler OnHealthCheck;

    public void HealthCheck()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (OnHealthCheck == null)
      {
        Log.Info("Nothing to Monitor");
        return;
      }

      _healthCheckStatusList.Clear();
      Log.Info("Requesting Health Check");
      Log.Info($"ZNetScene.instance.name = {ZNetScene.instance?.name}");
      Log.Info($"ObjectDB.instance.name = {ObjectDB.instance?.name}");
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

      Log.Info($"Health Check - Results ({_healthCheckStatusList.Count})");
      foreach (HealthCheckStatus healthCheckStatus in _healthCheckStatusList)
      {
        Log.Info($"| {healthCheckStatus.HealthStatus.ToString().PadRight(10)}| {healthCheckStatus.Name} ");
      }

      foreach (HealthCheckStatus healthCheckStatus in _healthCheckStatusList)
      {
        if (healthCheckStatus.Reasons.Count == 0) continue;
        Log.Info("------------------------------------------");
        foreach (var reason in healthCheckStatus.Reasons)
        {
          Log.Info($"| {healthCheckStatus.Name} | {reason}");
        }
        Log.Info("------------------------------------------");
      }
    }

    private static void HandleDelegateError(MethodInfo method, Exception exception)
    {
      Log.Error($"[{method}] {exception.Message}");
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
