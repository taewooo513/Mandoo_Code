using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RangeCheck : MonoBehaviour //NotActiveTrap, TreasureChest 등 범위 체크해야되는 오브젝트에 사용
{
    public GameObject outline;
    public GameObject openUIButton; //오브젝트 눌렀을 때 열고싶은 UI(버튼). notActiveTrapUI, ChestUI 등 
    public LayerMask playableLayer; //레이어 선택
    private float _findRange = 4f; //범위
    public bool uiIsInteract = false; //상호작용 여부 (ui 한 번 띄웠는지)
    private NotActiveTrapUI _notActiveTrapUI;

    public void Start() //UI 켜져있으면 전부 끄고 시작 
    {
        outline.SetActive(false);
        openUIButton.SetActive(true);
    }

    public void Update()
    {
        Collider2D playableSensor = Physics2D.OverlapCircle(transform.position, _findRange, playableLayer);
        if (playableSensor != null) //플레이어가 다가왔을 때
        {
            if (!uiIsInteract) //상호작용한 상태가 아니라면
            {
                outline.SetActive(true); //ui 뜨도록
                openUIButton.SetActive(true);
            }
            else //이미 상호작용 했으면 (uiIsInteract == true)
            {
                //if (_treasureChest != null)
                //{
                //    _treasureChest.openChestButton.interactable = false; //상자 다시 눌러도 ui 안 뜨도록 수정
                //    _treasureChest.Outline.SetActive(false); //외곽선 끄기
                //}
                if (_notActiveTrapUI != null)
                {
                    _notActiveTrapUI.notActiveTrapButton.interactable = false; //함정에 걸렸을 때 ui 안 뜨도록 수정
                    _notActiveTrapUI.outline.SetActive(false); //외곽선 끄기
                }
            }
        }
        if(playableSensor == null)
        {
            outline.SetActive(false);
            openUIButton.SetActive(false);
        }
    }
    
    //public void Init(TreasureChest childUI)
    //{
    //    _treasureChest = childUI;
    //}
    
    public void Init(NotActiveTrapUI childUI)
    {
        _notActiveTrapUI = childUI;
    }

    void OnDrawGizmos() //범위 그리기
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _findRange);
    }
}
