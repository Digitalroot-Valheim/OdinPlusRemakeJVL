using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractInvisibleMead : SE_AbstractMead
  {
    protected SE_AbstractInvisibleMead(string name, float ttl)
      : base(name, ttl)
    {
      m_noiseModifier = -1;
      m_stealthModifier = -1;
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_InvisibleMeadS : SE_AbstractInvisibleMead
  {
    public SE_InvisibleMeadS()
      : base(MeadNames.InvisibleMeadS, 60)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_InvisibleMeadM : SE_AbstractInvisibleMead
  {
    public SE_InvisibleMeadM()
      : base(MeadNames.InvisibleMeadM, 90)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_InvisibleMeadL : SE_AbstractInvisibleMead
  {
    public SE_InvisibleMeadL()
      : base(MeadNames.InvisibleMeadL, 120)
    {
    }
  }
}
