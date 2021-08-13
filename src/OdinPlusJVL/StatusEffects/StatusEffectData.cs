using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace OdinPlusJVL.StatusEffects
{
  [SuppressMessage("ReSharper", "InconsistentNaming")]
  internal class StatusEffectData
  {
    public float m_ttl;
    public float m_tickInterval;
    public float m_healthPerTickMinHealthPercentage;
    public float m_healthPerTick;
    public float m_healthOverTime;
    public float m_healthOverTimeDuration;
    public float m_healthOverTimeInterval = 5f;
    public float m_staminaOverTimeDuration;
    public float m_staminaDrainPerSec;
    public float m_runStaminaDrainModifier;
    public float m_jumpStaminaUseModifier;
    public float m_healthRegenMultiplier = 1f;
    public float m_staminaRegenMultiplier = 1f;
    public Skills.SkillType m_raiseSkill;
    public float m_raiseSkillModifier;
    public List<HitData.DamageModPair> m_mods = new List<HitData.DamageModPair>();
    public Skills.SkillType m_modifyAttackSkill;
    public float m_damageModifier = 1f;
    public float m_noiseModifier;
    public float m_stealthModifier;
    public float m_addMaxCarryWeight;
  }
}
