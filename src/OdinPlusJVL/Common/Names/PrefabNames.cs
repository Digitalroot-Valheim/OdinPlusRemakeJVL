using System.Collections.Generic;

namespace OdinPlusJVL.Common.Names
{
  public static class PrefabNames
  {
    public static readonly string Keeper = "keeper_prefab";
    public static readonly string FatTroll = "FatTroll";
    public static readonly string OdinCredit = nameof(OdinCredit);
    public static readonly string OdinsCamp = nameof(OdinsCamp);
    public static readonly string OdinsCampCauldron = $"oc_{nameof(OdinsCamp)}_Cauldron";
    public static readonly string OdinsCampFirePit = $"oc_{nameof(OdinsCamp)}_FirePit";
    public static readonly string OdinsCampGoblinShaman = $"oc_{nameof(OdinsCamp)}_GoblinShaman";
    public static readonly string OdinsCampEmissary = $"oc_{nameof(OdinsCamp)}_Keeper";
    public static readonly string OdinsCampMunin = $"oc_{nameof(OdinsCamp)}_Munin";
    public static readonly string OdinsCampOdin = $"oc_{nameof(OdinsCamp)}_Odin";
    public static readonly string OrnamentalCauldron = "oc_OrnamentalCauldron";
    public static readonly string OrnamentalFirePit = "oc_OrnamentalFirePit";
    public static readonly string OrnamentalGoblinShaman = "oc_OrnamentalGoblinShaman";
    public static readonly string OrnamentalEmissary = "oc_OrnamentalKeeper";
    public static readonly string OrnamentalFatTroll = "oc_OrnamentalFatTroll";
    public static readonly string OrnamentalMunin = "oc_OrnamentalMunin";
    public static readonly string OrnamentalOdin = "oc_OrnamentalOdin";

    public static readonly IEnumerable<string> AllNames = Digitalroot.Valheim.Common.Utils.AllNames(typeof(PrefabNames));
  }
}
