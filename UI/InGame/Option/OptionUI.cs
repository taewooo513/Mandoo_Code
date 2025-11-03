using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionUI : UIBase
{
    public GameObject optionMiddleInit;
    public GameObject optionMiddleSound;
    public GameObject optionMiddleKey;
    public OptionMiddleInit optionMiddleInitCs;
    private OptionUI _optionUI;
    private HelpUI _helpUI;
    
    public void Start()
    {
        OnClickInit();
        _optionUI = UIManager.Instance.GetUI<OptionUI>();
        _helpUI = UIManager.Instance.GetUI<HelpUI>();
        _helpUI.CloseUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_helpUI != null && _helpUI.gameObject.activeInHierarchy && _helpUI.gameObject.activeSelf) //도움말 닫기
            {
                _helpUI.OnClickClose();
            }
            else if (_optionUI.optionMiddleSound.activeInHierarchy || _optionUI.optionMiddleKey.activeInHierarchy) //뒤로가기
            {
                OnClickBack();
            }
            else
            { 
                _optionUI.OnClickClose();
            }
        }
    }

    public void OnClickOpen()
    {
        OnClickInit();
    }

    public void OnClickInit()
    {
        optionMiddleInitCs.OnInGameOption();
        optionMiddleSound.SetActive(false);
        optionMiddleKey.SetActive(false);
        optionMiddleInit.SetActive(true);
    }
    
    public void OnClickBack() //뒤로가기 버튼
    {
        OnClickInit();
    }

    public void OnClickHelp()
    {
        UIManager.Instance.OpenUI<HelpUI>();
    }

    public void OnClickSound()
    {
        optionMiddleInit.SetActive(false);
        optionMiddleSound.SetActive(true);
    }

    public void OnClickKey()
    {
        optionMiddleInit.SetActive(false);
        optionMiddleKey.SetActive(true);
    }

    public void OnClickTutorial()
    {
        Tutorials.ResetAllSessionShown();
        GameManager.Instance.StartTutorial();
    }

    public void OnClickBackToTitle()
    {
        BattleManager.Instance.EndBattle();
        InGameItemManager.Instance.ClearInventory();
        SceneLoadManager.Instance.LoadScene(SceneKey.titleScene);
        UIManager.Instance.ClearUI();
    }

    public void OnClickClose()
    {
        UIManager.Instance.CloseUI<OptionUI>();
        //MenuController.CanControl = true;
        GameManager.Instance.uiPlayerCanMove = true;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
