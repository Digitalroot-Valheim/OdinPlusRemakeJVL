using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common.Names
{
  internal static class ConsoleCommandNames
  {
    internal static string HealthCheck = "healthcheck";
    internal static string ForceLoad = "forceload";
    internal static string GotoOdin = "gotoodin";
    internal static string OdinHere = "odinhere";
    internal static string SetOdin = "setodin";
    internal static string WhereOdin = "whereodin";
    internal static string WhereAmI = "whereami";
    internal static string WhereIsStartTemple = "whereisstart";
    public static readonly IEnumerable<string> AllNames = Utils.Utils.AllNames(typeof(ConsoleCommandNames));
  }
}
