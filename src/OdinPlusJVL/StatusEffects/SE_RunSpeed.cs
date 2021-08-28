using OdinPlusJVL.Common.Names;
using OdinPlusJVL.Managers;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class SE_RunSpeed : StatusEffect
  {
    public SE_RunSpeed()
    {
      name = $"{MeadNames.SpeedMeadsL}StatusEffect";
      m_name = $"op_{MeadNames.SpeedMeadsL}_name";
      m_tooltip = $"$op_{MeadNames.SpeedMeadsL}_tooltip";
      m_icon = SpriteManager.Instance.GetSprite(MeadNames.SpeedMeadsL);
      m_ttl = 300;
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
