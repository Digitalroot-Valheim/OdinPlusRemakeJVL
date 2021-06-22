using System.Collections.Generic;

namespace OdinPlusRemakeJVL.Common
{
  public static class PetNames
  {
    public static readonly string WolfPet = nameof(WolfPet);
    public static readonly string TrollPet = nameof(TrollPet);

    public static readonly IEnumerable<string> AllNames = Utils.AllNames(typeof(PetNames));
  }
}
