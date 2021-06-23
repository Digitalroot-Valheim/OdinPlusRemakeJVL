using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common
{
  public static class DungeonLocationNames
  {
    public static readonly string Crypt2 = nameof(Crypt2);
    public static readonly string Crypt3 = nameof(Crypt3);
    public static readonly string Crypt4 = nameof(Crypt4);
    public static readonly string GoblinCamp2 = nameof(GoblinCamp2);
    public static readonly string SunkenCrypt4 = nameof(SunkenCrypt4);

    public static readonly IEnumerable<string> AllNames = Utils.AllNames(typeof(DungeonLocationNames));
  }
}
