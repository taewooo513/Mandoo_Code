using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Image;
using System;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDraggingObject, IPointerEnterHandler, IPointerExitHandler
{
    public DragOrigin Origin => origin;
    public DragOrigin origin;

    public int SlotIndex { get => slotIndex; }
    [SerializeField]
    private int slotIndex;

    public InGameItem item { get => _item; }
    public Transform original;
    private InGameItem _item;
    private Canvas baseCanvas;
    private RectTransform rect;
    private RectTransform canvasRect;
    private CanvasGroup cg;
    private ItemDragDropManager itemDragDropManager;
    private List<RaycastResult> hits = new();
    public ItemDragDropManager DraggingManager { get => itemDragDropManager; }
    [SerializeField] private GameObject tooltip;
    private Tooltip tooltipScript;

    public event Action OnPointerEnterEvent;
    public event Action OnPointerExitEvent;

    private void Awake()
    {
        baseCanvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        itemDragDropManager = GetComponentInParent<ItemDragDropManager>();
        if (baseCanvas)
            canvasRect = baseCanvas.GetComponent<RectTransform>();

        if (tooltip != null)
        {
            tooltipScript = tooltip.GetComponent<Tooltip>();
            tooltipScript.HideTooltip();
        }

    }

    private void OnEnable()
    {
        BattleManager.Instance.OnTurnEnded += ForceClose;
    }
    
    private void OnDisable()
    {
        BattleManager.Instance.OnTurnEnded -= ForceClose;
    }
    
    private void ForceClose()
    {
        tooltipScript.HideTooltip();
        OnPointerExitEvent?.Invoke();
    }
    
    public void Setting(InGameItem item)
    {
        _item = item;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke();
        if (_item != null)
        {
            string itemName = _item.GetItemInfo.name;
            string itemDesc = _item.GetItemInfo.desc;
            if (tooltipScript != null)
            {
                tooltipScript.SetTooltipText(itemName, itemDesc);
                tooltipScript.SetSortingOrder();
                tooltipScript.ShowTooltip();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke();
        if (tooltipScript != null)
            tooltipScript.HideTooltip();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item == null)
        {
            return;
        }
        var image = GetComponent<UnityEngine.UI.Image>();

        // 드래그 시작 시 현재 부모 Transform 저장
        if (tooltipScript != null)
            tooltipScript.HideTooltip();
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
    public void OnDrag(PointerEventData eventData)
    {
        UpdatePosition(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (BattleManager.Instance.isBattleStarted && _item.GetItemInfo.type == ItemType.Equipment)
        {
            // TODO: 배틀 시작하면 한번은 장착/해제 가능하게끔 구현
            ResetToOriginalPosition();
            return;
        }
        
        IDroppingTarget dropTarget = FindDroppingTarget(eventData);
        bool drop = dropTarget != null && dropTarget.Drop(this);
        //
        // if (origin == DragOrigin.Equipment && dropTarget == null)
        // {
        //     dropTarget.Drop(this);
        // }
        //
        transform.SetParent(original, false);
        var rt = GetComponent<RectTransform>();
        if (rt)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            if (origin == DragOrigin.Equipment) rt.anchoredPosition = new Vector2(0, -20);
            else rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(100, 100);
            rt.localScale = Vector3.one;
        }

        if (itemDragDropManager != null)
        {
            itemDragDropManager.UpdateUI();
        }

        if (cg)
        {
            cg.blocksRaycasts = true;
            cg.alpha = 1f;
        }
    }

    private void ResetToOriginalPosition()
    {
        transform.SetParent(original, false);
        var rt = GetComponent<RectTransform>();
        if (rt)
        {
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            if (origin == DragOrigin.Equipment) rt.anchoredPosition = new Vector2(0, -20);
            else rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(100, 100);
            rt.localScale = Vector3.one;
        }

        if (cg)
        {
            cg.blocksRaycasts = true;
            cg.alpha = 1f;
        }
    }

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

    public void OnClickUseItem()
    {
        if (origin == DragOrigin.Inventory)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                InGameItemManager.Instance.RemoveItem(slotIndex);
            }
            else
            {
                if (GameManager.Instance.isShop == false)
                {
                    if (AnalyticsManager.Instance.Step == 12)
                        AnalyticsManager.Instance.SendEventStep(13);
                    if (BattleManager.Instance.isBattleStarted == BattleManager.Instance.isEndTurn)
                    {
                        if (!BattleManager.Instance.isUseItem)
                        {
                            item.UseItem(1);
                        }
                    }
                }
                else
                {
                    AchievementManager.Instance.SetParam("sellItem", 1);
                    InGameItemManager.Instance.SellItem(slotIndex);
                }
            }
            tooltipScript.HideTooltip();
            itemDragDropManager.UpdateUI();
        }
    }
}
