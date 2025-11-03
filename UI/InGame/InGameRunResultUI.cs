using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameRunResultUI : UIBase
{
    [SerializeField] private GameObject playerIconSlot; //프리팹
    [SerializeField] private RectTransform content; //프리팹 생성할 위치
    public Button returnBtn; //처음으로 돌아가는 버튼
    public TextMeshProUGUI roomCountText;
    public TextMeshProUGUI playerCountText;
    public TextMeshProUGUI descriptionText;

    public void Start()
    {
        Tutorials.ShowIfNeeded<RunResultTutorial>();
        if(AnalyticsManager.Instance.Step == 31)
            AnalyticsManager.Instance.SendEventStep(32);
        returnBtn.onClick.AddListener(OnClickBtn);
        Connect();
    }

    public void Connect()
    {
        List<BaseEntity> playableCharacter = GameManager.Instance.playableCharacter; //살아있는 캐릭터 리스트 가져오기
        foreach (BaseEntity character in playableCharacter) //살아있는 캐릭터만큼 반복 돌리면서
        {
            Debug.Log(playableCharacter.Count);
            GameObject slot = Instantiate(playerIconSlot, content); //특정 위치에 프리팹 생성

            string path = character.entityInfo.iconPath; //플레이어 아이콘(스프라이트) 경로
            Sprite playerIcon = Resources.Load<Sprite>(path); //아이콘 불러오기

            //플레이어 아이콘 스프라이트 -> 프리팹 이미지에 할당
            Image playerIconImage = slot.GetComponentInChildren<Image>();
            if (playerIconImage != null && playerIcon != null)
            {
                playerIconImage.sprite = playerIcon;
            }
            else
            {
                Debug.LogWarning($"플레이어 아이콘 로드 실패 : {path}");
            }
        }

        //탐색한 방의 수 띄워주기
        int roomCount = MapManager.Instance.RoomVisitedCount;
        roomCountText.text = roomCount.ToString();
        
        int playerCount = playableCharacter.Count;
        playerCountText.text = playerCount.ToString();
    }

    public void Description(int mapIndex, bool isClear, bool isNewlyCleared)
    {
        if (isClear)
        {
            AudioManager.Instance.PlayBGM(AudioInfo.Instance.battleClearSfx, AudioInfo.Instance.battleClearSfxVolume);
            if (isNewlyCleared) //처음 깬 스테이지일때
            {
                switch (mapIndex)
                {
                    case 0 :
                        descriptionText.text = "튜토리얼 클리어!";
                        AchievementManager.Instance.AddParam("completeTutorial", 1);

                        break;
                    case 1 : 
                        descriptionText.text = "[쉬움] 난이도 최초 클리어! [보통] 난이도가 해금되었습니다.";
                        break;
                    case 2 : 
                        descriptionText.text = "[보통] 난이도 최초 클리어! [어려움] 난이도가 해금되었습니다.";
                        break;
                    case 3 : 
                        descriptionText.text = "[어려움] 난이도 최초 클리어! 축하드립니다! 모든 난이도를 다 클리어하셨습니다!";
                        break;
                    default:
                        break;
                }
            }
            else //클리어는 했지만 여러 번 깼을 때
            {
                switch (mapIndex)
                {
                    case 0 : 
                        descriptionText.text = "튜토리얼이 그리우셨군요? 축하드립니다!";
                        break;
                    case 1 : 
                        descriptionText.text = "[쉬움] 난이도 클리어!";
                        break;
                    case 2 : 
                        descriptionText.text = "[보통] 난이도 클리어!";
                        break;
                    case 3 : 
                        descriptionText.text = "[어려움] 난이도 클리어! 나도 이제 고인물?!";
                        break;
                }
            }
        }
        else //사망했을 때
        {
            AudioManager.Instance.PlayBGM(AudioInfo.Instance.gameOverBGM, AudioInfo.Instance.gameOverBGMVolume);
            descriptionText.text = "다음에는 더 잘 할 수 있을 겁니다.. 파이팅!";
        }
    }

    public void OnClickBtn() //처음으로 버튼
    {
        SceneLoadManager.Instance.LoadScene(SceneKey.titleScene);
        GameManager.Instance.playerCanMove = true;
        UIManager.Instance.CloseUI<InGameRunResultUI>();

    }
}
