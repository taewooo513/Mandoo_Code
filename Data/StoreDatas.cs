using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDatas : StoreData
{
    public StoreData GetStoreData(int idx)
    {
        return StoreDataList[idx];
    }
}
