using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuffSkill : SkillEffect
{
    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity) // not used
    {
        BuffInfo statEffectInfo = new BuffInfo();
        Debug.Log("fdskml");
        targetEntity.AddEffect(statEffectInfo);
    }
}
