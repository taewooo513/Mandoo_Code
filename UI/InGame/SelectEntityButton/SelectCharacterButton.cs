using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class SelectCharacterButton : SelectEntityButton
{
    PlayableCharacter player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<PlayableCharacter>();
    }

    protected override void OnClickActionButton(Skill skill) // 스킬사용
    {
        skill.UseSkill(player);
        inGameUIManager.buttonSkillDeActiveAction?.Invoke();
    }

    public override void ActiveSkillButtonAction(Skill skill)
    {
        if (skill.GetSkillType() == EffectType.Debuff || skill.GetSkillType() == EffectType.Attack)
        {
            return;
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClickActionButton(skill));
        BattleManager.Instance.isEndTrun = false;
    }
}