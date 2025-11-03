using DataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SkillInfo
{
    public string skillName;
    public TargetType targetType;
    public List<int> enablePos;
    public List<int> targetPos;
    public string iconPathString;
    public SkillEffect[] skillEffects;
    public SkillEffectType skillEffectType;
    private SkillData sd;
    public string effectPath;
    public string description;

    public SkillInfo(int id)
    {
        sd = DataManager.Instance.Skill.GetSkillData(id);

        this.skillName = sd.skillName;
        this.targetType = sd.targetType;
        this.enablePos = sd.enablePos;
        this.targetPos = sd.targetPos;
        this.iconPathString = sd.iconPathString;
        this.skillEffectType = sd.skillEffectType;
        this.effectPath = sd.effectPathString;
        this.description = sd.skillDescription;
        skillEffects = new SkillEffect[sd.effectId.Count];
        for (int i = 0; i < sd.effectId.Count; i++)
        {
            var datas = DataManager.Instance.Effect.GetEffectData(sd.effectId[i]);
            switch (datas.effectType)
            {
                case EffectType.Attack:
                    skillEffects[i] = new DamageSkill();
                    skillEffects[i].Init(datas);
                    break;
                case EffectType.Heal:
                    skillEffects[i] = new HealSkill();
                    skillEffects[i].Init(datas);
                    break;
                case EffectType.Buff:
                case EffectType.Debuff:
                case EffectType.Mark:
                    skillEffects[i] = new BuffSkill();
                    skillEffects[i].Init(datas);
                    break;
                case EffectType.Protect:
                    break;
                case EffectType.SwapPosition:
                    skillEffects[i] = new SwapSkill();
                    skillEffects[i].Init(datas);
                    break;
            }
        }
    }
}

public class Skill
{
    public SkillInfo skillInfo { get; private set; }
    public const float defaultWeight = 0.25f;
    public float addedWeight;
    public BaseEntity baseEntity;
    public void Init(int id, BaseEntity entity)
    {
        skillInfo = new SkillInfo(id);
        baseEntity = entity;
        Setting();
    }

    public bool IsAblePosition()
    {
        for (int i = 0; i < skillInfo.enablePos.Count; i++)
        {
            if (BattleManager.Instance.FindEntityPosition(baseEntity) == skillInfo.enablePos[i])
            {
                return true;
            }
        }
        return false;
    }

    public void Init(int id)
    {
        skillInfo = new SkillInfo(id);
        Setting();
    }

    private void Setting()
    {

    }

    public bool IsAbleUseSkill(BaseEntity target)
    {
        bool isEnablePos = false;
        if (baseEntity == null)
        {
            return true;
        }
        foreach (var item in skillInfo.enablePos)
        {
            if (item == BattleManager.Instance.PlayableCharacters.IndexOf(baseEntity))
            {
                isEnablePos = true;
                break;
            }
        }

        if (isEnablePos == false)
        {
            return false;
        }
        // 여기서 내가 사용가능한 위치에 있는지 확인
        foreach (var item in skillInfo.targetPos)
        {
            if (BattleManager.Instance.EnemyCharacters.IndexOf(baseEntity) == item)
            {
                return true;
            }
        }// 여기서 적이 사용가능한 위치에있는지확인

        return false;
    }

    public void UseSkill(BaseEntity targetEntity)
    {
        if (skillInfo.targetType == TargetType.Range)
        {
            List<BaseEntity> list = new List<BaseEntity>();
            var val = BattleManager.Instance.GetPossibleSkillRange(skillInfo.targetPos, targetEntity);
            for (int i = 0; i < val.Count; i++)
            {
                if (targetEntity is PlayableCharacter)
                {
                    if (BattleManager.Instance._playableCharacters == null)
                    {
                        if (BattleManager.Instance._playableCharacters.Count == 0)
                            list.Add(BattleManager.Instance._playableCharacters[val[i]]);
                    }
                    else
                    {
                        list.Add(GameManager.Instance.playableCharacter[val[i]]);
                    }
                }
                else
                {
                    list.Add(BattleManager.Instance._enemyCharacters[val[i]]);
                }
            }
            if (baseEntity != null)
            {
                var gameDataSkillExp = GameManager.Instance.gameDatas.GetGameData(GameManager.Instance.CurrentMapIndex).skillUseExp;
                if (targetEntity is PlayableCharacter && baseEntity.entityInfo.equips[0] != null)
                {
                    this.baseEntity.entityInfo.equips[0].AddWeaponExp(gameDataSkillExp);
                }
                baseEntity.UseSkill(() => UseActiveSkill(targetEntity), list, this);
            }
            else
            {
                UseActiveSkill(targetEntity);
            }
            return;
        }
        if (baseEntity != null && GetSkillType() != EffectType.SwapPosition)
        {
            var gameDataSkillExp = GameManager.Instance.gameDatas.GetGameData(GameManager.Instance.CurrentMapIndex).skillUseExp;
            if (targetEntity is PlayableCharacter && baseEntity.entityInfo.equips[0] != null)
            {
                this.baseEntity.entityInfo.equips[0].AddWeaponExp(gameDataSkillExp);
            }
            baseEntity.UseSkill(() => UseActiveSkill(targetEntity), targetEntity, this);
        }
        else
        {
            UseActiveSkill(targetEntity);
        }
    }

    private void UseActiveSkill(BaseEntity targetEntity)
    {
        var val = BattleManager.Instance.GetPossibleSkillRange(skillInfo.targetPos, targetEntity);
        List<BaseEntity> playableCharacters = BattleManager.Instance.PlayableCharacters;
        List<BaseEntity> baseEntities = new List<BaseEntity>();
        if (skillInfo.targetType == TargetType.Range)
        {
            for (int j = 0; j < val.Count; j++)
            {
                if (targetEntity is PlayableCharacter && baseEntity != null && baseEntity is Enemy)
                {
                    if (IsEvade(playableCharacters[val[j]]))
                        continue;
                }

                if (targetEntity is Enemy && baseEntity != null && baseEntity is PlayableCharacter)
                {
                    if (IsEvade(BattleManager.Instance._enemyCharacters[val[j]]))
                        continue;
                }
                
                for (int i = 0; i < skillInfo.skillEffects.Length; i++)
                {
                    if (targetEntity is PlayableCharacter)
                    {
                        if (baseEntity != null)
                            skillInfo.skillEffects[i].ActiveEffect(baseEntity, playableCharacters[val[j]]);
                        else
                            skillInfo.skillEffects[i].ActiveEffect(playableCharacters[val[j]]);
                    }
                    else
                    {
                        if (baseEntity != null)
                            skillInfo.skillEffects[i].ActiveEffect(baseEntity, BattleManager.Instance._enemyCharacters[val[j]]);
                        else
                            skillInfo.skillEffects[i].ActiveEffect(BattleManager.Instance._enemyCharacters[val[j]]);
                    }
                }
            }
        }
        else
        {
            if (baseEntity != null)
            {
                if (targetEntity is PlayableCharacter && baseEntity != null && baseEntity is Enemy)
                {
                    if (IsEvade(targetEntity))
                        return;
                }
                else if (targetEntity is Enemy && baseEntity != null && baseEntity is PlayableCharacter)
                {
                    if (IsEvade(targetEntity))
                    {
                        return;
                    }
                }
            }
            for (int i = 0; i < skillInfo.skillEffects.Length; i++)
            {
                if (baseEntity != null)
                    skillInfo.skillEffects[i].ActiveEffect(baseEntity, targetEntity);
                else
                    skillInfo.skillEffects[i].ActiveEffect(targetEntity);
            }
        }
    }

    private bool IsEvade(BaseEntity damagedEntity)
    {
        int temp = (int)(damagedEntity.entityInfo.evasion * 100f);

        if (UnityEngine.Random.Range(0, 100) <= temp)
        {
            damagedEntity.evasionText.OnEvasion();
            return true;
        }

        return false;
    }
    public EffectType GetSkillType()
    {
        return skillInfo.skillEffects[0].GetEffectType();
    }
}