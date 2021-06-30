using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common.Names
{
  public static class PetNames
  {
    public static readonly string WolfPet = nameof(WolfPet);
    public static readonly string TrollPet = nameof(TrollPet);

    public static readonly IEnumerable<string> AllNames = Utils.Utils.AllNames(typeof(PetNames));
  }
}
