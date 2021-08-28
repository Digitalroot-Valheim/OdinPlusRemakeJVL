using System.Collections.Generic;

namespace OdinPlusJVL.Common.Names
{
  public static class HunterMonsterNames
  {
    public static readonly string DraugrElite = "Draugr_Elite";
    public static readonly string Fenring = nameof(Fenring);
    public static readonly string GoblinBrute = nameof(GoblinBrute);
    public static readonly string Troll = nameof(Troll);

    public static readonly IEnumerable<string> AllNames = Digitalroot.Valheim.Common.Utils.AllNames(typeof(HunterMonsterNames));
  }
}
