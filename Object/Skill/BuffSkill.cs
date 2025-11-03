using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSkill : SkillEffect
{
    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        BuffInfo statEffectInfo = new BuffInfo();
        statEffectInfo.Init(adRatio, duration, constantValue, actionEntity, buffType, debuffType);
        targetEntity.AddEffect(statEffectInfo);
    }

    public override void ActiveEffect(BaseEntity targetEntity)
    {
        BuffInfo statEffectInfo = new BuffInfo();
        statEffectInfo.Init(adRatio, duration, constantValue, null, buffType, debuffType);
        targetEntity.AddEffect(statEffectInfo);
        BattleManager.Instance.OnceUseItem();
    }
}
