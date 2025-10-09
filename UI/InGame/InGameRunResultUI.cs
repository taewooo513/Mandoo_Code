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

    public void Start()
    {
        returnBtn.onClick.AddListener(OnClickBtn);
        Connect();
    }

    public void Connect()
    {
        List<BaseEntity> playableCharacter = GameManager.Instance.PlayableCharacter; //살아있는 캐릭터 리스트 가져오기
        foreach (BaseEntity character in playableCharacter) //살아있는 캐릭터만큼 반복 돌리면서
        {
            Debug.Log(playableCharacter.Count);
            GameObject slot = Instantiate(playerIconSlot, content); //특정 위치에 프리팹 생성

            string path = character.entityInfo.gameObjectString; //플레이어 아이콘(스프라이트) 경로
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
    }

    public void OnClickBtn() //처음으로 버튼
    {
        UIManager.Instance.CloseUI<InGameRunResultUI>();
    }
}
