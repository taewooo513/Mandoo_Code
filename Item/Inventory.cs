using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private Item[] items;
    public Inventory()
    {
        items = new Item[10];
    }

    public Item GetItem(int index)
    {
        return items[index];
    }

    public Item FindItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                if (items[i] == item)
                {
                    return items[i];
                }
            }
        }
        return null;
    }

    public void AddItem(Item item, int index)
    {
        if (items[index] == null)
        {
            items[index] = item;
        }
    }

    public void SwapItem(int idx1, int idx2)
    {
        (items[idx1], items[idx2]) = (items[idx2], items[idx1]);
    }

    public void SawpItem(Item item1, Item item2)
    {
        for (int i = 0; i < items.Length; i++)
        {
            for (int j = 0; j < items.Length; j++)
            {
                if (items[i] == item1 && items[j] == item2)
                {
                    (items[i], items[j]) = (items[j], items[i]);
                    return;
                }
            }
        }
    }

    public void MergeItem(Item targetItem, Item addItem)
    {

    }

    public void UseItem(Item item)
    {
        item.UseItem();
    }
}
