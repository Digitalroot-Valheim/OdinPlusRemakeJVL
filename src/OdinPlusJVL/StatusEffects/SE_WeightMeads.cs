using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractWeightMead : SE_AbstractMead
  {
    /// <inheritdoc />
    protected SE_AbstractWeightMead(string meadName, float addMaxCarryWeight) 
      : base(meadName, 300)
    {
      m_addMaxCarryWeight = addMaxCarryWeight;
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  class SE_WeightMeadS : SE_AbstractWeightMead
  {
    /// <inheritdoc />
    public SE_WeightMeadS() 
      : base(MeadNames.WeightMeadS, 100)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  class SE_WeightMeadM : SE_AbstractWeightMead
  {
    /// <inheritdoc />
    public SE_WeightMeadM()
      : base(MeadNames.WeightMeadM, 150)
    {
    }
  }

  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  class SE_WeightMeadL : SE_AbstractWeightMead
  {
    /// <inheritdoc />
    public SE_WeightMeadL()
      : base(MeadNames.WeightMeadL, 300)
    {
    }
  }
}
