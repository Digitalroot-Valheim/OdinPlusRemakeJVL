using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractBowsMead : SE_AbstractMead
  {
    protected SE_AbstractBowsMead(string name, float ttl)
      : base(name, ttl)
    {
      m_modifyAttackSkill = Skills.SkillType.Bows;
      m_damageModifier = 2f;
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_BowsMeadS : SE_AbstractBowsMead
  {
    public SE_BowsMeadS()
      : base(MeadNames.BowsMeadS, 60)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_BowsMeadM : SE_AbstractBowsMead
  {
    public SE_BowsMeadM()
      : base(MeadNames.BowsMeadM, 150)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_BowsMeadL : SE_AbstractBowsMead
  {
    public SE_BowsMeadL()
      : base(MeadNames.BowsMeadL, 300)
    {
    }
  }
}
