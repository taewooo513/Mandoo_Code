using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatas : ItemData
{
    public ItemData GetItemData(int index)
    {
        if (ItemDataMap.TryGetValue(index, out ItemData data))
            return data;
        
        Debug.LogWarning($"ItemData not found: {index}");
        return null;
    }
}
