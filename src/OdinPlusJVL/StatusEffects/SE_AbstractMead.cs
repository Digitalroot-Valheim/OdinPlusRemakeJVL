using OdinPlusJVL.Managers;

namespace OdinPlusJVL.StatusEffects
{
  // ReSharper disable once InconsistentNaming
  // ReSharper disable once ClassNeverInstantiated.Global
  internal abstract class SE_AbstractMead : SE_Stats
  {
    protected SE_AbstractMead(string meadName)
    {
      name = $"SE_{meadName}";
      m_name = $"$op_{meadName}_name";
      m_tooltip = $"$op_{meadName}_tooltip";
      m_icon = SpriteManager.Instance.GetSprite(meadName);
    }
  }
}
