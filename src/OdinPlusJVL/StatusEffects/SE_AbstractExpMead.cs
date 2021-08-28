namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractExpMead : SE_AbstractMead
  {
    protected SE_AbstractExpMead(string name)
      : base(name)
    {
      m_ttl = GetTTL();
      m_raiseSkillModifier = GetRaiseSkillModifier();
      m_raiseSkill = Skills.SkillType.All;
    }

    protected abstract float GetRaiseSkillModifier();

    protected abstract float GetTTL();
  }
}
