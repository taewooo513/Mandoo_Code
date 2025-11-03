using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainTitleUI : UIBase
{
    public OptionButtonUI optionButtonUI;
    private LoadOutUI _loadOutUI;
    private LoadOutUIShop _loadOutUIShop;
    private LevelChoiceUI _levelChoiceUI;

    public void Start()
    {
        optionButtonUI.CloseInGameOptionUI();
        AudioManager.Instance.PlayBGM(AudioInfo.Instance.titleBGM, AudioInfo.Instance.titleBGMVolume);
        if (DataManager.Instance.SaveData.SettingSave.IsMuted)
        {
            AudioManager.Instance.MuteAudio();
        }
        _loadOutUI = UIManager.Instance.GetUI<LoadOutUI>();
        _loadOutUIShop = UIManager.Instance.GetUI<LoadOutUIShop>();
        _levelChoiceUI = UIManager.Instance.GetUI<LevelChoiceUI>();
        _loadOutUI.CloseUI();
        _loadOutUIShop.CloseUI();
        _levelChoiceUI.CloseUI();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_loadOutUI != null && _loadOutUI.gameObject.activeInHierarchy && _levelChoiceUI.gameObject.activeInHierarchy)
            {
                _levelChoiceUI.CloseUI();
            }
            else if (_loadOutUI != null && _loadOutUI.gameObject.activeInHierarchy)
            {
                _loadOutUI.CloseUI();
            }
            else if (_loadOutUIShop != null && _loadOutUIShop.gameObject.activeInHierarchy)
            {
                _loadOutUIShop.CloseUI();
            }
        }
    }

    public void OnClickGameStartButton()
    {
        if (!DataManager.Instance.SaveData.IsStageCleared(0))
        {
            AnalyticsManager.Instance.SendEventStep(2);
            GameManager.Instance.StartTutorial();
            return;
        }
        AnalyticsManager.Instance.SendEventStep(29);
        UIManager.Instance.OpenUI<LoadOutUI>();
    }
    
    public void OnClickLoadOutUI()
    {
        UIManager.Instance.OpenUI<LoadOutUIShop>();
    }
    
    public void OnClickCloseLoadOutUI()
    {
        UIManager.Instance.CloseUI<LoadOutUIShop>();
    }

    public void OpenOptionUI()
    {
        //MenuController.CanControl = false;
        UIManager.Instance.OpenUI<OptionUI>().OnClickOpen();
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
