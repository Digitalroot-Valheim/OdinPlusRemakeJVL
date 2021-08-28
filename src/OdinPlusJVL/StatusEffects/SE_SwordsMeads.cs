using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractSwordsMead : SE_AbstractMead
  {
    protected SE_AbstractSwordsMead(string name, float ttl)
      : base(name, ttl)
    {
      m_modifyAttackSkill = Skills.SkillType.Swords;
      m_damageModifier = 2f;
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_SwordsMeadS : SE_AbstractSwordsMead
  {
    public SE_SwordsMeadS()
      : base(MeadNames.SwordsMeadS, 60)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_SwordsMeadM : SE_AbstractSwordsMead
  {
    public SE_SwordsMeadM()
      : base(MeadNames.SwordsMeadM, 150)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_SwordsMeadL : SE_AbstractSwordsMead
  {
    public SE_SwordsMeadL()
      : base(MeadNames.SwordsMeadL, 300)
    {
    }
  }
}
