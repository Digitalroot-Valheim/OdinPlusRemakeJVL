﻿using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using OdinPlusJVL.Common;
using OdinPlusJVL.Common.Interfaces;
using System;
using System.Reflection;

namespace OdinPlusJVL.Managers
{
  /// <summary>
  /// Source: https://github.com/Digitalroot/digitalroot-valheim-mods/blob/main/Digitalroot.Valheim.Common/Core/Managers/AbstractManager.cs
  /// Permission granted to OdinPlus under 
  /// License: "GNU Affero General Public License v3.0"
  /// License ref: https://github.com/Digitalroot/digitalroot-valheim-mods/blob/main/LICENSE
  /// </summary>
  public abstract class AbstractManager<T> : Singleton<T>, IInitializeable where T : AbstractManager<T>, new()
  {
    public bool IsInitialized { get; private protected set; }

    protected AbstractManager()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      Log.Trace(Main.Instance, $"[{GetType().BaseType?.Name}] {nameof(IsInitialized)}: {IsInitialized}");
      if (IsInitialized) return;
      // ReSharper disable once VirtualMemberCallInConstructor
      Initialize();
    }

    public virtual void Initialize()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      Log.Trace(Main.Instance, $"[{GetType().Name}] {nameof(IsInitialized)}: {IsInitialized}");
      if (IsInitialized) return;
      if (HasDependencyError())
      {
        Log.Fatal(Main.Instance, $"[{GetType().Name}]: HasDependencyError(): true");
        Log.Fatal(Main.Instance, $"[{GetType().Name}]: Checking health status");
        HealthManager.Instance.HealthCheck();
        return;
      }
      
      IsInitialized = OnInitialize();
      if (!IsInitialized)
      {
        Log.Fatal(Main.Instance, $"[{GetType().Name}]: IsInitialized: {IsInitialized}");
        Log.Fatal(Main.Instance, $"[{GetType().Name}]: Failed to IsInitialize");
        Log.Fatal(Main.Instance, $"[{GetType().Name}]: Checking health status");
        HealthManager.Instance.HealthCheck();
        HealthManager.Instance.OnHealthCheck -= HealthCheck;
      }
    }

    protected virtual bool OnInitialize()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
        HealthManager.Instance.OnHealthCheck += HealthCheck;
        return true;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        return false;
      }
    }

    public virtual bool PostInitialize()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      Log.Trace(Main.Instance, $"[{GetType().Name}] {nameof(IsInitialized)}: {IsInitialized}");
      return OnPostInitialize();
    }

    protected virtual bool OnPostInitialize()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      return IsInitialized;
    }

    public HealthCheckStatus HealthCheck()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().BaseType?.Name}.{MethodBase.GetCurrentMethod().Name}({GetType().Name})");
      var healthCheckStatus = new HealthCheckStatus
      {
        Name = MethodBase.GetCurrentMethod().DeclaringType?.Name,
        HealthStatus = HealthStatus.Healthy
      };

      try
      {
        if (!IsInitialized)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{GetType().Name}]: IsInitialized: {IsInitialized}";
        }
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        healthCheckStatus.HealthStatus = HealthStatus.Failed;
        healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: {e.Message}";
      }

      return OnHealthCheck(healthCheckStatus);
    }

    public virtual bool HasDependencyError()
    {
      return false;
    }

    protected abstract HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus);
  }
}
