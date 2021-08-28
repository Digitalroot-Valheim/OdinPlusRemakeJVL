using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Interfaces;
using System;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Managers
{
  internal class LocationManager : AbstractManager<LocationManager>, IOnZNetReady
  {
    private Vector3 _odinsPosition = Vector3.zero;

    protected override bool OnInitialize()
    {
      try
      {
        if (!base.OnInitialize()) return false;
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        return true;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        return false;
      }
    }

    public void OnZNetReady(ZNet zNet)
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (HasDependencyError()) return;
      SetupRPC();
      GetStartPosition();
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      return healthCheckStatus;
    }

    private void SetupRPC()
    {
      ZRoutedRpc.instance.Register(_rpcRPC_SetStartPos, new Action<long, Vector3>(RPC_SetStartPos));
      // ZRoutedRpc.instance.Register("RPC_ReceiveServerFOP", new Action<long, bool>(RPC_ReceiveServerFOP));
      if (ZNet.instance.IsServer())
      {
        ZRoutedRpc.instance.Register("RPC_GetStartPos", RPC_GetStartPosition);
        // ZRoutedRpc.instance.Register("RPC_SendServerFOP", RPC_SendServerFOP);
        // ZRoutedRpc.instance.Register("RPC_ServerFindLocation", new Action<long, string, Vector3>(RPC_ServerFindLocation));
      }
    }

    public void GetStartPosition()
    {
      ZRoutedRpc.instance.InvokeRoutedRPC(_rpcGetStartPosition);
    }

    #region RPC

    private readonly string _rpcGetStartPosition = "RPC_GetStartPosition";
    private readonly string _rpcRPC_SetStartPos = "RPC_SetStartPos";

    private void RPC_GetStartPosition(long sender)  
    {
      Log.Debug(Main.Instance, "Server received Odin's Position request");
      if (Main.ConfigEntryOdinPosition.Value != Vector3.zero)
      {
        _odinsPosition = Main.ConfigEntryOdinPosition.Value;
      }

      ZRoutedRpc.instance.InvokeRoutedRPC(sender, _rpcRPC_SetStartPos, _odinsPosition);
    }

    private void RPC_SetStartPos(long sender, Vector3 pos)
    {
      Log.Debug(Main.Instance, $"Client received Odin position {pos}");
      // OdinsCampManager.Root.transform.localPosition = pos;
    }

    #endregion
  }
}
