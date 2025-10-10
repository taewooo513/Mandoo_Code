using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System;

public class SkillInfo
{
    public string skillName;
    public TargetType targetType;
    public List<int> enablePos;
    public List<int> targetPos;
    public string iconPathString;
    public SkillEffect[] skillEffects;
    private SkillData sd;

    public SkillInfo(int id)
    {
        sd = DataManager.Instance.Skill.GetSkillData(id);

        this.skillName = sd.skillName;
        this.targetType = sd.targetType;
        this.enablePos = sd.enablePos;
        this.targetPos = sd.targetPos;
        this.iconPathString = sd.iconPathString;
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
            }
        }
    }
}

public class Skill
{
    public SkillInfo skillInfo { get; private set; }
    public const float defaultWeight = 0.25f;
    public float addedWeight;
    BaseEntity baseEntity;
    public void Init(int id, BaseEntity entity)
    {
        skillInfo = new SkillInfo(id);
        baseEntity = entity;
        Setting();
    }

    private void Setting()
    {

    }

    public bool IsAbleUseSkill(BaseEntity target)
    {
        bool isEnablePos = false;
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
                    list.Add(BattleManager.Instance._playableCharacters[val[i]]);
                }
                else
                {
                    list.Add(BattleManager.Instance._enemyCharacters[val[i]]);
                }
            }
            baseEntity.UseSkill(() => UseActiveSkill(targetEntity), list);
            return;
        }
        baseEntity.UseSkill(() => UseActiveSkill(targetEntity), targetEntity);
    }

    private void UseActiveSkill(BaseEntity targetEntity)
    {
        var val = BattleManager.Instance.GetPossibleSkillRange(skillInfo.targetPos, targetEntity);
        List<BaseEntity> baseEntities = new List<BaseEntity>();
        if (skillInfo.targetType == TargetType.Range)
        {
            for (int j = 0; j < val.Count; j++)
            {
                for (int i = 0; i < skillInfo.skillEffects.Length; i++)
                {
                    if (targetEntity is PlayableCharacter)
                    {
                        skillInfo.skillEffects[i].ActiveEffect(baseEntity, BattleManager.Instance._playableCharacters[val[j]]);
                    }
                    else
                    {
                        skillInfo.skillEffects[i].ActiveEffect(baseEntity, BattleManager.Instance._enemyCharacters[val[j]]);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < skillInfo.skillEffects.Length; i++)
            {
                skillInfo.skillEffects[i].ActiveEffect(baseEntity, targetEntity);
            }
        }
    }

    public EffectType GetSkillType()
    {
        EffectType skillType;

        for (int i = 0; i < skillInfo.skillEffects.Length; i++)
        {

        }
        return skillInfo.skillEffects[0].GetEffectType();
    }
}