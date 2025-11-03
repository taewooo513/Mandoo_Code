using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public interface ItemDragDropManager // 이친구 인벤토리나 그런곳에서 상속받아서 사용하도록 합시다
{
    InGameItem[] GetItems { get; }

    void UpdateUI();
    void OnDropSwap(int index1, int index2);
    void OnDrop();
    bool OnDrop(ItemDrop drop, IDraggingObject draggingObject);
    public void RemoveItem(InGameItem item);
}
