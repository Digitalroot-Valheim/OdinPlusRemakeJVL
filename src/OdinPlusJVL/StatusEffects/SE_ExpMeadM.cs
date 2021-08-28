using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_ExpMeadM : SE_AbstractExpMead
  {
    public SE_ExpMeadM()
      : base(MeadNames.ExpMeadM)
    {
    }

    #region Overrides of SE_AbstractExpMead

    /// <inheritdoc />
    protected override float GetRaiseSkillModifier() => 75f;

    /// <inheritdoc />
    protected override float GetTTL() => 450f;

    #endregion
  }
}
