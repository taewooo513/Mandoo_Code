using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class MercenaryDatas : MercenaryData
{
    public MercenaryData GetMercenaryData(int idx)
    {
        if (!MercenaryDataMap.TryGetValue(idx, out MercenaryData data))
        {
            Debug.LogWarning($"[MercenaryDatas] ID {idx} not found in MercenaryDataMap!");
            return null;
        }
        return data;
    }

    public List<int> GetMercenaryIdList()
    {
        return new List<int>(MercenaryDataMap.Keys);
    }

    public bool HasKey(int idx)
    {
        return MercenaryDataMap.ContainsKey(idx);
    }
}
