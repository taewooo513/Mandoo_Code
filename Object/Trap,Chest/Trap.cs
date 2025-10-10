using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Trap : MonoBehaviour //얘 ui랑 분리해서 리팩토링해야되는데, 함정 자체가 ui로 제작되어서 애매함...
{
    [SerializeField] private GameObject activeTrapUI;
    [SerializeField] private GameObject notActiveTrapUI; //발동x 함정 오브젝트 버튼
    [SerializeField] private GameObject notActiveTrap;
    [SerializeField] private RectTransform activeTrapRect; //발동한 함정 오브젝트 위치
    [SerializeField] private GameObject trapUI; //함정 해제/취소 선택창 UI
    [SerializeField] private NotActiveTrapUI notActiveTrapUIcs;
    //[SerializeField] private InputActionReference moveAction;
    private BaseEntity _trappedPlayer;
    private Vector3 _startPos = new Vector3(0, -1025, 0);
    private Vector3 _targetPos = new Vector3(0, -365, 0);

    public void Awake()
    {
        if (notActiveTrapUIcs == null && notActiveTrap != null) //null 방지 코드
        {
            notActiveTrapUIcs = notActiveTrap.GetComponent<NotActiveTrapUI>();
        }
    }

    public void Start()
    {
        activeTrapUI.SetActive(false);
        trapUI.SetActive(false);
        notActiveTrapUI.SetActive(false);
        activeTrapRect.anchoredPosition = _startPos; //시작 위치
    }

    public void OnTrapUI()
    {
        trapUI.SetActive(true);
        Time.timeScale = 0f; //플레이어 못 움직임
    }

    public void OnTrapUIClose()
    {
        trapUI.SetActive(false);
        Time.timeScale = 1f; //캐릭터 움직임 활성화
    }

    public void OnTrapRelease() //클릭해서 함정 해제하기
    {
        //int randomNum = Random.Range(0, 2); //0, 1중 랜덤 숫자
        //var item = InventoryManager.Instance.BoomBreakToolCheck();
        
        //if (item != null) //아이템의 '함정 해제 도구'를 사용중이라면 100%로 해제
        //{
        //    //InventoryManager.Instance.UseItem(item); //사용아이템 차감
        //    TrapReleaseSuccess(); //해제 성공
        //}
        //else //아이템 사용중이 아니라면
        //{
        //    switch (randomNum)
        //    {
        //        case 0: //해제 성공
        //            TrapReleaseSuccess();
        //            break;
        //        case 1: //실패
        //            _trappedPlayer = GameManager.Instance.PlayableCharacter[0]; //제일 앞에 있는 플레이어
        //            TrapReleaseFail(_trappedPlayer); //데미지 입기
        //            Destroy(notActiveTrap); //함정 삭제
        //            Time.timeScale = 1f; //캐릭터 움직임 활성화
        //            //todo : 체력 닳고 난 후에 체력바 ui 불러서 화면에 띄워줘야 되나?
        //            _trappedPlayer = null; //혹시 몰라서 초기화
        //            break;
        //    }
        //}
    }

    public async void TrapActive(BaseEntity trappedPlayer) //함정 발동
    {   
        _trappedPlayer = trappedPlayer;
        //activeTrapRect.DOAnchorPos(_targetPos, 0.1f).SetEase(Ease.Linear); //함정 튀어나오기 -> 필요없어진 부분1. 애니메이션으로 변경 필요함.
        Time.timeScale = 0f; //플레이어 못 움직임
        if (notActiveTrapUIcs != null)
        {
            notActiveTrapUIcs.CatchTrapNotClick(); //트랩이 발동했을 때, 트랩 다시 못 누르도록 하기
        }
        activeTrapUI.SetActive(true); //빨개지는 UI 뜨기
        TrapReleaseFail(_trappedPlayer); //데미지 닳음
        await Task.Delay(1000); //딜레이 주기
        ActiveTrapReturn(_trappedPlayer); //발동한 함정 원위치
        Destroy(notActiveTrap); //함정 삭제
        Time.timeScale = 1f; //캐릭터 움직임 활성화
    }

    public async void ActiveTrapReturn(BaseEntity trappedPlayer) //발동한 함정 원위치
    {
        //activeTrapRect.DOAnchorPos(_startPos, 0.1f).SetEase(Ease.Linear); //트랩 원위치
        activeTrapUI.SetActive(false); //UI 끄기
    }

    public void TrapReleaseSuccess() //미발동 함정 해제 성공 시
    {
        notActiveTrapUI.SetActive(false); //버튼 UI 끄기
        Destroy(notActiveTrap); //함정 삭제
        Time.timeScale = 1f; //캐릭터 움직임 활성화
    }

    public void TrapReleaseFail(BaseEntity trappedPlayer) //함정 해제 실패
    {
        double damage = trappedPlayer.entityInfo.maxHp * 0.1; //최대 체력의 10% 피해
        trappedPlayer.Damaged((int)damage); //체력 감소
        
        if (trappedPlayer.entityInfo.currentHp <= 0)
        {
            BattleManager.Instance.EntityDead(trappedPlayer); //리스트에서 빼주기
        }
    }
}
