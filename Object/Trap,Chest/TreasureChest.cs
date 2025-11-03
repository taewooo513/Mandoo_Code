using System.Collections.Generic;
using DataTable;
using UnityEngine;
using UnityEngine.UI;

public class TreasureChest : MonoBehaviour
{
    InGameItem[] items;

    [Header("UI 관련")]
    public GameObject inGameTreasureChestUIPrefab; //상자 보상 UI
    private TreasureRoom _parentRoom;
    private InGameTreasureChestUI inGameTreasureChestUI; //드래그앤드롭 관리
    public Button openChestButton;
    [SerializeField] private RangeCheck _rangeCheck;
    public GameObject Outline;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    [Header("보상 관련")]
    private int _rewardId; //실제로 주는 보상id
    private int _battleRewardGroupId; //배틀데이터에 있는 그룹 아이디
    private int _rewardGroupId; //보상 테이블 연결해주는 id
    private List<RewardData> _rewardIdList; //그룹에 속한 id 리스트
    List<int> itemIdList = new(); //보상id 리스트
    List<int> itemCountList = new(); //보상 개수 리스트

    public BattleData battleData; //배틀데이터 데이터테이블
    public RewardData rewardData;

    private Cell _cell;
    private void Awake()
    {
        _rangeCheck = GetComponentInParent<RangeCheck>();
        inGameTreasureChestUI = GetComponentInChildren<InGameTreasureChestUI>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        inGameTreasureChestUIPrefab.SetActive(false); //시작할 때 ui 꺼줌
        inGameTreasureChestUI.gameObject.SetActive(false);
        
        if(_rangeCheck != null)
            _rangeCheck.Init(this);
    }
    public void InitReward(List<int> itemIdList, List<int> itemCountList) //보상 ui에 넣어주는용
    {
        if (itemIdList != null)
        {
            for (int i = 0; i < itemIdList.Count; i++) //아이템 넣어주기
            {
                int item = itemIdList[i]; //아이템id
                int count = itemCountList[i]; //아이템개수
                InGameItem itemValue = InGameItemManager.Instance.AddItem(item, count);
                items[i] = itemValue;
            }
        }
        inGameTreasureChestUI.Setting(items); //UI 세팅
    }

    public void Init(TreasureRoom parentRoom, int id) //보물 룸 전용
    {
        Setting(id);
        _parentRoom = parentRoom; //상호작용 체크용
        InitReward(itemIdList, itemCountList);
    }

    public void Init(Cell cell, int id) //통로 전용
    {
        _cell = cell;
        Setting(id);
        InitReward(itemIdList, itemCountList);
    }

    public void Setting(int id)
    {
        battleData = DataManager.Instance.Battle.GetBattleData(id);
        items = new InGameItem[10];
        
        _battleRewardGroupId = battleData.rewardId;
        _rewardGroupId = _battleRewardGroupId; //랜덤가챠 돌릴 범위
        _rewardIdList = DataManager.Instance.Reward.GetRewardGroupList(_rewardGroupId); //보상 그룹 가져오기
        ChestRewardDecision(); //보상 리스트 중 1개로 결정
    }

    public void OpenChestRewardUI() //상자 눌러서 오픈 시
    {
        var image = Outline.GetComponent<UnityEngine.UI.Image>();

        if (image != null)
        {
            var color = image.color;
            color.a = 0f;
            image.color = color;
        }
        Outline.SetActive(false);
        _spriteRenderer.color = new Color(1, 1, 1, 0f);
        AudioManager.Instance.PlaySfx(AudioInfo.Instance.chestOpenSfx, AudioInfo.Instance.chestOpenSfxVolume);
        _animator.SetTrigger("IsOpen");
        openChestButton.targetGraphic = null;
        openChestButton.GetComponent<Image>().raycastTarget = false;
        if (_cell != null)
        {
            _cell.AlreadyVisited = true;
        }
    }

    public void OpenChestEventEnd()
    {
        AchievementManager.Instance.AddParam("interactChest", 1);
        inGameTreasureChestUIPrefab.SetActive(true); //ui 켜줌
        inGameTreasureChestUI.gameObject.SetActive(true);

        if (_parentRoom != null) //부모가 있을 때만
        {
            _parentRoom.isInteract = true; //부모쪽의 상호작용 여부 true로 변경해줌
        }
        if (_rangeCheck != null) //범위 체크하는 부분이 있으면
        {
            _rangeCheck.Init(this);
            _rangeCheck.uiIsInteract = true; //다시 상호작용 안 되게끔 값변경
        }
        GameManager.Instance.playerObjectInteract = true; //플레이어 움직임 멈추기
    }

    public void ChestRewardDecision() //보상id 랜덤으로 뽑고, 보상 ui에 넣어주는 함수.
    {
        List<float> dropProbWeightList = new(); //가중치 리스트

        if (_battleRewardGroupId == _rewardGroupId) //그룹 아이디가 같을 때
        {
            for (int i = 0; i < _rewardIdList.Count; i++) //id 개수만큼 돌리면서 
            {
                dropProbWeightList.Add(_rewardIdList[i].dropProb); //인덱스 순으로 드랍 확률(가중치) 추가하기
            }
            _rewardId = RandomizeUtility.TryGetRandomPlayerIndexByWeight(dropProbWeightList); //가중치 돌려서 보상주는 방 id 뽑기
        }

        if (_rewardIdList[_rewardId].itemIdList.Count == _rewardIdList[_rewardId].itemCount.Count) //아이템 리스트와 아이템 개수가 같을 때
        {
            itemIdList.AddRange(_rewardIdList[_rewardId].itemIdList); //보상 아이템 리스트에 한 번에 넣기
            itemCountList.AddRange(_rewardIdList[_rewardId].itemCount);
        }
    }
}
