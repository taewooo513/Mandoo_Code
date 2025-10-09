using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameInventoryUI : ItemDragDropManager
{
    [SerializeField]
    private ItemDrag[] itemDrag;

    [SerializeField]
    private ItemDrop[] itemDrop;

    private void Awake()
    {
        itemDrag = GetComponentsInChildren<ItemDrag>();
        itemDrop = GetComponentsInChildren<ItemDrop>();
    }

    public void Setting(Item[] items)
    {
        this.items = items;
    }

    public override void UpdateUI()
    {
        if (itemDrag != null)
        {
            for (int i = 0; i < itemDrag.Length; i++)
            {
                if (itemDrag[i] != null)
                {
                    itemDrop[i].UpdateUI(items[i]);
                }
            }
        }
    }

    public override void OnDropSwap(int index1, int index2)
    {
        ItemManager.Instance.SwapItem(index1, index2);
    }

    public override void OnDrop()
    {
    }
    public override void OnDrop(int index, Item item)
    {

    }
}
