using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatas : ItemData
{
    public ItemData GetItemData(int index)
    {
        return ItemDataList[index];
    }
}
