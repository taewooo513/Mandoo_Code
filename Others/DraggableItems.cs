using UnityEngine;

public enum DragOrigin
{
    Inventory,
    Equipment,
    Reward,
}

public interface IDraggingObject
{
    DragOrigin Origin { get; }
    int SlotIndex { get; }
    InGameItem item { get; }
    ItemDragDropManager DraggingManager { get; }
}

public interface IDroppingTarget
{
    bool CanDrop(IDraggingObject draggingObject); // 드롭 할 수 있는지 확인
    bool Drop(IDraggingObject draggingObject); // 드롭 처리
    Transform RootObject { get; } // 알맞게 슬롯에 배치해줄 상위의 오브젝트
}