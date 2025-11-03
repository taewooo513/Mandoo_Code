using JetBrains.Annotations;
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

    public int consumableSkillId;
    public ConsumableType consumableType;
    public Skill skill;
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

        if (type == ItemType.Consumable)
        {
            var consumable = DataManager.Instance.Consumable.GetConsumableData(id);
            consumableSkillId = consumable.consumableSkillId;
            consumableType = consumable.consumableType;
            if (consumableSkillId > 0)
            {
                skill = new Skill();
                skill.Init(consumableSkillId);
            }
        }
    }
}
public class InGameItem
{
    protected ItemInfo itemInfo;
    public ItemInfo GetItemInfo { get { return itemInfo; } }
    public int count = 0;
    public virtual void InitItem(int id)
    {
        itemInfo = new ItemInfo(id);
    }

    public virtual void UseItem(int amount)
    {
        if (itemInfo.type == ItemType.Consumable && itemInfo.skill != null)
        {
            UseItemAction();
            UIManager.Instance.OpenUI<InGameUIManager>().OnClickSkillButton(itemInfo.skill, this);
            UIManager.Instance.GetUI<InGamePlayerUI>().UpdateUI();
        }
        else if (itemInfo.type == ItemType.Consumable && itemInfo.consumableType == ConsumableType.Map)
        {
            MapItem();
            RemoveItem(1);
        }
        else if (itemInfo.type == ItemType.Consumable && itemInfo.consumableType == ConsumableType.ExplorerSupport)
        {
            RemoveItem(amount);
        }
    }

    public void MapItem()
    {
        UIManager.Instance.OpenUI<MapUI>().ActivateEveryRoomIcon();
    }

    private void UseItemAction()
    {

    }

    public int GetItem(int amount)
    {
        int result = itemInfo.maxCount;
        count += amount;
        if (count > itemInfo.maxCount)
        {
            result = count - itemInfo.maxCount;
            count = itemInfo.maxCount;
            return result;
        }
        return 0;
    }
    public void RemoveItem(int amount)
    {
        count -= amount;
        if (count <= 0)
        {
            count = 0;
            InGameItemManager.Instance.RemoveItem(this);
        }
    }
}
