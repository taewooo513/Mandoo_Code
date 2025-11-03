using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameVictoryUI : UIBase, ItemDragDropManager
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
        // TODO: 드래그하여 아이템 사라졌을 때 창 닫기
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

        if (!isActive)
        {
            gameObject.SetActive(isActive);
            GameManager.Instance.playerCanMove = true;
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
            GameManager.Instance.playerCanMove = true;
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
        gameObject.SetActive(false);
        GameManager.Instance.playerCanMove = true;
    }
}
