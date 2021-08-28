using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_ExpMeadL : SE_AbstractExpMead
  {
    public SE_ExpMeadL()
      : base(MeadNames.ExpMeadL)
    {
    }

    #region Overrides of SE_AbstractExpMead

    /// <inheritdoc />
    protected override float GetRaiseSkillModifier() => 125f;

    /// <inheritdoc />
    protected override float GetTTL() => 600f;

    #endregion
  }
}
