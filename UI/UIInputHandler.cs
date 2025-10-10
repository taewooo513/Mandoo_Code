using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary>
/// UI 입력을 처리하는 클래스
/// </summary>
public class UIInputHandler : UIBase
{
    [SerializeField] private GraphicRaycaster raycaster;      // UI 레이캐스트를 위한 컴포넌트
    [SerializeField] private InputActionAsset inputActions;    // 입력 액션 에셋
   // [SerializeField] private InGameInventoryUI inventoryUI;    // 인벤토리 UI 참조

    private InputActionMap map;       // UI 관련 입력 액션 맵
    private InputAction use;          // 아이템 사용 입력
    private InputAction drop;         // 아이템 버리기 입력

    private readonly List<RaycastResult> hits = new();    // 레이캐스트 결과 저장 리스트

    /// <summary>
    /// 컴포넌트 초기화시 호출되는 메서드
    /// </summary>
    private void Awake()
    {
        // 레이캐스터가 없으면 부모에서 찾아서 설정
        if (raycaster == null)
            raycaster = GetComponentInParent<GraphicRaycaster>(true);
        // 인벤토리 UI가 없으면 캔버스에서 찾아서 설정
        //if (inventoryUI == null)
        //{
        //    var canvas = GetComponentInParent<Canvas>(true);
        //    if (canvas != null)
        //        inventoryUI = canvas.GetComponentInChildren<InGameInventoryUI>(true);
        //}

        // 입력 액션 설정
        map = inputActions.FindActionMap("UI");
        use = map.FindAction("UseItem");
        drop = map.FindAction("DropItem");
    }

    /// <summary>
    /// 컴포넌트가 활성화될 때 호출되는 메서드
    /// </summary>
    //private void OnEnable()
    //{
    //    map.Enable();
    //    use.performed += OnUse;
    //    drop.performed += OnDrop;
    //}

    ///// <summary>
    ///// 컴포넌트가 비활성화될 때 호출되는 메서드
    ///// </summary>
    //private void OnDisable()
    //{
    //    use.performed -= OnUse;
    //    drop.performed -= OnDrop;
    //    map.Disable();
    //}

    /// <summary>
    /// 마우스 포인터 위치의 인벤토리 슬롯을 가져오는 메서드
    /// </summary>
    /// <returns>선택된 인벤토리 슬롯 UI</returns>
    //private InventorySlotUI GetSlot()
    //{
    //    // UI 레이캐스팅이 불가능한 경우 null 반환
    //    if (raycaster == null || EventSystem.current == null)
    //        return null;

    //    // 마우스 커서 위치로 레이캐스트할 이벤트 데이터 생성
    //    var pointer = new PointerEventData(EventSystem.current);
    //    pointer.position = Mouse.current.position.ReadValue();

    //    // 이전 레이캐스트 결과 초기화
    //    hits.Clear();
    //    // UI 레이캐스트 수행
    //    raycaster.Raycast(pointer, hits);

    //    // 레이캐스트 결과에서 InventorySlotUI 컴포넌트 검색
    //    foreach (var hit in hits)
    //    {
    //        var slot = hit.gameObject.GetComponentInParent<InventorySlotUI>();
    //        if (slot != null)
    //            return slot;
    //    }
    //    return null;
    //}

    /// <summary>
    /// 아이템 사용 입력 처리 메서드
    /// </summary>
    //private void OnUse(InputAction.CallbackContext context)
    //{
    //    var slot = GetSlot();
    //    if (slot == null) return;

    //    //var item = InventoryManager.Instance.GetItemInSlot(slot.SlotIndex);
    //    //if (item == null) return;

    //    //if (InventoryManager.Instance.UseItemFromSlot(slot.SlotIndex, 1))
    //        inventoryUI.RefreshSlots();
    //}

    /// <summary>
    /// 아이템 버리기 입력 처리 메서드
    /// </summary>
    //private void OnDrop(InputAction.CallbackContext context)
    //{
    //    var slot = GetSlot();
    //    if (slot == null) return;

    //    var item = InventoryManager.Instance.GetItemInSlot((slot.SlotIndex));
    //    if (item == null) return;

    //    //if (InventoryManager.Instance.RemoveItemFromSlot(slot.SlotIndex, 1)) // TODO: 인자값 맞는지 확인 필요
    //    inventoryUI.RefreshSlots();
    //}
}