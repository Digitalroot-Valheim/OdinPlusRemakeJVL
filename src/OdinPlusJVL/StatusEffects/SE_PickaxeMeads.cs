using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractPickaxeMead : SE_AbstractMead
  {
    protected SE_AbstractPickaxeMead(string name, float ttl)
      : base(name, ttl)
    {
      m_modifyAttackSkill = Skills.SkillType.Pickaxes;
      m_damageModifier = 2f;
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_PickaxeMeadS : SE_AbstractPickaxeMead
  {
    public SE_PickaxeMeadS()
      : base(MeadNames.PickaxeMeadS, 60)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_PickaxeMeadM : SE_AbstractPickaxeMead
  {
    public SE_PickaxeMeadM()
      : base(MeadNames.PickaxeMeadM, 150)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_PickaxeMeadL : SE_AbstractPickaxeMead
  {
    public SE_PickaxeMeadL()
      : base(MeadNames.PickaxeMeadL, 300)
    {
    }
  }
}
