using DataTable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardDatas : RewardData
{
    public RewardData GetRewardData(int idx)
    {
        return RewardDataMap[idx];
    }

    public List<RewardData> GetRewardGroupList(int groupId) //데이터 테이블에 적어둔 것들 중에서, 특정 groupId만 읽어서 리스트로 반환하는 메서드
    {
        return RewardDataMap.Values //모든 보상 데이터
            .Where(r => r.groupId == groupId) //특정 그룹만 골라냄
            .ToList(); //리스트로 반환
    }
}