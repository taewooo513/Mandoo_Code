using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectEnemyButton : SelectEntityButton
{
    Enemy enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
    }

    public override void OnClickButton()
    {
        //UIManager.Instance.CloseUI<InGameInventoryUI>();
        UIManager.Instance.CloseUI<MapUI>();
        UIManager.Instance.OpenUI<InGameEnemyUI>().UpdateUI(enemy.entityInfo);
    }
    public override void ActiveSkillButtonAction(Skill skill)
    {
        if (skill.GetSkillType() == EffectType.Mark || skill.GetSkillType() == EffectType.Buff || skill.GetSkillType() == EffectType.Heal || skill.GetSkillType() == EffectType.Protect)
        {
            return;
        }
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClickActionButton(skill));
    }

    protected override void OnClickActionButton(Skill skill)
    {
        skill.UseSkill(enemy);
        inGameUIManager.buttonSkillDeActiveAction?.Invoke();
        BattleManager.Instance.isEndTrun = false;
    }
}
