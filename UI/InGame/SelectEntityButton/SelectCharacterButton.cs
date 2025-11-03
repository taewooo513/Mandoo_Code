using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCharacterButton : SelectEntityButton
{
    PlayableCharacter player;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponentInParent<PlayableCharacter>();
        entity = player;
    }

    public override void OnClickButton()
    {
        if (!BattleManager.Instance.isBattleStarted)
            UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(player.entityInfo, player.entityInfo.skills, player);
    }

    protected override void OnClickActionButton(Skill skill, InGameItem inGameItem) // 스킬사용
    {
        inGameUIManager.buttonSkillDeActiveAction?.Invoke();
        DefaultChangeColor();
        skill.UseSkill(player);
        if (inGameItem != null)
        {
            inGameItem.RemoveItem(1);
            UIManager.Instance.CloseUI<MapUI>();
            UIManager.Instance.OpenUI<InGameInventoryUI>().UpdateUI();
            if (BattleManager.Instance.isBattleStarted)
                BattleManager.Instance.isEndTurn = true;
            AchievementManager.Instance.AddParam("useConsumable", 1);
            ActionClear();
        }
        else
        {
            BattleManager.Instance.isEndTurn = false;
        }
    }

    public override void ActiveSkillButtonAction(Skill skill, InGameItem inGameItem)
    {
        ActionClear();
        DefaultChangeColor();
        button.onClick.RemoveAllListeners();
        if (skill.GetSkillType() == EffectType.Debuff || skill.GetSkillType() == EffectType.Attack)
        {
            return;
        }

        RangeCheckChangeColor(skill, player, () =>
        {
            Color color;
            defaultColor = rangeCheck.color;
            button.onClick.AddListener(() => OnClickActionButton(skill, inGameItem));
            color = new Color(82f / 255f, 0, 0);
            rangeCheck.color = color;
            action += () => base.OnChangeColor(skill, new Color(241f / 255f, 0, 0));
            exitAction += () => base.OnChangeColor(skill, new Color(82f / 255f, 0, 0));
        });
    }
}