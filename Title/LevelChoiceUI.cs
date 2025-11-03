using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChoiceUI : UIBase
{
    public GameObject level2Button;
    public GameObject level3Button;
    
    void Start()
    {
        //AnalyticsManager.Instance.SendEventStep(27);
        LevelClearCheck();
    }
    
    public void LevelClearCheck()
    {
        if (!DataManager.Instance.SaveData.IsStageCleared(1))
        {
            level2Button.SetActive(false);
            level3Button.SetActive(false);
        }
        else if (!DataManager.Instance.SaveData.IsStageCleared(2))
        {
            level2Button.SetActive(true);
            level3Button.SetActive(false);
        }
        else
        {
            level2Button.SetActive(true);
            level3Button.SetActive(true);
        }
    }

    public void OnClickDifficultyLevel1()
    {
        GameManager.Instance.StartGame(1);
        
    }
    public void OnClickDifficultyLevel2()
    {
        GameManager.Instance.StartGame(2);
    }
    public void OnClickDifficultyLevel3()
    {
        GameManager.Instance.StartGame(3);
    }
    
    public void OnClickBack()
    {
        UIManager.Instance.CloseUI<LevelChoiceUI>();
    }
}
