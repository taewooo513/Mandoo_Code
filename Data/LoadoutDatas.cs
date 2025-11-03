using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadoutDatas : LoadoutData
{
    public List<LoadoutData> GetLoadoutDatas()
    {
        return LoadoutDataList;
    }
    public LoadoutData GetLoadoutData(int idx)
    {
        return LoadoutDataMap[idx];
    }
}
