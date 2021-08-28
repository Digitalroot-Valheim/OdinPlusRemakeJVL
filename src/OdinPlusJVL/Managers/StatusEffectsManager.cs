using Digitalroot.Valheim.Common;
using Digitalroot.Valheim.Common.Json;
using Jotunn.Entities;
using OdinPlusJVL.Common.Names;
using OdinPlusJVL.StatusEffects;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace OdinPlusJVL.Managers
{
  internal class StatusEffectsManager : AbstractManager<StatusEffectsManager>
  {
    protected override bool OnInitialize()
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnInitialize()) return false;
        AddStatusEffects();
        return true;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        return false;
      }
    }

    public override bool HasDependencyError()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !SpriteManager.Instance.IsInitialized
                            || SpriteManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal(Main.Instance, $"SpriteManager.Instance.IsInitialized: {SpriteManager.Instance.IsInitialized}");
        Log.Fatal(Main.Instance, $"SpriteManager.Instance.HasDependencyError: {SpriteManager.Instance.HasDependencyError()}");
      }

      return dependencyError;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = GetType().Name;
        // if (GetMasterStatusEffectData().Any())
        // {
        //   healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //   healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: GetMasterStatusEffectData().Any(): false";
        // }

        return healthCheckStatus;
      }
      catch (Exception e)
      {
        Log.Error(Main.Instance, e);
        healthCheckStatus.HealthStatus = HealthStatus.Failed;
        healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: {e.Message}";
        return healthCheckStatus;
      }
    }

    private void AddStatusEffects()
    {
      Log.Trace(Main.Instance, $"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (SE_Stats statusEffect in CreateStatusEffects(GetMasterStatusEffectData()))
      {
        Log.Trace(Main.Instance, $"[{GetType().Name}] Adding {statusEffect.name}");
        try
        {
          Jotunn.Managers.ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(statusEffect, fixReference: false));
        }
        catch (Exception e)
        {
          e.Data.Add(nameof(statusEffect), JsonSerializationProvider.ToJson(statusEffect));
          throw;
        }
      }

      Log.Trace(Main.Instance, $"[{GetType().Name}] Adding {GetSpeedStatusEffect().name}");
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(GetSpeedStatusEffect(), fixReference: false));
      Log.Trace(Main.Instance, $"[{GetType().Name}] Adding {GetSummonTrollPetStatusEffect().name}");
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(GetSummonTrollPetStatusEffect(), fixReference: false));
      Log.Trace(Main.Instance, $"[{GetType().Name}] Adding {GetSummonWolfPetStatusEffect().name}");
      Jotunn.Managers.ItemManager.Instance.AddStatusEffect(new CustomStatusEffect(GetSummonWolfPetStatusEffect(), fixReference: false));
    }

    private IEnumerable<KeyValuePair<string, StatusEffectData>> GetMasterStatusEffectData()
    {
      foreach (var keyValuePair in GetMeadStatusEffectData())
      {
        yield return keyValuePair;
      }

      foreach (var keyValuePair in GetMonsterStatusEffectData())
      {
        yield return keyValuePair;
      }
    }

    private IEnumerable<KeyValuePair<string, StatusEffectData>> GetMeadStatusEffectData()
    {
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.ExpMeadS, new StatusEffectData {m_ttl = 300, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 50});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.ExpMeadM, new StatusEffectData {m_ttl = 450, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 75});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.ExpMeadL, new StatusEffectData {m_ttl = 600, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 125});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.WeightMeadS, new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 100});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.WeightMeadM, new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 150});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.WeightMeadL, new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 300});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.InvisibleMeadS, new StatusEffectData {m_ttl = 60, m_noiseModifier = -1, m_stealthModifier = -1});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.InvisibleMeadM, new StatusEffectData {m_ttl = 90, m_noiseModifier = -1, m_stealthModifier = -1});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.InvisibleMeadL, new StatusEffectData {m_ttl = 120, m_noiseModifier = -1, m_stealthModifier = -1});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.PickaxeMeadS, new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.PickaxeMeadM, new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.PickaxeMeadL, new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.BowsMeadS, new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.BowsMeadM, new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.BowsMeadL, new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.SwordsMeadS, new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.SwordsMeadM, new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.SwordsMeadL, new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.AxeMeadS, new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.AxeMeadM, new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
      yield return new KeyValuePair<string, StatusEffectData>(MeadNames.AxeMeadL, new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
    }

    private IEnumerable<KeyValuePair<string, StatusEffectData>> GetMonsterStatusEffectData()
    {
      for (var i = 1; i <= 5; i++)
      {
        yield return new KeyValuePair<string, StatusEffectData>($"MonsterAttackAMP{i}", new StatusEffectData {m_ttl = 0, m_modifyAttackSkill = Skills.SkillType.All, m_damageModifier = 1 + (i - 1) * 0.1f});
      }
    }

    private IEnumerable<SE_Stats> CreateStatusEffects(IEnumerable<KeyValuePair<string, StatusEffectData>> keyValuePairs)
    {
      foreach (var keyValuePair in keyValuePairs)
      {
        var effect = ScriptableObject.CreateInstance<SE_Stats>();
        try
        {
          effect.name = $"{keyValuePair.Key}StatusEffect";
          effect.m_name = $"op_{keyValuePair.Key}_name";
          effect.m_tooltip = $"$op_{keyValuePair.Key}_tooltip";
          effect.m_icon = SpriteManager.Instance.GetSprite(keyValuePair.Key) ?? effect.m_icon;
          effect.m_ttl = 300;
          effect.m_tickInterval = keyValuePair.Value.m_tickInterval;
          effect.m_healthPerTickMinHealthPercentage = keyValuePair.Value.m_healthPerTickMinHealthPercentage;
          effect.m_healthPerTick = keyValuePair.Value.m_healthPerTick;
          effect.m_healthOverTime = keyValuePair.Value.m_healthOverTime;
          effect.m_healthOverTimeDuration = keyValuePair.Value.m_healthOverTimeDuration;
          effect.m_healthOverTimeInterval = keyValuePair.Value.m_healthOverTimeInterval;
          effect.m_staminaOverTimeDuration = keyValuePair.Value.m_staminaOverTimeDuration;
          effect.m_staminaDrainPerSec = keyValuePair.Value.m_staminaDrainPerSec;
          effect.m_runStaminaDrainModifier = keyValuePair.Value.m_runStaminaDrainModifier;
          effect.m_jumpStaminaUseModifier = keyValuePair.Value.m_jumpStaminaUseModifier;
          effect.m_healthRegenMultiplier = keyValuePair.Value.m_healthRegenMultiplier;
          effect.m_staminaRegenMultiplier = keyValuePair.Value.m_staminaRegenMultiplier;
          effect.m_raiseSkill = keyValuePair.Value.m_raiseSkill;
          effect.m_raiseSkillModifier = keyValuePair.Value.m_raiseSkillModifier;
          effect.m_modifyAttackSkill = keyValuePair.Value.m_modifyAttackSkill;
          effect.m_damageModifier = keyValuePair.Value.m_damageModifier;
          effect.m_noiseModifier = keyValuePair.Value.m_noiseModifier;
          effect.m_stealthModifier = keyValuePair.Value.m_stealthModifier;
          effect.m_addMaxCarryWeight = keyValuePair.Value.m_addMaxCarryWeight;
        }
        catch (Exception e)
        {
          e.Data.Add(nameof(keyValuePair.Key), keyValuePair.Key);
          e.Data.Add(nameof(keyValuePair.Value), JsonSerializationProvider.ToJson(keyValuePair.Value));
          throw;
        }

        yield return effect;
      }
    }

    private SE_RunSpeed GetSpeedStatusEffect() => ScriptableObject.CreateInstance<SE_RunSpeed>();

    private SE_SummonTrollPet GetSummonTrollPetStatusEffect() => ScriptableObject.CreateInstance<SE_SummonTrollPet>();

    private SE_SummonWolfPet GetSummonWolfPetStatusEffect() => ScriptableObject.CreateInstance<SE_SummonWolfPet>();
  }
}
