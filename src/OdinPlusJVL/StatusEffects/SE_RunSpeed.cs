using OdinPlusJVL.Common.Names;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_RunSpeed : SE_AbstractMead
  {
    public SE_RunSpeed()
      : base(MeadNames.SpeedMeadsL, 300)
    {
    }

    #region Overrides of StatusEffect

    /// <inheritdoc />
    public override void ModifySpeed(ref float speed)
    {
      speed *= 1.5f;
    }

    #endregion
  }
}
