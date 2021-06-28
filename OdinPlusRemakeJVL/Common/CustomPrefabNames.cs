using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common
{
  public static class CustomPrefabNames
  {
    public static readonly string FirePit = "oc_fire";
    public static readonly string Cauldron = "oc_cauldron";
    public static readonly string Odin = "oc_odin";
    public static readonly string OdinsCamp = nameof(OdinsCamp);

    public static readonly IEnumerable<string> AllNames = Utils.AllNames(typeof(PetNames));
  }
}
