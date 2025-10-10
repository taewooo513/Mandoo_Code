using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSkill : SkillEffect
{
    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        targetEntity.Heal(actionEntity.entityInfo.GetTotalBuffStat().attackDmg * adRatio + constantValue);
    }
}
