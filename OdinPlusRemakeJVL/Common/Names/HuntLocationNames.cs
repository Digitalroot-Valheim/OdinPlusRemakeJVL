using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common.Names
{
  public static class HuntLocationNames
  {
    public static readonly string Ruin1 = nameof(Ruin1);
    public static readonly string RunestoneDraugr = "Runestone_Draugr";
    public static readonly string RunestoneGreydwarfs = "Runestone_Greydwarfs";
    public static readonly string RunestonePlains = "Runestone_Plains";
    public static readonly string Waymarker02 = nameof(Waymarker02);

    public static readonly IEnumerable<string> AllNames = Utils.Utils.AllNames(typeof(HuntLocationNames));
  }
}
