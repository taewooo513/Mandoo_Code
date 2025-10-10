using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class ItemDragDropManager : UIBase // 이친구 인벤토리나 그런곳에서 상속받아서 사용하도록 합시다
{
    protected Item[] items;

    public abstract void UpdateUI();

    public abstract void OnDropSwap(int index1, int index2);
    public abstract void OnDrop();
    public abstract void OnDrop(int index, Item item);

}
