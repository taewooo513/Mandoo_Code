using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameTreasureChestUI : UIBase, ItemDragDropManager
{
    InGameItem[] items;
    public InGameItem[] GetItems => items;

    [SerializeField]
    private ItemDrag[] itemDrag;
    [SerializeField]
    private ItemDrop[] itemDrop;

    private void Awake()
    {
        itemDrag = GetComponentsInChildren<ItemDrag>();
        itemDrop = GetComponentsInChildren<ItemDrop>();
        if (itemDrag != null)
        {
            foreach (var item in itemDrag)
            {
                item.origin = DragOrigin.Reward;
            }
        }
    }

    public void Setting(InGameItem[] items)
    {
        this.items = items;
        UpdateUI();
    }

    public void UpdateUI()
    {
        // 드래그 앤 드랍 또는 모두 수령을 통해 아이템을 모두 수령한 경우 창 닫기
        // 하지만 이 코드가 여기에 있으면 안되지만 일단 귀찮으니 여기 넣어둠.
        bool isActive = false;
        if (itemDrop != null)
        {
            for (int i = 0; i < itemDrop.Length; i++)
            {
                if (itemDrop[i] != null)
                {
                    if (items[i] == null)
                    {
                        itemDrop[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        isActive = true;
                        itemDrop[i].gameObject.SetActive(true);
                        itemDrop[i].UpdateUI(items[i]);
                    }
                }
            }
        }
        if(!isActive)
        {
            AnalyticsManager.Instance.SendEventStep(19);
            gameObject.SetActive(isActive);
            GameManager.Instance.playerObjectInteract = false;
        }
    }

    public void OnDrop()
    {
    }

    public bool OnDrop(ItemDrop drop, IDraggingObject draggingObject)
    {
        return true;
    }

    public void OnDropSwap(int index1, int index2)
    {
    }

    public void OnAllGetItemButton()
    {
        for (int i = 0; i < items.Length;)
        {
            if (items[i] != null)
            {
                if (InGameItemManager.Instance.PushBackItem(items[i]))
                {
                    RemoveItem(items[i]);
                }
                else
                {
                    i++;
                }
            }
            else
            {
                i++;
            }
        }
        bool isActive = false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
            {
                isActive = true;
                break;
            }
        }
        gameObject.SetActive(isActive);
        UIManager.Instance.CloseUI<MapUI>();
        UIManager.Instance.OpenUI<InGameInventoryUI>().UpdateUI();
        if (!isActive)
        {
            GameManager.Instance.playerObjectInteract = false;
        }

        UpdateUI();
    }
    public void RemoveItem(InGameItem item)
    {
        int startIndex = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                startIndex = i;
                items[i] = null;
                break;
            }
        }// 인덱스비우고

        for (int i = startIndex; i < items.Length; i++)
        {
            if (i + 1 < items.Length)
            {
                items[i] = items[i + 1];
            }
        }// 앞으로 당겨주는 작업
        items[items.Length - 1] = null;
    }

    public void OnCloseUIButton()
    {
        // 만약 튜토리얼 진행 중 수령 안하고 닫았을 때에 대한 처리
        if(AnalyticsManager.Instance.Step == 18)
            AnalyticsManager.Instance.SendEventStep(19);
        gameObject.SetActive(false);
        GameManager.Instance.playerObjectInteract = false;
    }
}
