using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectEntityButton : MonoBehaviour
{
    protected Button button;
    protected InGamePlayerUI inGamePlayerUI;
    protected InGameUIManager inGameUIManager;
    protected EventTrigger triggerEvent;
    [SerializeField]
    protected Image rangeCheck;
    protected Color defaultColor;
    protected Action action;
    protected Action exitAction;
    protected BaseEntity entity;
    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();
        inGameUIManager.AddSkillButtonAction(ActiveSkillButtonAction, DeActiveSkillButtonAction);
        triggerEvent = GetComponent<EventTrigger>();
        for (int i = 0; i < 8; i++)
        {
            if (UIManager.Instance.OpenUI<InGameUIManager>().entityButtons[i] == null)
            {
                UIManager.Instance.OpenUI<InGameUIManager>().entityButtons[i] = this;
                return;
            }
        }

    }

    public virtual void OnClickButton()
    {
    }

    protected virtual void OnClickActionButton(Skill skill, InGameItem inGameItem)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickButton);
    }

    public virtual void ActiveSkillButtonAction(Skill skill, InGameItem inGameItem)
    {
        DeActiveSkillButtonAction();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClickActionButton(skill, inGameItem));
    }

    public virtual void DeActiveSkillButtonAction()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClickButton());
        DefaultChangeColor();
        ActionClear();
    }

    public void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }

    protected void RangeCheckChangeColor(Skill skill, BaseEntity baseEntity, Action a)
    {
        DefaultChangeColor();
        ActionClear();

        for (int i = 0; i < skill.skillInfo.targetPos.Count; i++)
        {
            if (BattleManager.Instance.FindEntityPosition(baseEntity) == skill.skillInfo.targetPos[i])
            {
                a();
                break;
            }
        }
    }

    public void ActionClear()
    {
        action = null;
        exitAction = null;
    }

    public void OnChangeColor(Skill skill, Color color)
    {
        defaultColor = rangeCheck.color;
        rangeCheck.color = color;
    }

    public void OnMouseEnterChange()
    {
        action?.Invoke();
    }

    public void OnMouseExitChange()
    {
        exitAction?.Invoke();
    }

    protected void DefaultChangeColor()
    {
        Color color = new Color(120f / 255f, 120 / 255f, 120f / 255f);
        if (BattleManager.Instance.NowTurnEntity == entity)
        {
            Debug.Log("내턴이지롱"); color = new Color(255f / 255f, 255f / 255f, 0);
        }
        defaultColor = color;
        if (rangeCheck != null)
        {
            rangeCheck.color = defaultColor;
        }
    }

}
