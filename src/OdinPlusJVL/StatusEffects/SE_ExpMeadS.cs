using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_ExpMeadS : SE_AbstractExpMead
  {
    public SE_ExpMeadS()
      : base(MeadNames.ExpMeadS)
    {
    }

    #region Overrides of SE_AbstractExpMead

    /// <inheritdoc />
    protected override float GetRaiseSkillModifier() => 50f;

    /// <inheritdoc />
    protected override float GetTTL() => 300f;

    #endregion
  }
}
