using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Trap : MonoBehaviour //얘 이름 바꾸고 리팩토링해야되는데 기찮다...
{
    [SerializeField] private GameObject activeTrapUI;
    [SerializeField] private GameObject notActiveTrapUI; //발동x 함정 오브젝트 버튼
    [SerializeField] private GameObject notActiveTrap;
    [SerializeField] private NotActiveTrapUI notActiveTrapUIcs;
    public SpriteRenderer trap;
    private BaseEntity _trappedPlayer;
    private Animator _animator;
    public GameObject trapUI;

    private Cell _cell;
    public void Awake()
    {
        if (notActiveTrapUIcs == null && notActiveTrap != null) //null 방지 코드
        {
            notActiveTrapUIcs = notActiveTrap.GetComponent<NotActiveTrapUI>();
        }
        _animator = GetComponentInChildren<Animator>();
    }

    public void Init(Cell cell = null)
    {
        _cell = cell;
    }
    public void Start()
    {
        activeTrapUI.SetActive(false);
        trapUI.SetActive(false);
        notActiveTrapUI.SetActive(false);
    }

    public void OnTrapUI()
    {
        trapUI.SetActive(true);
        GameManager.Instance.playerObjectInteract = true; //플레이어 못 움직임. 타임스케일 0
    }

    public void OnTrapUIClose()
    {
        trapUI.SetActive(false);
        GameManager.Instance.playerObjectInteract = false; //캐릭터 움직임 활성화
    }

    public void OnTrapRelease() //클릭해서 함정 해제하기
    {
        _cell.AlreadyVisited = true;
        if (GameManager.Instance.CurrentMapIndex == 0)
        {
            _trappedPlayer = GameManager.Instance.playableCharacter[0]; //제일 앞에 있는 플레이어
            TrapActive(_trappedPlayer);
            return;
        }

        int randomNum = Random.Range(0, 2); //0, 1중 랜덤 숫자
        var item = InGameItemManager.Instance.BoomBreakToolCheck();

        if (item != null) //아이템의 '함정 해제 도구'를 사용중이라면 100%로 해제
        {
            item.UseItem(1); //사용아이템 차감
            UIManager.Instance.CloseUI<MapUI>();
            UIManager.Instance.OpenUI<InGameInventoryUI>().UpdateUI();
            TrapReleaseSuccess(); //해제 성공
        }
        else //아이템 사용중이 아니라면
        {
            switch (randomNum)
            {
                case 0: //해제 성공
                    TrapReleaseSuccess();
                    break;
                case 1: //실패
                    _trappedPlayer = GameManager.Instance.playableCharacter[0]; //제일 앞에 있는 플레이어
                    TrapActive(_trappedPlayer);
                    GameManager.Instance.playerObjectInteract = false;
                    break;
            }
        }
    }

    public void TrapActive(BaseEntity trappedPlayer) //함정 발동
    {
        _cell.AlreadyVisited = true;
        if (GameManager.Instance.CurrentMapIndex == 0)
            AnalyticsManager.Instance.SendEventStep(11);
        _trappedPlayer = trappedPlayer;
        trapUI.SetActive(false);
        notActiveTrapUIcs.outline.SetActive(false);
        activeTrapUI.SetActive(true); //빨개지는 UI 뜨기
        trap.color = new Color(1, 1, 1, 0f);
        _animator.SetTrigger("IsActive"); //애니메이션 실행
    }

    public void EndTrapActive() //애니메이션 동작 후 실행될 함수들
    {
        GameManager.Instance.playerObjectInteract = true;
        if (notActiveTrapUIcs != null)
        {
            notActiveTrapUIcs.CatchTrapNotClick(); //트랩이 발동했을 때, 트랩 다시 못 누르도록 하기
        }
        activeTrapUI.SetActive(true); //빨개지는 UI 뜨기
        TrapReleaseFail(_trappedPlayer); //데미지 닳음
        StartCoroutine(EndTrap());
        Tutorials.ShowIfNeeded<UseItemTutorial>();
    }

    IEnumerator EndTrap()
    {
        yield return new WaitForSeconds(1f);
        Destroy(notActiveTrap); //함정 삭제
        GameManager.Instance.playerObjectInteract = false; //캐릭터 움직임 활성화
    }

    public void TrapReleaseSuccess() //미발동 함정 해제 성공 시
    {
        notActiveTrapUI.SetActive(false); //버튼 UI 끄기
        AchievementManager.Instance.AddParam("useUntrapTool", 1);

        Destroy(notActiveTrap); //함정 삭제
        GameManager.Instance.playerObjectInteract = false; //캐릭터 움직임 활성화
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
