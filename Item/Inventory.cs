using UnityEngine;

public class Inventory
{
    private InGameItem[] items;
    public Inventory()
    {
        items = new InGameItem[10];
    }

    public void ClearItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = null;
        }
    }

    public InGameItem GetItem(int index)
    {
        Debug.Log(index);
        return items[index];
    }
    public InGameItem[] GetItems()
    {
        return items;
    }

    public InGameItem FindItem(InGameItem item)
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

    public void AddItem(InGameItem item, int index)
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

    public void SawpItem(InGameItem item1, InGameItem item2)
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

    public void MergeItem(int index1, int index2)
    {
        int val = items[index1].GetItem(items[index2].count);
        items[index2].RemoveItem(items[index2].count - val);
    }

    public void MergeItem(InGameItem item1, InGameItem item2)
    {
        int val = item1.GetItem(item2.count);
        item2.RemoveItem(item2.count - val);
    }

    public void UseItem(InGameItem item, int amount)
    {
        item.UseItem(amount);
    }

    public void RemoveItem(InGameItem item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (item == items[i])
            {
                items[i] = null;
                return;
            }
        }
    }

    public void UseGold(int amount)
    {
        int val = amount;
        if (IsUseGold(amount))
        {
            for (int i = items.Length - 1; i >= 0; i--)
            {
                if (items[i] != null)
                {
                    if (items[i].GetItemInfo.type == ItemType.Gold)
                    {
                        int temp = items[i].count;
                        items[i].RemoveItem(val);
                        val -= temp;
                        if (val <= 0)
                        {
                            return;
                        }
                    }
                }
            }
        }
    }

    public bool PushBackItem(InGameItem item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                if (items[i].GetItemInfo.id == item.GetItemInfo.id)
                {
                    MergeItem(items[i], item);
                    if (item == null)
                    {
                        return true;
                    }
                    else
                    {
                        if (item.count == 0)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                return true;
            }
        }
        return false;
    }

    public bool IsUseGold(int amount)
    {
        if (GetTotalCoin() < amount)
        {
            return false;
        }
        return true;
    }
    public int GetTotalCoin()
    {
        int totalCoin = 0;
        foreach (var item in items)
        {
            if (item != null)
            {
                if (item.GetItemInfo.type == ItemType.Gold)
                {
                    totalCoin += item.count;
                }
            }
        }
        return totalCoin;
    }
    public InGameItem BoomBreakToolCheck()
    {
        InGameItem item = null;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                if (items[i].GetItemInfo.type == ItemType.Consumable)
                {
                    if (items[i].GetItemInfo.consumableType == ConsumableType.ExplorerSupport)
                    {
                        item = items[i];
                    }
                }
            }
        }
        return item;
    }
}
