using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common.Names
{
  public static class CustomPrefabNames
  {
    public static readonly string OdinsCamp = nameof(OdinsCamp);
    public static readonly string OrnamentalCauldron = "oc_OrnamentalCauldron";
    public static readonly string OrnamentalFirePit = "oc_OrnamentalFirePit";
    public static readonly string OrnamentalGoblinShaman = "oc_OrnamentalGoblinShaman";
    public static readonly string OrnamentalKeeper = "oc_OrnamentalKeeper";
    public static readonly string OrnamentalMunin = "oc_OrnamentalMunin";
    public static readonly string OrnamentalOdin = "oc_OrnamentalOdin";

    public static readonly IEnumerable<string> AllNames = Utils.Utils.AllNames(typeof(PetNames));
  }
}
