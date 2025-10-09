using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTreasureChestUI : ItemDragDropManager
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
        if (itemDrop != null)
        {
            for (int i = 0; i < itemDrop.Length; i++)
            {
                if (itemDrop[i] != null)
                {
                    itemDrop[i].UpdateUI(items[i]);
                }
            }
        }
    }

    public override void OnDrop()
    {
    }
    public override void OnDropSwap(int index1, int index2) 
    {
    }
    public override void OnDrop(int index, Item item) 
    {
    }
}
