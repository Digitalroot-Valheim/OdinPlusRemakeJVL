using Digitalroot.Valheim.Common;
using OdinPlusJVL.ConsoleCommands;
using OdinPlusJVL.Managers;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL
{
  public partial class Main
  {
    #region Refactor
    public static List<string> traderNameList = new List<string>();
    public static int Credits;
    public static void AddCredits(int val, Transform m_head)
    {
      AddCredits(val);
      Player.m_localPlayer.m_skillLevelupEffects.Create(m_head.position, m_head.rotation, m_head);
    }

    public static bool RemoveCredits(int s)
    {
      if (Credits - s < 0)
      {
        return false;
      }

      Credits -= s;
      return true;
    }

    public static void AddCredits(int val)
    {
      Credits += val;
    }

    public static void AddCredits(int val, bool _notice)
    {
      AddCredits(val);
      if (_notice)
      {
        string n = String.Format("Odin Credits added : <color=lightblue><b>[{0}]</b></color>", Credits);
        MessageHud.instance.ShowBiomeFoundMsg(n, true); //trans
      }
    }

    #endregion

    private void AddCommands()
    {
      ConsoleCommandManager.Instance.AddConsoleCommand(new ForceLoadCommand());
    }

    private void RegisterObjects()
    {
      Log.Trace(Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
    }
  }
}
