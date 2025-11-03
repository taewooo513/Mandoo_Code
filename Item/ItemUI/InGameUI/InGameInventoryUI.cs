using UnityEngine;

public class InGameInventoryUI : UIBase, ItemDragDropManager
{
    InGameItem[] items;
    public InGameItem[] GetItems => items;
    [SerializeField]
    private ItemDrag[] itemDrag;

    [SerializeField]
    private ItemDrop[] itemDrop;

    private int originalSortingIndex;
    private bool isSorted;

    public void RemoveItem(InGameItem item)
    {

    }
    private void Awake()
    {
        itemDrag = GetComponentsInChildren<ItemDrag>();
        itemDrop = GetComponentsInChildren<ItemDrop>();
        originalSortingIndex = -1;

        foreach (var item in itemDrag)
        {
            if (item != null)
            {
                item.OnPointerEnterEvent += OnItemDragPointerEnter;
                item.OnPointerExitEvent += OnItemDragPointerExit;
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var item in itemDrag)
        {
            if (item != null)
            {
                item.OnPointerEnterEvent -= OnItemDragPointerEnter;
                item.OnPointerExitEvent -= OnItemDragPointerExit;
            }
        }
    }
    private void OnItemDragPointerEnter()
    {
        if (!isSorted)
        {
            originalSortingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            isSorted = true;
        }
    }

    private void OnItemDragPointerExit()
    {
        if (isSorted && originalSortingIndex != -1)
        {
            transform.SetSiblingIndex(originalSortingIndex);
            isSorted = false;
        }
    }
    protected override void OnOpen()
    {
        Setting(InGameItemManager.Instance.GetItems());
        UpdateUI();
        UIManager.Instance.RaiseUIVisibilityChanged(this, true);
    }

    protected override void OnClose()
    {
        base.OnClose();
        UIManager.Instance.RaiseUIVisibilityChanged(this, false);
    }

    public void Setting(InGameItem[] items)
    {
        this.items = items;
    }

    public void UpdateUI()
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
    public void OnDropSwap(int index1, int index2)
    {
        InGameItemManager.Instance.SwapItem(index1, index2);
    }
    public void OnDrop()
    {
    }
    public bool OnDrop(ItemDrop drop, IDraggingObject draggingObject)
    {
        if (draggingObject.item == null)
        {
            return false;
        }
        switch (draggingObject.Origin)
        {
            case DragOrigin.Inventory:
                if (InGameItemManager.Instance.IsEqualItem(drop.SlotIndex, draggingObject.SlotIndex))
                {
                    InGameItemManager.Instance.MergeItem(drop.SlotIndex, draggingObject.SlotIndex);
                }
                else
                {
                    InGameItemManager.Instance.SwapItem(drop.SlotIndex, draggingObject.SlotIndex);
                }
                UpdateUI();
                return true;
            case DragOrigin.Reward:
                if (InGameItemManager.Instance.GetItem(drop.SlotIndex) == null)
                {
                    InGameItemManager.Instance.InsertItem(draggingObject.item, drop.SlotIndex);
                    draggingObject.DraggingManager.RemoveItem(draggingObject.item);
                }
                else
                {
                    if (items[drop.SlotIndex].GetItemInfo.id == draggingObject.item.GetItemInfo.id)
                    {
                        InGameItemManager.Instance.MergeItem(items[drop.SlotIndex], draggingObject.item);
                        if (draggingObject.item.count == 0)
                        {
                            draggingObject.DraggingManager.RemoveItem(draggingObject.item);
                        }
                    }
                }
                draggingObject.DraggingManager.UpdateUI();
                UpdateUI();
                return true;
            case DragOrigin.Equipment:
                InGameItemManager.Instance.InsertItem(draggingObject.item, drop.SlotIndex);
                ((Weapon)draggingObject.DraggingManager.GetItems[draggingObject.SlotIndex]).Downgrad();
                draggingObject.DraggingManager.GetItems[draggingObject.SlotIndex] = null;
                draggingObject.DraggingManager.UpdateUI();
                ((InGameEquipmentUI)draggingObject.DraggingManager).UnEquip(draggingObject.SlotIndex == 0 ? true : false);
                UpdateUI();
                return true;
            default:
                return false;
        }
    }
}
