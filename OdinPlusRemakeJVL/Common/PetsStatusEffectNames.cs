using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common
{
  public static class PetsStatusEffectNames
  {
    public static string Troll = "SE_Troll";
    public static string Wolf  = "SE_Wolf";
    public static readonly IEnumerable<string> AllNames = Utils.AllNames(typeof(PetsStatusEffectNames));
  }
}
