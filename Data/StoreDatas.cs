using DataTable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreDatas : StoreData
{
    public StoreData GetStoreData(int idx)
    {
        return StoreDataList[idx];
    }

    public List<StoreData> GetRandomStoreDataByGroupID(int groupId)
    {
        var tempList = new List<StoreData>();
        var storeDataList = new List<StoreData>();
        tempList.AddRange(StoreDataList.Where(item => item.groupId == groupId));
        
        var count = tempList.Count;
        for (var i = 0; i < count; i++)
        {
            if (tempList[i].dropProb >= 1.0f || tempList[i].dropProb >= Random.value)
            {
                storeDataList.Add(tempList[i]);
            }
        }
        return storeDataList;
    }
}
