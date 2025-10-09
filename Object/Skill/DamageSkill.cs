using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : SkillEffect
{
    public void DeBuffDamageSkill(int duration, BaseEntity targetEntity)
    {
    }
    public void HitDamageSkill(BaseEntity attackEntity, BaseEntity damagedEntity)
    {
        float dmg = ((float)attackEntity.entityInfo.GetTotalBuffStat().attackDmg) * adRatio;
        Debug.Log((float)attackEntity.entityInfo.GetTotalBuffStat().attackDmg);
        attackEntity.Attack(dmg, damagedEntity);
    }
    public void UseBuffSkill(BaseEntity attackEntity, BaseEntity damagedEntity)
    {
        if (duration != 0)
        {
            DeBuffDamageSkill(duration, damagedEntity);
        }
        else
        {
            HitDamageSkill(attackEntity, damagedEntity);
        }
    }

    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        HitDamageSkill(actionEntity, targetEntity);
    }
}
