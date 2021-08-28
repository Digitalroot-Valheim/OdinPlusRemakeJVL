namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractMonsterAttackAmp : SE_Stats
  {
    protected SE_AbstractMonsterAttackAmp(float damageModifier)
    {
      name = $"SE_MonsterAttackAmp{damageModifier}";
      m_ttl = 0;
      m_modifyAttackSkill = Skills.SkillType.All;
      m_damageModifier = 1 + (damageModifier - 1) * 0.1f;
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_MonsterAttackAmp1 : SE_AbstractMonsterAttackAmp
  {
    /// <inheritdoc />
    public SE_MonsterAttackAmp1() : base(1)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_MonsterAttackAmp2 : SE_AbstractMonsterAttackAmp
  {
    /// <inheritdoc />
    public SE_MonsterAttackAmp2() : base(2)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_MonsterAttackAmp3 : SE_AbstractMonsterAttackAmp
  {
    /// <inheritdoc />
    public SE_MonsterAttackAmp3() : base(3)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_MonsterAttackAmp4 : SE_AbstractMonsterAttackAmp
  {
    /// <inheritdoc />
    public SE_MonsterAttackAmp4() : base(4)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_MonsterAttackAmp5 : SE_AbstractMonsterAttackAmp
  {
    /// <inheritdoc />
    public SE_MonsterAttackAmp5() : base(5)
    {
    }
  }
}
