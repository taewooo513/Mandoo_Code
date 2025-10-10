using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InGameUIManager : UIBase
{
    private Action<Skill> buttonSkillActiveAction;
    public Action buttonSkillDeActiveAction;


    public void OnClickSkillButton(Skill skill)
    {
        buttonSkillActiveAction?.Invoke(skill); // null 체크
    }

    public void AddSkillButtonAction(Action<Skill> action, Action action1)
    {
        buttonSkillActiveAction += action;
        buttonSkillDeActiveAction += action1;
    }

    public void RemoveSkillButtonAction(Action<Skill> action, Action action1)
    {
        buttonSkillActiveAction -= action;
        buttonSkillDeActiveAction -= action1;
    }

    public void OpenInventoryUI()
    {
        UIManager.Instance.CloseUI<MapUI>();
        UIManager.Instance.CloseUI<InGameEnemyUI>();
        //UIManager.Instance.OpenUI<InGameInventoryUI>();
    }

    public void CloseInventoryUI()
    {
        //UIManager.Instance.OpenUI<InGameEnemyUI>();
        //UIManager.Instance.CloseUI<InGameInventoryUI>();
    }
    public void OpenMapUI()
    {
        UIManager.Instance.CloseUI<InGameEnemyUI>();
        //UIManager.Instance.CloseUI<InGameInventoryUI>();
        UIManager.Instance.OpenUI<MapUI>();
    }
    public void CloseMapUI()
    {
        //UIManager.Instance.OpenUI<InGameEnemyUI>();
        UIManager.Instance.CloseUI<MapUI>();
    }
    public void DeselectSkill()
    {
    }
}

