using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common.Names
{
  public static class HunterMonsterNames
  {
    public static readonly string DraugrElite = "Draugr_Elite";
    public static readonly string Fenring = nameof(Fenring);
    public static readonly string GoblinBrute = nameof(GoblinBrute);
    public static readonly string Troll = nameof(Troll);

    public static readonly IEnumerable<string> AllNames = Utils.Utils.AllNames(typeof(HunterMonsterNames));
  }
}
