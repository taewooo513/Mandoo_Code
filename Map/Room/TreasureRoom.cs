using System.Collections;
using System.Collections.Generic;
using DataTable;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TreasureRoom : BattleTreasureEvent
{
    private GameObject _treasureChestPrefab; //상자 프리팹
    private int _rewardId; //실제로 주는 보상id
    private int _battleRewardGroupId; //배틀데이터에 있는 그룹 아이디
    private int _rewardGroupId; //보상 테이블 연결해주는 id
    private List<RewardData> _rewardIdList; //그룹에 속한 id 리스트
    List<int> itemIdList = new(); //보상id 리스트
    List<int> itemCountList = new(); //보상 개수 리스트

    public override void Init(int id)
    {
        base.Init(id);
        _battleRewardGroupId = battleData.rewardId;
        //_rewardGroupId = rewardData.groupId; //랜덤가챠 돌릴 범위 수정필요
        _rewardIdList = DataManager.Instance.Reward.GetRewardGroupList(_rewardGroupId); //보상 그룹 가져오기
        ChestRewardDecision(); //방에 들어왔을 때 보상 리스트 1개로 결정됨
    }

    public override void EnterRoom()
    {
        base.EnterRoom(); //플레이어 소환(위치 선정)
        OnEventEnded(); //상자를 열어도 되고, 안 열어도 되니 바로 버튼 활성화되도록 함
        Time.timeScale = 1f; //혹시몰라서 움직임 가능하게끔 함
        
        if (!isInteract) //상호작용을 안 했을 시
        {
            _treasureChestPrefab = spawn.TresureChestCreate(); //상자 생성
            //_treasureChest = _treasureChestPrefab.GetComponentInChildren<TreasureChest>(); //상자의 cs 가져옴
            //_treasureChest.Init(this);
            //_treasureChest.InitReward(itemIdList, itemCountList);

            Vector3 pos = new Vector3(3.5f, 0, 0);
            _treasureChestPrefab.transform.position = pos; //상자 생성해줌
        }
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

    public override void ExitRoom() //나갈 때
    {
        Clear(); //보상 id값들 넣어뒀던 리스트 비우기 (그룹id 안에 속한 id들 리스트)
        spawn.DestroyGameObject(_treasureChestPrefab); //상자 파괴
    }
    
    public void Clear() //리스트 내용 비워주기
    {
        _rewardIdList.Clear();
        itemIdList.Clear();
        itemCountList.Clear();
    }
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}
