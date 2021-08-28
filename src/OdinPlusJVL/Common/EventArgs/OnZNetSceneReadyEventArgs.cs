namespace OdinPlusJVL.Common.EventArgs
{
  class OnZNetSceneReadyEventArgs : System.EventArgs
  {
    public ZNetScene ZNetScene { get; }

    public OnZNetSceneReadyEventArgs(ZNetScene zZNetScene)
    {
      ZNetScene = zZNetScene;
    }
  }
}
