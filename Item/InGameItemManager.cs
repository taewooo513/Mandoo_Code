using UnityEngine;

public class InGameItemManager : Singleton<InGameItemManager>
{
    private Inventory inventory;
    
    protected override void Awake()
    {
        base.Awake();
        inventory = new Inventory();
    }

    public void SellItem(int slotIndex)
    {
        var item = inventory.GetItems()[slotIndex];
        if (item != null)
        {
            if (item.GetItemInfo.type != ItemType.Gold)
            {
                PushBackItem(AddItem(1001, (int)((float)(item.GetItemInfo.price * item.count) * 0.75f)));
                RemoveItem(slotIndex);
            }
        }
    }

    private void Update()
    {
        AchievementManager.Instance.SetParam("totalGoldPerInGame", inventory.GetTotalCoin());
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.G))
        {
            InsertItem(AddItem(3002, 1));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            InsertItem(AddItem(2001, 3));
            InsertItem(AddItem(2002, 3));
            InsertItem(AddItem(2003, 3));
            InsertItem(AddItem(40011, 1));
            InsertItem(AddItem(40021, 1));
            InsertItem(AddItem(50010, 1));
        }
#endif
    }

    public void ClearInventory()
    {
        if (inventory == null)
        {
            return;
        }
        inventory.ClearItem();
    }

    public InGameItem GetItem(int index)
    {
        if (inventory == null)
        {
            return null;
        }
        return inventory.GetItem(index);
    }
    public InGameItem[] GetItems()
    {
        if (inventory == null)
        {
            return null;
        }
        return inventory.GetItems();
    }

    public bool IsEqualItem(int index1, int index2)
    {
        if (inventory == null)
        {
            return false;
        }
        if (inventory.GetItem(index2) == null)
            return false;

        else if (inventory.GetItem(index1) == null)
            return false;
        return inventory.GetItem(index1).GetItemInfo.id == inventory.GetItem(index2).GetItemInfo.id;
    }

    public InGameItem AddItem(int id, int amount)
    {
        if (id < 40000)
        {
            InGameItem item = new InGameItem();
            item.InitItem(id);
            item.count = amount;
            return item;
        }
        else
        {
            Weapon item = new Weapon();
            item.InitItem(id);
            item.count = amount;
            return item;
        }
    }

    public InGameItem AddWeapon(int id)
    {
        Weapon item = new Weapon();
        item.InitItem(id);
        return item;
    }

    public void UseGold(int amount)
    {
        inventory.UseGold(amount);
    }

    public bool IsUseGold(int amount)
    {
        return inventory.IsUseGold(amount);
    }


    public void InsertItem(InGameItem item) // 비어있는곳중 앞에있는곳에 추가
    {
        if (inventory == null)
        {
            return;
        }
        inventory.PushBackItem(item);
        UIManager.Instance.CloseUI<MapUI>();
        UIManager.Instance.OpenUI<InGameInventoryUI>().UpdateUI();
    }

    public void InsertItem(InGameItem item, int index) // 인덱스에 직접추가
    {
        if (inventory == null)
        {
            return;
        }
        inventory.AddItem(item, index);
        UIManager.Instance.CloseUI<MapUI>();
        UIManager.Instance.OpenUI<InGameInventoryUI>().UpdateUI();
    }
    public void MergeItem(int index1, int index2)
    {
        if (inventory == null)
        {
            return;
        }
        inventory.MergeItem(index1, index2);
    }

    public void MergeItem(InGameItem item1, InGameItem item2)
    {
        if (inventory == null)
        {
            return;
        }
        inventory.MergeItem(item1, item2);
    }

    public void SwapItem(int index1, int index2)
    {
        if (inventory == null)
        {
            return;
        }
        inventory.SwapItem(index1, index2);
    }

    public void RemoveItem(int index)
    {
        if (inventory == null)
        {
            return;
        }
        inventory.RemoveItem(GetItem(index));
        UIManager.Instance.UIGet<InGameInventoryUI>().UpdateUI();
    }
    public bool PushBackItem(InGameItem item) //인벤ui에 띄워주기
    {
        if (inventory == null)
        {
            return false;
        }
        return inventory.PushBackItem(item);
    }
    public void RemoveItem(InGameItem item)
    {
        if (inventory == null)
        {
            return;
        }
        inventory.RemoveItem(item);
        item = null;
    }

    public InGameItem BoomBreakToolCheck()
    {
        if (inventory == null)
        {
            return null;
        }
        return inventory.BoomBreakToolCheck();
    }
}
