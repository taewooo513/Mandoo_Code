using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionMiddleInit : MonoBehaviour
{
   public GameObject continueButton;
   public GameObject backTitleButton;
   public GameObject exitGameButton;
   public GameObject tutorialButton;
   public bool isInGameOption = false;

   public void Start()
   {
      string currentScene = SceneManager.GetActiveScene().name;
      if (currentScene.ToLower().Contains("title"))
      {
         isInGameOption = false;
      }
      else
      {
         isInGameOption = true;
      }
      
      OnInGameOption();
   }

   public void OnInGameOption()
   {
      if (isInGameOption) //인게임 옵션창이라면
      {
         continueButton.SetActive(true);
         backTitleButton.SetActive(true);
         exitGameButton.SetActive(false);
         tutorialButton.SetActive(false);
      }
      else //타이틀 옵션창인데
      {
         if (!DataManager.Instance.SaveData.IsStageCleared(0)) //튜토리얼 안 깨진 상태면
         {
            continueButton.SetActive(false);
            backTitleButton.SetActive(false);
            exitGameButton.SetActive(false);
            tutorialButton.SetActive(false); //'튜토리얼 다시하기' 버튼 비활성화
         }
         else
         {
            continueButton.SetActive(false);
            backTitleButton.SetActive(false);
            exitGameButton.SetActive(false);
            tutorialButton.SetActive(true); 
         }
      }
   }
}
