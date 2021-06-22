using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common
{
  internal static class ConsoleCommandNames
  {
    internal static string HealthCheck = "healthcheck";
    internal static string ForceLoad = "forceload";
    public static readonly IEnumerable<string> AllNames = Utils.AllNames(typeof(ConsoleCommandNames));
  }
}
