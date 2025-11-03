using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapSkill : SkillEffect
{
    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        if (actionEntity == targetEntity)
        {
            return;
        }
        BattleManager.Instance.SwitchPlayerPosition((PlayableCharacter)actionEntity, (PlayableCharacter)targetEntity);
        BattleManager.Instance.EndTurn();
        BattleManager.Instance.isEndTurn = false;
    }
}
