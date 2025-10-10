using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.UI;

public class SelectEntityButton : MonoBehaviour
{
    protected Button button;
    protected InGamePlayerUI inGamePlayerUI;
    protected InGameUIManager inGameUIManager;
    protected virtual void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();
        inGameUIManager.AddSkillButtonAction(ActiveSkillButtonAction, DeActiveSkillButtonAction);
    }

    public virtual void OnClickButton()
    {
    }

    protected virtual void OnClickActionButton(Skill skill)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickButton);
    }

    public virtual void ActiveSkillButtonAction(Skill skill)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClickActionButton(skill));
    }

    public virtual void DeActiveSkillButtonAction()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClickButton());
    }

    public void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
}
