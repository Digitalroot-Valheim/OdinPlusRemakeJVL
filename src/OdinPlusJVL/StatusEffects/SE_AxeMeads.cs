using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractAxeMead : SE_AbstractMead
  {
    protected SE_AbstractAxeMead(string name, float ttl)
      : base(name, ttl)
    {
      m_modifyAttackSkill = Skills.SkillType.Axes;
      m_damageModifier = 2f;
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_AxeMeadS : SE_AbstractAxeMead
  {
    public SE_AxeMeadS()
      : base(MeadNames.AxeMeadS, 60)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_AxeMeadM : SE_AbstractAxeMead
  {
    public SE_AxeMeadM()
      : base(MeadNames.AxeMeadM, 150)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_AxeMeadL : SE_AbstractAxeMead
  {
    public SE_AxeMeadL()
      : base(MeadNames.AxeMeadL, 300)
    {
    }
  }
}
