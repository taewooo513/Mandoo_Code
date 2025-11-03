using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OptionButtonUI : UIBase
{
    public GameObject optionUI;
    private OptionMiddleInit _optionMiddleInit;

    public void Awake()
    {
        if (optionUI != null)
        {
            _optionMiddleInit = optionUI.GetComponentInChildren<OptionMiddleInit>(true);
        }
        if (_optionMiddleInit == null)
        {
            Debug.LogWarning("OptionMiddleInit 스크립트를 찾을 수 없습니다!");
        }
    }

    public void OpenInGameOptionUI()
    {
        if (_optionMiddleInit != null)
        {
            _optionMiddleInit.isInGameOption = true;
        }
        GameManager.Instance.uiPlayerCanMove = false;
        UIManager.Instance.OpenUI<OptionUI>().OnClickOpen();
    }
    
    public void CloseInGameOptionUI()
    {
        if (_optionMiddleInit != null)
        {
            _optionMiddleInit.isInGameOption = false;
        }
        GameManager.Instance.uiPlayerCanMove = true;
        UIManager.Instance.CloseUI<OptionUI>();
        //MenuController.CanControl = true;
    }
}
