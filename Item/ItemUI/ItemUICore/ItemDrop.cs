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
    public int SlotIndex { get => slotIndex; }

    private Image icon;
    private TextMeshProUGUI count;

    private void Awake()
    {
        itemDrag = GetComponentInChildren<ItemDrag>();
        if (itemDrag != null)
        {
            dragDropManager = GetComponentInParent<ItemDragDropManager>();
            count = GetComponentInChildren<TextMeshProUGUI>();
            icon = itemDrag.GetComponent<Image>();
        }
    }

    public bool CanDrop(IDraggingObject draggingObject)
    {
        if (draggingObject == null) return false;
        if (dragDropManager == null) return false;
        if ((ItemDrag)draggingObject == itemDrag) return false;

        var dragItem = (ItemDrag)draggingObject;
        if (dragItem.origin == DragOrigin.Equipment)
        {
            if (itemDrag != null && itemDrag.item != null)
                return false;
        }
        return true;
    }

    public bool Drop(IDraggingObject draggingObject)
    {
        if (!CanDrop(draggingObject)) return false;
        bool isDrop = dragDropManager.OnDrop(this, draggingObject);
        if (isDrop == true)
        {
            dragDropManager.UpdateUI();
        }
        return isDrop;
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggingObject = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<IDraggingObject>() : null;
        if (draggingObject == null) return;
    }

    public void Setting(ItemDragDropManager dragDropManager)
    {
        this.dragDropManager = dragDropManager;
    }

    public void UpdateUI(InGameItem item)
    {
        if (item == null)
        {
            itemDrag.Setting(null);
            if (count)
                count.text = "";
            icon.sprite = null;
            itemDrag.gameObject.SetActive(false);
            return;
        }
        itemDrag.Setting(item);
        icon.sprite = Resources.Load<Sprite>(item.GetItemInfo.iconPath);
        if (count)
        {
            if (DragOrigin.Equipment == itemDrag.origin)
            {
                count.text = item.GetItemInfo.name;
            }
            else
            {
                count.text = item.count.ToString();
            }
        }
        itemDrag.gameObject.SetActive(true);
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

/**
 *   public void OnBeginDrag(PointerEventData eventData)
    {
        var image = GetComponent<UnityEngine.UI.Image>();
        if (image == null || image.sprite == null) return;

        var im = InventoryManager.Instance;
        var item = im.GetItemInSlot(SlotIndex);
        if (item == null) return;

        origin = DragOrigin.Inventory;

        // 드래그 시작 시 현재 부모 Transform 저장
        original = transform.parent;
        transform.SetParent(baseCanvas.transform, true);
        transform.SetAsLastSibling();

        if (cg)
        {
            cg.blocksRaycasts = false; // 드래그 중에 슬롯들이 레이캐스트 받게끔 설정
            cg.alpha = 0.5f; // 투명도 설정
        }
        UpdatePosition(eventData);
    }

    /// <summary>
    /// 드래그 중일 때 호출
    /// </summary>
    public void OnDrag(PointerEventData eventData) => UpdatePosition(eventData);

    /// <summary>
    /// 드래그가 끝났을 때 호출
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        IDroppingTarget dropTarget = FindDroppingTarget(eventData);
        //bool drop = dropTarget != null && dropTarget.Drop(this);

        if (true)
        {
            transform.SetParent(original, false);
            var rt = GetComponent<RectTransform>();
            if (rt)
            {
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(100, 100);
                rt.localScale = Vector3.one;
            }
            owner?.RefreshSlots();
        }
        else
        {
            transform.SetParent(original, false);
            var rt = GetComponent<RectTransform>();
            if (rt)
            {
                rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = Vector2.zero;
                rt.sizeDelta = new Vector2(100, 100);
                rt.localScale = Vector3.one;
            }
        }

        if (cg)
        {
            cg.blocksRaycasts = true;
            cg.alpha = 1f;
        }
    }

    /// <summary>
    /// 드래그 중인 아이템의 위치를 업데이트
    /// </summary>
    /// <param name="eventData">포인터 이벤트 데이터</param>
    private void UpdatePosition(PointerEventData eventData)
    {
        if (!canvasRect || !rect) return;
        var camera = eventData.pressEventCamera != null ? eventData.pressEventCamera : baseCanvas.worldCamera;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, eventData.position, camera,
                out var local))
            rect.anchoredPosition = local; // + dragOffset;
    }

    private IDroppingTarget FindDroppingTarget(PointerEventData eventData)
    {
        hits.Clear();
        var gr = baseCanvas
            ? baseCanvas.GetComponentInParent<GraphicRaycaster>()
            : GetComponentInParent<GraphicRaycaster>();
        if (gr == null || EventSystem.current == null) return null;

        EventSystem.current.RaycastAll(eventData, hits);
        foreach (var hit in hits)
        {
            var target = hit.gameObject.GetComponentInParent<IDroppingTarget>();
            if (target != null) return target;
        }
        return null;
    }
}
 */