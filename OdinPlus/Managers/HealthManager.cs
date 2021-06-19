using OdinPlus.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OdinPlus.Managers
{
  public class HealthManager : Singleton<HealthManager>
  {
    private protected const string Namespace = "OdinPlus.Managers";
    private readonly List<HealthCheckStatus> _healthCheckStatusList = new List<HealthCheckStatus>();

    #region Event

    public delegate HealthCheckStatus HealthCheckEventHandler();

    public event HealthCheckEventHandler OnHealthCheck;

    public void HealthCheck()
    {
      Log.Trace($"{GetType().Namespace}.{MethodBase.GetCurrentMethod().DeclaringType?.Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (OnHealthCheck == null)
      {
        Log.Info("Nothing to Monitor");
        return;
      }

      _healthCheckStatusList.Clear();
      Log.Info("Requesting Health Check");
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
      // private get { return ""; }
      set => _reasons.Add(value);
    }

    public List<string> Reasons => _reasons;
  }
}
