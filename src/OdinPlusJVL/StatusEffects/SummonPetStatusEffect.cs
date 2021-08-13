using System;

namespace OdinPlusJVL.StatusEffects
{
  internal class SummonPetStatusEffect : StatusEffect
  {
    public string PetName;

    public override void Setup(Character character)
    {
      base.Setup(character);
      throw new NotImplementedException();
      // PetManager.SummonPet(PetName);
    }
  }
}
