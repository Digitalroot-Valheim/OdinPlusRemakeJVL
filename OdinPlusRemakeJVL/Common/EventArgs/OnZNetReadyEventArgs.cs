namespace OdinPlusRemakeJVL.Common.EventArgs
{
  class OnZNetReadyEventArgs : System.EventArgs
  {
    public ZNet ZNet { get; }

    public OnZNetReadyEventArgs(ZNet zNet)
    {
      ZNet = zNet;
    }
  }
}
