using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common.Names
{
  public static class LocationNames
  {
    public static readonly string StartTemple = nameof(StartTemple);

    public static readonly IEnumerable<string> AllNames = Utils.Utils.AllNames(typeof(HuntLocationNames));
  }
}
