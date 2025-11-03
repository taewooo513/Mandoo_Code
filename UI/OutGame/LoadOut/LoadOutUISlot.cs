using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadOutUISlot : MonoBehaviour
{
    private OutGameItem gameItem;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private GameObject buttonObject;
    [SerializeField] private GameObject img;
    [SerializeField] private GameObject img2;

    [SerializeField] private GameObject takeImgOn;
    [SerializeField] private GameObject takeImgOff;

    [SerializeField] private TextMeshProUGUI pointText;
    private LoadOutUIShop loadOutUIShop;
    
    private void Awake()
    {
        loadOutUIShop = GetComponentInParent<LoadOutUIShop>();
    }
    
    public void Init(OutGameItem gameItem)
    {
        this.gameItem = gameItem;
        countText.text = gameItem.requireLoadoutPoint.ToString();
        nameText.text = gameItem.name + " X " + gameItem.rewardCount.ToString();

        if (img != null)
            img.SetActive(!gameItem.isUnLock);
        if (img2 != null)
            img2.SetActive(!gameItem.isAchieveUnlock);

        if (takeImgOn != null)
        {
            takeImgOn.SetActive(gameItem.isTake);
            takeImgOff.SetActive(!gameItem.isTake);
        }
    }
    public void Init2(OutGameItem gameItem)
    {
        this.gameItem = gameItem;
        countText.text = gameItem.unLockPoint.ToString();
        nameText.text = gameItem.name + " X " + gameItem.rewardCount.ToString();

        if (img != null)
            img.SetActive(!gameItem.isUnLock);
        if (img2 != null)
            img2.SetActive(!gameItem.isAchieveUnlock);

        if (takeImgOn != null)
        {
            takeImgOn.SetActive(gameItem.isTake);
            takeImgOff.SetActive(!gameItem.isTake);
        }
    }

    public void OnClickButton()
    {
        if (gameItem.isAchieveUnlock)
            OnClickUnLockItem();
        loadOutUIShop.infoSlot.GetComponent<LoadoutInfoSlot>().SetInfoSlot(gameItem);
        loadOutUIShop.infoSlot.GetComponent<LoadoutInfoSlot>().ShowInfoSlot();
    }

    public void OnClickButton2()
    {
        if (gameItem.isUnLock)
            OnClickTakeItem();
    }

    private void OnClickUnLockItem()
    {
        if (!gameItem.isUnLock)
        {
            if (AchievementManager.Instance.RemoveUnLockPoint(gameItem.unLockPoint))
            {
                gameItem.isUnLock = true;
                DataManager.Instance.OnLoadOutBought(AchievementManager.Instance.unLockPoint, gameItem.id);
            }
        }
        GetComponentInParent<LoadOutUIShop>().UpdateUI();
        pointText.text = AchievementManager.Instance.unLockPoint.ToString();
        Init2(gameItem);
    }
    private void OnClickTakeItem()
    {
        if (gameItem.isTake)
        {
            OutGameItemManager.Instance.AddLoadOutPoint(gameItem.requireLoadoutPoint);
        }
        else if (!OutGameItemManager.Instance.RemoveLaodOutPoint(gameItem.requireLoadoutPoint))
        {
            return;
        }
        gameItem.isTake = !gameItem.isTake;
        pointText.text = OutGameItemManager.Instance.loadOutPoint.ToString();
        Init(gameItem);
    }
}
