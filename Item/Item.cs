using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo
{
    public int id;
    public ItemType type;
    public string name;
    public string desc;
    public int maxCount;
    public int price;
    public string iconPath;
    public int weaponId;
    public int consumableId;

    public ItemInfo(int id)
    {
        var item = DataManager.Instance.Item.GetItemData(id);
        this.id = id;
        type = item.itemType;
        name = item.itemName;
        desc = item.itemDescription;
        maxCount = item.maxCount;
        price = item.price;
        iconPath = item.iconPathString;
        weaponId = item.weaponId;
        consumableId = item.consumableId;
    }

    public int AddItem(int amount)
    {
        return amount; // 남은개수 반환
    }
}
public class Item
{
    protected ItemInfo itemInfo;
    public ItemInfo GetItemInfo { get { return itemInfo; } }
    public int count;
    public virtual void InitItem(int id)
    {
        itemInfo = new ItemInfo(id);
    }

    public void UseItem()
    {
    }
}
