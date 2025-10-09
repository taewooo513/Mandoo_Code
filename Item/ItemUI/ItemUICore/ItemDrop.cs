using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour, IDroppingTarget, IDropHandler
{
    public Transform RootObject => throw new System.NotImplementedException();
    private ItemDragDropManager dragDropManager;
    private ItemDrag itemDrag;

    [SerializeField]
    private int slotIndex;

    private Image icon;
    private TextMeshProUGUI count;

    private void Awake()
    {
        itemDrag = GetComponentInChildren<ItemDrag>();
        if (itemDrag != null)
        {
            count = GetComponent<TextMeshProUGUI>();
            icon = itemDrag.GetComponent<Image>();
        }
    }

    public bool CanDrop(IDraggingObject draggingObject)
    {
        if (draggingObject == null) return false;
        if (dragDropManager == null) return false;
        dragDropManager.OnDropSwap(draggingObject.SlotIndex, draggingObject.SlotIndex);

        return false;
    }

    public bool Drop(IDraggingObject draggingObject)
    {
        if (!CanDrop(draggingObject)) return false;

        switch (draggingObject.Origin)
        {
            case DragOrigin.Inventory:
                return true;
            case DragOrigin.Reward:
                return true;
            case DragOrigin.Equipment:
                return true;
            default:
                return false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedItem = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<ItemDrag>() : null;
        if (draggedItem == null) return;

        // 이동할 출발지와 목적지 슬롯 인덱스 설정
        int from = draggedItem.SlotIndex;
        int to = slotIndex;

        if (owner != null && owner.isTestMode)
        {
            var targetContainer = this.transform;
            var sourceContainer = draggedItem.original;

            var existingIcon = targetContainer.GetComponentInChildren<ItemDrag>(true);
            if (existingIcon != null && existingIcon != draggedItem && sourceContainer != null)
            {
                existingIcon.transform.SetParent(sourceContainer, false);
                ResetRectTransform(existingIcon.GetComponent<RectTransform>());
                existingIcon.Setup(from, owner, owner.baseCanvas);
            }

            draggedItem.transform.SetParent(targetContainer, false);
            ResetRectTransform(draggedItem.GetComponent<RectTransform>());
            draggedItem.Setup(to, owner, owner.baseCanvas);

            return;
        }
    }

    public void Setting(ItemDragDropManager dragDropManager)
    {
        this.dragDropManager = dragDropManager;
    }

    public void UpdateUI(Item item)
    {
        if (item == null)
        {
            itemDrag.gameObject.SetActive(false);
            return;
        }
        itemDrag.Setting(item);
        icon.sprite = Resources.Load<Sprite>(item.GetItemInfo.iconPath);
        count.text = item.count.ToString();
    }
    public void ResetRectTransform(RectTransform rt)
    {
        if (!rt) return;

        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = new Vector2(100, 100);
        rt.localScale = Vector3.one;
    }
}
