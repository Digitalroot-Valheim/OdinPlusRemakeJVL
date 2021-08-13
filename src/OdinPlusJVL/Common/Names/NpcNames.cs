using System.Collections.Generic;

namespace OdinPlusJVL.Common.Names
{
  public static class NpcNames
  {
    public static readonly string Odin = "odin";
    public static readonly string Unknown = nameof(Unknown);

    public static readonly IEnumerable<string> AllNames = Digitalroot.Valheim.Common.Utils.AllNames(typeof(NpcNames));
  }
}
