using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common.Names
{
  public static class NpcNames
  {
    public static readonly string Odin = "odin";
    public static readonly string Unknown = nameof(Unknown);

    public static readonly IEnumerable<string> AllNames = Utils.Utils.AllNames(typeof(NpcNames));
  }
}
