using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadOutUIShop : UIBase
{
    [SerializeField]
    private GameObject _achievementUI;

    private LoadOutUISlot[] slots;

    [SerializeField]
    private TextMeshProUGUI pointText;

    public GameObject infoSlot;

    protected override void OnOpen()
    {
        slots = GetComponentsInChildren<LoadOutUISlot>();
        UpdateUI();
        infoSlot.GetComponent<LoadoutInfoSlot>().HideInfoSlot();
    }

    public void UpdateUI()
    {
        pointText.text = AchievementManager.Instance.unLockPoint.ToString();
        for (int i = 0; i < slots.Length; i++)
        {
            if (OutGameItemManager.Instance.allOutGameItemList.Count > i)
            {
                slots[i].gameObject.SetActive(true);
                OutGameItemManager.Instance.allOutGameItemList[i].CheckUnlockItem();
                slots[i].Init2(OutGameItemManager.Instance.allOutGameItemList[i]);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnClickCloseAchievementUI()
    {
        _achievementUI.SetActive(false);
    }
    public void OnClickOpenAchievementUI()
    {
        _achievementUI.SetActive(true);
    }
}
