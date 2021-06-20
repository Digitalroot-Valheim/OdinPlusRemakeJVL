using OdinPlus.Common;
using OdinPlus.StatusEffects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OdinPlus.Managers
{
  internal class StatusEffectsManager : AbstractManager<StatusEffectsManager>
  {
    private readonly Dictionary<string, StatusEffect> _statusEffectsList = new Dictionary<string, StatusEffect>();
    private readonly Dictionary<string, StatusEffect> _buzzList = new Dictionary<string, StatusEffect>();
    //private readonly Dictionary<string, StatusEffect> ValList = new Dictionary<string, StatusEffect>();
    private readonly Dictionary<string, StatusEffectData> _valDataList = new Dictionary<string, StatusEffectData>();
    private readonly Dictionary<string, StatusEffectData> _monsterStatusEffectsList = new Dictionary<string, StatusEffectData>();

    public IEnumerable<string> BuzzListKeys => _buzzList.Keys;
    public IEnumerable<string> ValDataListKeys => _valDataList.Keys;

    protected override bool OnInitialize()
    {
      try
      {
        if (!base.OnInitialize()) return false;
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        SetupValStatusEffects();
        SetupMonsterStatusEffects();
        return true;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return false;
      }
    }

    protected override bool OnPostInitialize()
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        if (!base.OnPostInitialize()) return false;
        InitBuzzStatusEffects();
        InitTrollStatusEffects();
        InitWolfStatusEffects();
        InitValStatusEffects();
        InitMonsterStatusEffects();
        Register();
        return true;
      }
      catch (Exception e)
      {
        Log.Error(e);
        return false;
      }
    }

    public override bool HasDependencyError()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var dependencyError = !ResourceAssetManager.Instance.IsInitialized
                            || ResourceAssetManager.Instance.HasDependencyError();

      if (dependencyError)
      {
        Log.Fatal($"ResourceAssetManager.Instance.IsInitialized: {ResourceAssetManager.Instance.IsInitialized}");
        Log.Fatal($"ResourceAssetManager.Instance.HasDependencyError: {ResourceAssetManager.Instance.HasDependencyError()}");
      }

      return dependencyError;
    }

    protected override HealthCheckStatus OnHealthCheck(HealthCheckStatus healthCheckStatus)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
        healthCheckStatus.Name = MethodBase.GetCurrentMethod().DeclaringType?.Name;
        if (_statusEffectsList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _statusEffectsList.Count: {_statusEffectsList.Count}";
        }

        if (_buzzList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _buzzList.Count: {_buzzList.Count}";
        }

        //if (ValList.Count == 0)
        //{
        //  healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
        //  healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: ValList.Count: {ValList.Count}";
        //}

        if (_valDataList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _valDataList.Count: {_valDataList.Count}";
        }
        
        if (_monsterStatusEffectsList.Count == 0)
        {
          healthCheckStatus.HealthStatus = HealthStatus.Unhealthy;
          healthCheckStatus.Reason = $"[{healthCheckStatus.Name}]: _monsterStatusEffectsList.Count: {_monsterStatusEffectsList.Count}";
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

    private void Register()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      if (_statusEffectsList.Count == 0)
      {
        return;
      }

      foreach (var se in _statusEffectsList.Values)
      {
        ObjectDB.instance.m_StatusEffects.Add(se);
      }

      Log.Debug("Register Status Effects");
    }

    private void CreateBuzzStatusEffects(string name)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({name})");
        var se = ScriptableObject.CreateInstance<BuzzStatusEffect>();
        se.name = name;
        se.m_icon = ResourceAssetManager.Instance.GetIconSprite(name);
        se.m_name = "$op_" + name + "_name";
        se.m_tooltip = "$op_" + name + "_tooltip";
        se.m_ttl = 300;
        se.SpeedModifier = 1.5f;
        _statusEffectsList.Add(name, se);
        _buzzList.Add(name, se);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        throw;
      }
    }

    private void CreateValStatusEffects(string name, StatusEffectData data)
    {
      try
      {
        Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({name})");
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

        _statusEffectsList.Add(name, se);
      }
      catch (Exception e)
      {
        e.Data.Add(nameof(name), name);
        e.Data.Add(nameof(data), JsonSerializationProvider.ToJson(data));
        throw;
      }
    }

    public KeyValuePair<string, StatusEffectData> GetMonsterStatusEffectsListByIndex(int i)
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}({i})");
      if (i < 0 || i <= _monsterStatusEffectsList.Count)
      {
        var msg = $"IndexOutOfRange - The value {i} is invalid. The value must be between 0-{_monsterStatusEffectsList.Count - 1}.";
        Log.Error(msg);
        throw new IndexOutOfRangeException(msg);
      }
      
      return _monsterStatusEffectsList.ElementAt(i);
    }

    public StatusEffect GetStatusEffects(string statusEffectsName)
    {
      return !_statusEffectsList.ContainsKey(statusEffectsName) ? null : _statusEffectsList[statusEffectsName];
    }

    private void InitBuzzStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      CreateBuzzStatusEffects("SpeedMeadsL");
    }

    private void InitMonsterStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var item in _monsterStatusEffectsList)
      {
        CreateValStatusEffects(item.Key, item.Value);
      }
    }

    private void InitTrollStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var se = ScriptableObject.CreateInstance<SummonPetStatusEffect>();
      se.name = OdinPlusStatusEffect.SE_Troll;
      se.m_icon = Util.LoadResourceIcon(OdinPlusStatusEffect.SE_Troll);
      se.m_name = "$op_ScrollTroll_name";
      se.m_tooltip = "$op_ScrollTroll_tooltip";
      se.m_cooldownIcon = true;
      se.m_ttl = 600;
      se.PetName = OdinPlusPet.TrollPet;
      _statusEffectsList.Add(OdinPlusItem.ScrollTroll, se);
    }

    private void InitWolfStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      var se = ScriptableObject.CreateInstance<SummonPetStatusEffect>();
      se.name = OdinPlusStatusEffect.SE_Wolf;
      se.m_icon = Util.LoadResourceIcon(OdinPlusStatusEffect.SE_Wolf);
      se.m_name = "$op_ScrollWolf_name";
      se.m_tooltip = "$op_ScrollWolf_tooltip";
      se.m_cooldownIcon = true;
      se.m_ttl = 1800;
      se.PetName = OdinPlusPet.WolfPet;
      _statusEffectsList.Add(OdinPlusItem.ScrollWolf, se);
    }

    private void InitValStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      foreach (var item in _valDataList)
      {
        CreateValStatusEffects(item.Key, item.Value);
      }
    }

    private void SetupMonsterStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      for (var i = 1; i <= 5; i++)
      {
        _monsterStatusEffectsList.Add($"MonsterAttackAMP{i}", new StatusEffectData {m_ttl = 0, m_modifyAttackSkill = Skills.SkillType.All, m_damageModifier = 1 + (i - 1) * 0.1f});
      }
    }

    private void SetupValStatusEffects()
    {
      Log.Trace($"{GetType().Namespace}.{GetType().Name}.{MethodBase.GetCurrentMethod().Name}()");
      _valDataList.Add("ExpMeadS", new StatusEffectData {m_ttl = 300, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 50});
      _valDataList.Add("ExpMeadM", new StatusEffectData {m_ttl = 450, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 75});
      _valDataList.Add("ExpMeadL", new StatusEffectData {m_ttl = 600, m_raiseSkill = Skills.SkillType.All, m_raiseSkillModifier = 125});
      _valDataList.Add("WeightMeadS", new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 100});
      _valDataList.Add("WeightMeadM", new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 150});
      _valDataList.Add("WeightMeadL", new StatusEffectData {m_ttl = 300, m_addMaxCarryWeight = 300});
      _valDataList.Add("InvisibleMeadS", new StatusEffectData {m_ttl = 60, m_noiseModifier = -1, m_stealthModifier = -1});
      _valDataList.Add("InvisibleMeadM", new StatusEffectData {m_ttl = 90, m_noiseModifier = -1, m_stealthModifier = -1});
      _valDataList.Add("InvisibleMeadL", new StatusEffectData {m_ttl = 120, m_noiseModifier = -1, m_stealthModifier = -1});
      _valDataList.Add("PickaxeMeadS", new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      _valDataList.Add("PickaxeMeadM", new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      _valDataList.Add("PickaxeMeadL", new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Pickaxes, m_damageModifier = 2f});
      _valDataList.Add("BowsMeadS", new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      _valDataList.Add("BowsMeadM", new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      _valDataList.Add("BowsMeadL", new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Bows, m_damageModifier = 2f});
      _valDataList.Add("SwordsMeadS", new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      _valDataList.Add("SwordsMeadM", new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      _valDataList.Add("SwordsMeadL", new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Swords, m_damageModifier = 2f});
      _valDataList.Add("AxeMeadS", new StatusEffectData {m_ttl = 60, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
      _valDataList.Add("AxeMeadM", new StatusEffectData {m_ttl = 150, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
      _valDataList.Add("AxeMeadL", new StatusEffectData {m_ttl = 300, m_modifyAttackSkill = Skills.SkillType.Axes, m_damageModifier = 2f});
    }

    public void UnRegister()
    {
      foreach (var item in _statusEffectsList.Values)
      {
        ObjectDB.instance.m_StatusEffects.Remove(item);
      }
    }
  }
}
