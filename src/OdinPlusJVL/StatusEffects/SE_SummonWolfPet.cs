using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once UnusedType.Global
  internal class SE_SummonWolfPet : StatusEffect
  {
    public string PetName;

    public SE_SummonWolfPet()
    {
      name = PetsStatusEffectNames.Wolf;
      m_icon = SpriteManager.Instance.GetSprite(PetsStatusEffectNames.Wolf);
      m_name = $"$op_{ItemDropNames.ScrollWolf}_name";
      m_tooltip = $"$op_{ItemDropNames.ScrollWolf}_tooltip";
      m_cooldownIcon = true;
      m_ttl = 1800;
      PetName = PetNames.WolfPet;
    }

    public override void Setup(Character character)
    {
      base.Setup(character);
      
      // PetManager.SummonPet(PetName);
    }

    #region Overrides of StatusEffect

    /// <inheritdoc />
    public override void Stop()
    {
      base.Stop();
      //PetManager.UnSummonPet(PetName); 
    }

    #endregion
  }
}
