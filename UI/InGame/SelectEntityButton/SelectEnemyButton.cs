using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectEnemyButton : SelectEntityButton
{
    Enemy enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponentInParent<Enemy>();
        entity = enemy;
    }

    public override void OnClickButton()
    {
        UIManager.Instance.OpenUI<InGameEnemyUI>().UpdateUI(enemy.entityInfo);
    }
    public override void ActiveSkillButtonAction(Skill skill, InGameItem inGameItem)
    {
        ActionClear();
        DefaultChangeColor();
        button.onClick.RemoveAllListeners();
        if (skill.GetSkillType() == EffectType.SwapPosition || skill.GetSkillType() == EffectType.Mark || skill.GetSkillType() == EffectType.Buff || skill.GetSkillType() == EffectType.Heal || skill.GetSkillType() == EffectType.Protect)
        {
            return;
        }

        RangeCheckChangeColor(skill, enemy, () =>
        {
            button.onClick.AddListener(() => OnClickActionButton(skill, inGameItem));
            Color color;
            defaultColor = rangeCheck.color;
            color = new Color(82f / 255f, 0, 0);
            rangeCheck.color = color;
            action += () => base.OnChangeColor(skill, new Color(241f / 255f, 0, 0));
            exitAction += () => base.OnChangeColor(skill, new Color(82f / 255f, 0, 0));
        });
    }

    protected override void OnClickActionButton(Skill skill, InGameItem inGameItem)
    {
        skill.UseSkill(enemy);
        DefaultChangeColor();
        inGameUIManager.buttonSkillDeActiveAction?.Invoke();
        if (inGameItem != null)
        {
            inGameItem.RemoveItem(1);
            UIManager.Instance.CloseUI<MapUI>();
            UIManager.Instance.OpenUI<InGameInventoryUI>().UpdateUI();
            AchievementManager.Instance.AddParam("useConsumable", 1);
            BattleManager.Instance.isEndTurn = true;
            ActionClear();
        }
        else
        {
            BattleManager.Instance.isEndTurn = false;
        }
    }
}
