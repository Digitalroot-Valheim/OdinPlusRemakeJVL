using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once ClassNeverInstantiated.Global
  // ReSharper disable once InconsistentNaming
  internal class SE_SummonTrollPet : StatusEffect
  {
    public string PetName;

    public SE_SummonTrollPet()
    {
      name = PetsStatusEffectNames.Troll;
      m_icon = SpriteManager.Instance.GetSprite(PetsStatusEffectNames.Troll);
      m_name = $"$op_{ItemDropNames.ScrollTroll}_name";
      m_tooltip = $"$op_{ItemDropNames.ScrollTroll}_tooltip";
      m_cooldownIcon = true;
      m_ttl = 600;
      PetName = PetNames.TrollPet;
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

  // ReSharper disable once ClassNeverInstantiated.Global
  // ReSharper disable once InconsistentNaming
}
