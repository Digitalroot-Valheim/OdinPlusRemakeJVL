using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractExpMead : SE_AbstractMead
  {
    protected SE_AbstractExpMead(string name, float ttl, float raiseSkillModifier)
      : base(name, ttl)
    {
      m_raiseSkillModifier = raiseSkillModifier;
      m_raiseSkill = Skills.SkillType.All;
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_ExpMeadS : SE_AbstractExpMead
  {
    public SE_ExpMeadS()
      : base(MeadNames.ExpMeadS, 300, 50)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_ExpMeadM : SE_AbstractExpMead
  {
    public SE_ExpMeadM()
      : base(MeadNames.ExpMeadM, 450, 75)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_ExpMeadL : SE_AbstractExpMead
  {
    public SE_ExpMeadL()
      : base(MeadNames.ExpMeadL, 600, 125)
    {
    }
  }
}
