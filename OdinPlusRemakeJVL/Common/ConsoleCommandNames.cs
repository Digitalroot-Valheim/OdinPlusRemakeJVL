using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common
{
  internal static class ConsoleCommandNames
  {
    internal static string HealthCheck = "healthcheck";
    public static readonly IEnumerable<string> AllNames = Utils.AllNames(typeof(ConsoleCommandNames));
  }
}
