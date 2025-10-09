using DataTable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleDatas : BattleData
{
    public BattleData GetBattleData(int idx)
    {
        return BattleDataMap[idx];
    }

    public int GetRandomDataIDByType(EventType eventType)
    {
        var filteredList = BattleDataList.Where(x => x.type == eventType).ToList();
        return filteredList.Count > 0 ? filteredList[Random.Range(0, filteredList.Count)].id : -1;
    }
}
