using System.Collections.Generic;

namespace OdinPlusJVL.Common.Names
{
  public static class LocationNames
  {
    public static readonly string StartTemple = nameof(StartTemple);

    public static readonly IEnumerable<string> AllNames =Digitalroot.Valheim.Common.Utils.AllNames(typeof(HuntLocationNames));
  }
}
