namespace OdinPlusRemakeJVL.StatusEffects
{
  internal class SpeedStatusEffect : StatusEffect
  {
    private float _originalSpeed;

    public float SpeedModifier = 2;

    public override void Setup(Character character)
    {
      base.Setup(character);
      _originalSpeed = character.m_runSpeed;
      character.m_runSpeed *= SpeedModifier;
    }

    public override void OnDestroy()
    {
      m_character.m_runSpeed = _originalSpeed;
    }

    public override void Stop()
    {
      m_character.m_runSpeed = _originalSpeed;
      base.Stop();
    }
  }
}
