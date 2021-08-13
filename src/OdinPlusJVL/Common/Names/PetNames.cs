using System.Collections.Generic;

namespace OdinPlusJVL.Common.Names
{
  public static class PetNames
  {
    public static readonly string WolfPet = nameof(WolfPet);
    public static readonly string TrollPet = nameof(TrollPet);

    public static readonly IEnumerable<string> AllNames = Digitalroot.Valheim.Common.Utils.AllNames(typeof(PetNames));
  }
}
