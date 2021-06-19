using OdinPlus.Common;
using OdinPlus.StatusEffects;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace OdinPlus.Managers
{
  internal class StatusEffectsManager : AbstractManager<StatusEffectsManager>
  {
    public readonly Dictionary<string, StatusEffect> StatusEffectsList = new Dictionary<string, StatusEffect>();
    public readonly Dictionary<string, StatusEffect> BuzzList = new Dictionary<string, StatusEffect>();
    //private readonly Dictionary<string, StatusEffect> ValList = new Dictionary<string, StatusEffect>();
    public readonly Dictionary<string, StatusEffectData> ValDataList = new Dictionary<string, StatusEffectData>();
    public readonly Dictionary<string, StatusEffectData> MonsterStatusEffectsList = new Dictionary<string, StatusEffectData>();

    protected override void OnInitialize()
    {
      base.OnInitialize();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      SetupValStatusEffects();
      SetupMonsterStatusEffects();
    }

    protected override void OnPostInitialize()
    {
      base.OnPostInitialize();
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      InitBuzzStatusEffects();
      InitTrollStatusEffects();
      InitWolfStatusEffects();
      InitValStatusEffects();
      InitMonsterStatusEffects();
    }

    public override bool HasDependencyError()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !OdinMeadsManager.Instance.IsInitialized
                            || OdinMeadsManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal($"OdinMeadsManager.Instance.IsInitialized: {OdinMeadsManager.Instance.IsInitialized}");
        Log.Fatal($"OdinMeadsManager.Instance.HasDependencyError: {OdinMeadsManager.Instance.HasDependencyError()}");
      }

      return dependencyError;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        if (StatusEffectsList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: StatusEffectsList.Count: {StatusEffectsList.Count}";
        }

        if (BuzzList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: BuzzList.Count: {BuzzList.Count}";
        }

        //if (ValList.Count == 0)
        //{
        //  healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //  healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: ValList.Count: {ValList.Count}";
        //}

        if (ValDataList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: ValDataList.Count: {ValDataList.Count}";
        }

        if (MonsterStatusEffectsList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: MonsterStatusEffectsList.Count: {MonsterStatusEffectsList.Count}";
        }

        return healthCheckStatus;
      }
      catch (Exception e)
      {
        Log.Error(e);
        healthCheckStatus.HealthStatus = HealthStatus.Failed;
        healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: {e.Message}";
        return healthCheckStatus;
      }
    }

    public void Register(ObjectDB objectDB)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (StatusEffectsList.Count == 0)
        {
          return;
        }

        foreach (var se in StatusEffectsList.Values)
        {
          objectDB.m_StatusEffects.Add(se);
        }

        Log.Debug("Register Status Effects");
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(objectDB), JsonSerializationProvider.ToJson(objectDB));
        throw;
      }
    }

    private void InitTrollStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var se = ScriptableObject.CreateInstance<SummonPetStatusEffect>();
      se.name = OdinPlusStatusEffect.SE_Troll;
      se.m_icon = Util.LoadResouceIcon(OdinPlusStatusEffect.SE_Troll);
      se.m_name = "$op_ScrollTroll_name";
      se.m_tooltip = "$op_ScrollTroll_tooltip";
      se.m_cooldownIcon = true;
      se.m_ttl = 600;
      se.PetName = OdinPlusPet.TrollPet;
      StatusEffectsList.Add(OdinPlusItem.ScrollTroll, se);
    }

    private void InitWolfStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var se = ScriptableObject.CreateInstance<SummonPetStatusEffect>();
      se.name = OdinPlusStatusEffect.SE_Wolf;
      se.m_icon = Util.LoadResouceIcon(OdinPlusStatusEffect.SE_Wolf);
      se.m_name = "$op_ScrollWolf_name";
      se.m_tooltip = "$op_ScrollWolf_tooltip";
      se.m_cooldownIcon = true;
      se.m_ttl = 1800;
      se.PetName = OdinPlusPet.WolfPet;
      StatusEffectsList.Add(OdinPlusItem.ScrollWolf, se);
    }

    private void InitBuzzStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      CreateBuzzStatusEffects("SpeedMeadsL");
    }

    private void CreateBuzzStatusEffects(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var se = ScriptableObject.CreateInstance<BuzzStatusEffect>();
        se.name = name;
        se.m_icon = ResourceAssetManager.Instance.GetIconSprite(name);
        se.m_name = "$op_" + name + "_name";
        se.m_tooltip = "$op_" + name + "_tooltip";
        se.m_ttl = 300;
        se.SpeedModifier = 1.5f;
        StatusEffectsList.Add(name, se);
        BuzzList.Add(name, se);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    private void InitValStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var item in ValDataList)
      {
        CreateValStatusEffects(item.Key, item.Value);
      }
    }

    private void CreateValStatusEffects(string name, StatusEffectData data)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        var se = ScriptableObject.CreateInstance<SE_Stats>();
        se.name = name;

        if (ResourceAssetManager.Instance.GetIconSprite(name) != null)
        {
          se.m_icon = ResourceAssetManager.Instance.GetIconSprite(name);
        }

        se.m_name = "$op_" + name + "_name";
        se.m_tooltip = "$op_" + name + "_tooltip";

        se.m_ttl = data.m_ttl;
        se.m_tickInterval = data.m_tickInterval;
        se.m_healthPerTickMinHealthPercentage = data.m_healthPerTickMinHealthPercentage;
        se.m_healthPerTick = data.m_healthPerTick;
        se.m_healthOverTime = data.m_healthOverTime;
        se.m_healthOverTimeDuration = data.m_healthOverTimeDuration;
        se.m_healthOverTimeInterval = data.m_healthOverTimeInterval;
        se.m_staminaOverTimeDuration = data.m_staminaOverTimeDuration;
        se.m_staminaDrainPerSec = data.m_staminaDrainPerSec;
        se.m_runStaminaDrainModifier = data.m_runStaminaDrainModifier;
        se.m_jumpStaminaUseModifier = data.m_jumpStaminaUseModifier;
        se.m_healthRegenMultiplier = data.m_healthRegenMultiplier;
        se.m_staminaRegenMultiplier = data.m_staminaRegenMultiplier;
        se.m_raiseSkill = data.m_raiseSkill;
        se.m_raiseSkillModifier = data.m_raiseSkillModifier;
        //se.m_mods = new List<HitData.DamageModPair>() = data.m_mods = new List<HitData.DamageModPair>();
        se.m_modifyAttackSkill = data.m_modifyAttackSkill;
        se.m_damageModifier = data.m_damageModifier;
        se.m_noiseModifier = data.m_noiseModifier;
        se.m_stealthModifier = data.m_stealthModifier;
        se.m_addMaxCarryWeight = data.m_addMaxCarryWeight;

        StatusEffectsList.Add(name, se);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        e.Data.Add(nameof(data), JsonSerializationProvider.ToJson(data));
        throw;
      }
    }

    private void InitMonsterStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var item in MonsterStatusEffectsList)
      {
        CreateValStatusEffects(item.Key, item.Value);
      }
    }

    private void SetupMonsterStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      for (var i = 1; i < 6; i++)
      {
        MonsterStatusEffectsList.Add("MonsterAttackAMP" + i, new StatusEffectData {m_ttl = 0, m_modifyAttackSkill = Skills.SkillType.All, m_damageModifier = 1 + (i - 1) * 0.1f});
      }
    }

    private void SetupValStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      ValDataList.Add("ExpMeadS", new StatusEffectData {m_ttl = 300, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 50});
      ValDataList.Add("ExpMeadM", new StatusEffectData {m_ttl = 450, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 75});
      ValDataList.Add("ExpMeadL", new StatusEffectData {m_ttl = 600, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 125});
      ValDataList.Add("WeightMeadS", new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 100});
      ValDataList.Add("WeightMeadM", new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 150});
      ValDataList.Add("WeightMeadL", new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 300});
      ValDataList.Add("InvisibleMeadS", new StatusEffectData {m_ttl = 60, m_noiseModifier = -1, m_stealthModifier = -1});
      ValDataList.Add("InvisibleMeadM", new StatusEffectData {m_ttl = 90, m_noiseModifier = -1, m_stealthModifier = -1});
      ValDataList.Add("InvisibleMeadL", new StatusEffectData {m_ttl = 120, m_noiseModifier = -1, m_stealthModifier = -1});
      ValDataList.Add("PickaxeMeadS", new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      ValDataList.Add("PickaxeMeadM", new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      ValDataList.Add("PickaxeMeadL", new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      ValDataList.Add("BowsMeadS", new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      ValDataList.Add("BowsMeadM", new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      ValDataList.Add("BowsMeadL", new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      ValDataList.Add("SwordsMeadS", new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      ValDataList.Add("SwordsMeadM", new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      ValDataList.Add("SwordsMeadL", new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      ValDataList.Add("AxeMeadS", new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
      ValDataList.Add("AxeMeadM", new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
      ValDataList.Add("AxeMeadL", new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
    }
  }
}
