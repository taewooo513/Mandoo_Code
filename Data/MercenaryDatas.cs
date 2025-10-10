using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class MercenaryDatas : MercenaryData
{
    public MercenaryData GetMercenaryData(int idx)
    {
        return MercenaryDataMap[idx];
    }
}
