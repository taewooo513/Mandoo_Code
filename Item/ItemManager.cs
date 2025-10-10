using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    private Inventory inventory;

    private void Awake()
    {
        base.Awake();
        inventory = new Inventory();
    }

    public void AddItme(Item item) // 비어있는곳중 앞에있는곳에 추가
    {
        inventory.FindItem(item);
    }

    public void AddItem(Item item, int index) // 인덱스에 직접추가
    {
        inventory.AddItem(item, index);
    }

    public void SwapItem(int index1, int index2)
    {
        inventory.SwapItem(index1, index2);
    }
}
