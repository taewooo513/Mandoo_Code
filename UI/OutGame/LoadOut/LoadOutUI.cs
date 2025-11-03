using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadOutUI : UIBase
{
    [SerializeField]
    private GameObject _achievementUI;
    [SerializeField]
    private TextMeshProUGUI pointText;
    private LoadOutUISlot[] slots;
    [SerializeField]
    private GameObject selectUI;

    private void Awake()
    {
        slots = GetComponentsInChildren<LoadOutUISlot>();
    }
    protected override void OnOpen()
    {
        FilteringItem(1);
        pointText.text = OutGameItemManager.Instance.loadOutPoint.ToString();
    }

    public void OnClickActiveSelectPlayableCharacter()
    {
        selectUI.SetActive(true);
    }

    public void FilteringItem(int index)
    {
        int j = 0;
        for (int i = 0; i < OutGameItemManager.Instance.allOutGameItemList.Count; i++)
        {
            if (slots.Length > i)
            {
                if (OutGameItemManager.Instance.allOutGameItemList[i].loadOutType == (LoadOutType)index)
                {
                    slots[j].gameObject.SetActive(true);
                    OutGameItemManager.Instance.allOutGameItemList[i].CheckUnlockItem();
                    slots[j].Init(OutGameItemManager.Instance.allOutGameItemList[i]);
                    j++;
                }
                else
                {
                    slots[i].gameObject.SetActive(false);
                }
            }

        }
        for (; j < slots.Length; j++)
        {
            slots[j].gameObject.SetActive(false);
        }
    }

    public void OnClickStartButton()
    {
        AnalyticsManager.Instance.SendEventStep(30);
        UIManager.Instance.OpenUI<LevelChoiceUI>();
    }

    public void OnClickOpenAchievementUI()
    {
        _achievementUI.SetActive(true);
    }

    public void OnClickCloseAchievementUI()
    {
        _achievementUI.SetActive(false);
    }
}
