using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectSkillButton : MonoBehaviour
{

    private Button skillButton;
    private InGameUIManager inGameUIManager;

    private void Awake()
    {
        skillButton = GetComponent<Button>();
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();

    }
    public void SetButton(Skill skill)
    {
        skillButton.onClick.RemoveAllListeners();
        skillButton.onClick.AddListener(() => OnClickSkillButton(skill));
    }

    private void OnClickSkillButton(Skill skill)
    {
        if (BattleManager.Instance.isEndTrun == true)
        {
            inGameUIManager.OnClickSkillButton(skill);
        }
    }

}
