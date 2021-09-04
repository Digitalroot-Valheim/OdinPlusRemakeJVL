using Digitalroot.Valheim.Common;
using OdinPlusJVL.Managers;
using System;
using System.Reflection;

namespace OdinPlusJVL.Quests
{
  public class QuestManager : AbstractManager<QuestManager>
  {
    #region Overrides of AbstractManager<QuestManager>

    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;

        
        return true;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        return false;
      }
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = GetType().Name;
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

    #endregion

    public int Level => 333;

    public int Count() => 444;
  }
}
