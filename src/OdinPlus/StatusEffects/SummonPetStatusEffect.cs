using OdinPlus.Managers;

namespace OdinPlus.StatusEffects
{
  class SummonPetStatusEffect : StatusEffect
  {
    public string PetName;

    public override void Setup(Character character)
    {
      base.Setup(character);
      PetManager.SummonPet(PetName);
    }
  }
}
