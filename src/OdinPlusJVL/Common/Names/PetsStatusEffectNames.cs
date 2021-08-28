using System.Collections.Generic;

namespace OdinPlusJVL.Common.Names
{
  public static class PetsStatusEffectNames
  {
    public static string Troll = "SE_Troll";
    public static string Wolf  = "SE_Wolf";
    public static readonly IEnumerable<string> AllNames = Digitalroot.Valheim.Common.Utils.AllNames(typeof(PetsStatusEffectNames));
  }
}
