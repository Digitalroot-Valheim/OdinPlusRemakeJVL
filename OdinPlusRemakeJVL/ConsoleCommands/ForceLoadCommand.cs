using System;
using OdinPlusRemakeJVL.Common;
using OdinPlusRemakeJVL.Managers;

namespace OdinPlusRemakeJVL.ConsoleCommands
{
  internal class ForceLoadCommand : AbstractOdinPlusConsoleCommand
  {
    public override string Name => ConsoleCommandNames.ForceLoad;

    public override void Run(string[] args)
    {
      try
      {
        // FxAssetManager.Instance.Initialize();
        // FxAssetManager.Instance.PostInitialize();        // FxAssetManager.Instance.Initialize();
        // FxAssetManager.Instance.PostInitialize();
      }
      catch (Exception e)
      {
        Log.Error(e);
      }
    }
  }
}
