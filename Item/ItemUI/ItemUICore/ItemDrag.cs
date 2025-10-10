using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDraggingObject
{
    public DragOrigin Origin => origin;
    private DragOrigin origin;

    public int SlotIndex { get => slotIndex; }
    [SerializeField]
    private int slotIndex;

    public Item item { get => item; }
    public Transform original;
    private Item _item;
    private Canvas baseCanvas;
    private RectTransform rect;
    private RectTransform canvasRect;
    private CanvasGroup cg;
    private ItemDragDropManager itemDragDropManager;
    private List<RaycastResult> hits = new();

    private void Awake()
    {
        baseCanvas = GetComponentInParent<Canvas>();
        rect = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
        icon = GetComponent<Image>();
        itemDragDropManager = GetComponentInParent<ItemDragDropManager>();
        if (baseCanvas)
            canvasRect = baseCanvas.GetComponent<RectTransform>();
    }

    public void Setting(Item item)
    {
        _item = item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            return;
        }
        var image = GetComponent<UnityEngine.UI.Image>();

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
    public void OnDrag(PointerEventData eventData) => UpdatePosition(eventData);

    public void OnEndDrag(PointerEventData eventData)
    {
        IDroppingTarget dropTarget = FindDroppingTarget(eventData);
        bool drop = dropTarget != null && dropTarget.Drop(this);

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

            if (itemDragDropManager != null)
            {
                itemDragDropManager.UpdateUI();
            }
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
}
