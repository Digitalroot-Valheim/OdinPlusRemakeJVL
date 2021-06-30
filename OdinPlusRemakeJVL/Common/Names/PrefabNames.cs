using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common.Names
{
  public static class PrefabNames
  {
    public static string ForceField = nameof(ForceField);
    public static string Cauldron = "piece_cauldron";
    public static string FirePit = "fire_pit";
    public static string Odin = "odin";
    public static string OdinCredit = nameof(OdinCredit);
    public static string Munin = nameof(Munin);
    public static string GoblinShaman = nameof(GoblinShaman);

    public static readonly IEnumerable<string> AllNames = Utils.Utils.AllNames(typeof(PrefabNames));

  }
}
