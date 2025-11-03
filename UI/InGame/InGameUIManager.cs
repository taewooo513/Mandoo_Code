using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : UIBase
{
    private Action<Skill, InGameItem> buttonSkillActiveAction;
    public Action buttonSkillDeActiveAction;

    // 맵과 인벤토리를 열리는 거에 맞게 이미지의 컬러 값 조절
    // 맵과 인벤토리는 UIManager에서 열고 관리하고 있음.
    // 인벤토리가 열렸을 떄 인벤토리 아이콘 밝게 / 맵 아이콘 음영 처리
    // 맵이 열렸을 때 맵 아이콘 밝게 / 인벤토리 아이콘 음영 처리
    // 둘 다 안 열려있을 때 둘다 어둡게

    public SelectEntityButton[] entityButtons;

    public Image mapImage;
    public Image inventoryImage;
    public GameObject highlight;

    private bool _isMapActive;
    private bool isHighlight = false;

    public void Awake()
    {
        entityButtons = new SelectEntityButton[8];
    }
    private void OnEnable()
    {
        UIManager.Instance.OnUIVisibilityChanged += OnSwithImageShadow;
    }

    private void OnDisable()
    {
        UIManager.Instance.OnUIVisibilityChanged -= OnSwithImageShadow;
    }

    public void OnSwithImageShadow(object obj, bool isActive)
    {
        Color activeColor = Color.white;
        Color inactiveColor = new Color(135f / 255f, 135f / 255f, 135f / 255f); ;

        if(obj is MapUI)
        {
            _isMapActive = isActive;
            highlight.SetActive(!_isMapActive && isHighlight);
            mapImage.color = _isMapActive ? activeColor : inactiveColor;
        }
        if (obj is InGameInventoryUI)
            inventoryImage.color = isActive ? activeColor : inactiveColor;
    }

    public void OnClickSkillButton(Skill skill, InGameItem inGameItem = null)
    {
        if (skill != null)
        {
            buttonSkillActiveAction?.Invoke(skill, inGameItem); // null 체크
        }
        else
        {
            if (null != entityButtons)
            {
                for (int i = 0; i < entityButtons.Length; i++)
                {
                    if (entityButtons[i] != null)
                        entityButtons[i].DeActiveSkillButtonAction();
                }
            }
        }
    }
    public void RemoveSkillButton()
    {
        if (null != entityButtons)
        {
            for (int i = 0; i < entityButtons.Length; i++)
            {
                if (entityButtons[i] != null)
                    entityButtons[i].DeActiveSkillButtonAction();
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            for (int i = 0; i < entityButtons.Length; i++)
            {
                if (entityButtons[i] != null)
                {
                    entityButtons[i].DeActiveSkillButtonAction();
                }
            }
        }
    }

    public void AddSkillButtonAction(Action<Skill, InGameItem> action, Action action1)
    {
        buttonSkillActiveAction += action;
        buttonSkillDeActiveAction += action1;
    }

    public void RemoveSkillButtonAction()
    {
        buttonSkillActiveAction = null;
        buttonSkillDeActiveAction = null;
    }

    public void OpenInventoryUI()
    {
        UIManager.Instance.CloseUI<MapUI>();
        UIManager.Instance.CloseUI<InGameEnemyUI>();
        UIManager.Instance.OpenUI<InGameInventoryUI>();
    }

    public void CloseInventoryUI()
    {
        UIManager.Instance.OpenUI<InGameEnemyUI>();
        UIManager.Instance.CloseUI<InGameInventoryUI>();
    }
    public void OpenMapUI()
    {
        UIManager.Instance.CloseUI<InGameEnemyUI>();
        UIManager.Instance.CloseUI<InGameInventoryUI>();
        highlight.SetActive(false);
        UIManager.Instance.OpenUI<MapUI>();
    }
    public void CloseMapUI()
    {
        UIManager.Instance.OpenUI<InGameEnemyUI>();
        UIManager.Instance.CloseUI<MapUI>();
    }
    public void DeselectSkill()
    {
    }
    public void OnHighlightMap(bool isActive)
    {
        isHighlight = isActive;
        //if (!_isMapActive)  // 지도가 비활성화 되어 있을 때 하이라이트 처리
            highlight.SetActive(isActive);
    }

}

