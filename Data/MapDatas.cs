using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;
public class MapDatas : MapData
{
    public MapData GetMapData(int idx)
    {
        return MapDataMap[idx];
    }
}
