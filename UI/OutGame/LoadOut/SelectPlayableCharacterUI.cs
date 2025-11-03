using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayableCharacterUI : MonoBehaviour
{
    OutGameItem outGameItem;

    [SerializeField] private GameObject outLine;

    [SerializeField] private GameObject lockUI;
    [SerializeField] private GameObject lockIcon;

    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI lockText;
    // Start is called before the first frame update

    public void Init(OutGameItem outGameItem)
    {
        this.outGameItem = outGameItem;

        outLine.SetActive(outGameItem.isTake);
        lockIcon.SetActive(!outGameItem.isUnLock);

        lockText.text = outGameItem.unLockPoint.ToString();
        icon.sprite = Resources.Load<Sprite>(DataManager.Instance.Mercenary.GetMercenaryData(outGameItem.rewardId).gameObjectString);
    }

    public void OnClickSelect()
    {
        if (outGameItem?.isUnLock == false)
        {
            return;
        }
        var temp = OutGameItemManager.Instance.allOutGameItemList;
        for (int i = 0; i < temp.Count; i++)
        {
            if (temp[i].loadOutType == LoadOutType.Mercenary)
            {
                temp[i].isTake = false;
            }
        }
        outGameItem.isTake = true;
        outLine.SetActive(outGameItem.isTake);
        lockIcon.SetActive(!outGameItem.isUnLock);
    }
}
