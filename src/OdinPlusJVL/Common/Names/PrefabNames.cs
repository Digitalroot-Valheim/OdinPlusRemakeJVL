using System.Collections.Generic;

namespace OdinPlusJVL.Common.Names
{
  public static class PrefabNames
  {
    public static readonly string Cauldron = "piece_cauldron";
    public static readonly string FirePit = "fire_pit";
    public static readonly string ForceField = nameof(ForceField);
    public static readonly string GoblinShaman = nameof(GoblinShaman);
    public static readonly string Keeper = "keeper_prefab";
    public static readonly string Munin = nameof(Munin);
    public static readonly string Odin = "odin";
    public static readonly string OdinCredit = nameof(OdinCredit);

    public static readonly IEnumerable<string> AllNames = Digitalroot.Valheim.Common.Utils.AllNames(typeof(PrefabNames));
  }
}
