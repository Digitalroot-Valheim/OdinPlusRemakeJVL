using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common
{
  public static class OdinPlusFxNames
  {
    public static string RedSmoke = nameof(RedSmoke);
    public static string BlueSmoke = nameof(BlueSmoke);
    public static string YellowSmoke = nameof(YellowSmoke);
    public static string GreenSmoke = nameof(GreenSmoke);
    public static readonly IEnumerable<string> AllNames = Utils.AllNames(typeof(OdinPlusFxNames));
  }
}
