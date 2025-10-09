using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class BattleTreasureEvent : BaseRoom
{
    public BattleData battleData; //배틀데이터 데이터테이블
    public RewardData rewardData;

    protected List<int> equipItemIds = new(); //플레이어 죽었을 때, 가지고 있던 장비 아이템 저장하는 리스트. 복사본을 가져야되니 new로 생성

    public override void EnterRoom()
    {
        base.EnterRoom();
    }

    public override void ExitRoom()
    {
        base.ExitRoom();
    }

    public override void Init(int id)
    {
        base.Init(id);
        battleData = DataManager.Instance.Battle.GetBattleData(id); //배틀데이터 데이터테이블에 접근
        Debug.Log(battleData.rewardId);
        //rewardData = DataManager.Instance.Reward.GetRewardData(id); //보상 테이블 연결
    }

    public void PlayerDeadItem(List<int> id) //플레이어가 죽을 때 가지고 있던 아이템 리스트 todo : 플레이어가 템 초기화하기 전에 넘겨줘야 됨
    {
        equipItemIds = id;
    }
}
