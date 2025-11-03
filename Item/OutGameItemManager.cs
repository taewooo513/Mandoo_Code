using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OutGameItem
{
    public int id;
    public string name;
    public int rewardId;
    public bool isAchieveUnlock = false;
    public int unLockPoint;
    public int requireLoadoutPoint;
    public LoadOutType loadOutType;
    public int rewardCount;
    public int unlockAchieveId;
    public bool isTake = false;
    public bool isUnLock = false;
    public string loadoutItemPath;
    public string description;
    public OutGameItem(int id)
    {
        var val = DataManager.Instance.LoadOut.GetLoadoutData(id);
        this.id = id;
        name = val.name;
        rewardId = val.rewardId;
        unLockPoint = val.unlockPoint;
        requireLoadoutPoint = val.requireLoadoutPoint;
        loadOutType = val.loadoutType;
        rewardCount = val.rewardCount;
        unlockAchieveId = val.unlockAchieveId;
        this.loadoutItemPath = val.loadoutIconPath;
        this.description = val.description;
    }

    public void CheckUnlockItem()
    {
        for (int i = 0; i < AchievementManager.Instance.GetCompletedAchievement().Count; i++)
        {
            if (unlockAchieveId == i || unlockAchieveId == 0)
            {
                isAchieveUnlock = true;
            }
        }
    }
}
public class OutGameItemManager : Singleton<OutGameItemManager>
{
    public int loadOutPoint;

    public List<OutGameItem> allOutGameItemList { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        loadOutPoint = 5;
        allOutGameItemList = new List<OutGameItem>();
    }

    public void AddLoadOutPoint(int val)
    {
        loadOutPoint += val;
    }
    public bool RemoveLaodOutPoint(int val)
    {
        bool result = loadOutPoint - val >= 0;
        if (result)
        {
            loadOutPoint -= val;
        }
        return result;
    }

    private void Start()
    {
        for (int i = 0; i < DataManager.Instance.LoadOut.GetLoadoutDatas().Count; i++)
        {
            OutGameItem outGameItem = new OutGameItem(DataManager.Instance.LoadOut.GetLoadoutDatas()[i].id);
            allOutGameItemList.Add(outGameItem);
            if (DataManager.Instance.SaveData.LoadOutSave != null)
            {
                var val = DataManager.Instance.SaveData.LoadOutSave.CurrentLoadOutID.Contains(outGameItem.id);
                if (val)
                {
                    outGameItem.isTake = true;
                    RemoveLaodOutPoint(outGameItem.requireLoadoutPoint);
                }
                var val2 = DataManager.Instance.SaveData.LoadOutSave.LoadOutID.Contains(outGameItem.id);
                if (val2)
                {
                    outGameItem.isUnLock = true;
                }
            }
        }
    }

    public void StartGameItemInput()
    {
        List<int> res = new List<int>();
        for (int i = 0; i < allOutGameItemList.Count; i++)
        {
            if (allOutGameItemList[i].isTake == true)
            {
                res.Add(allOutGameItemList[i].id);
                InGameItemManager.Instance.PushBackItem(InGameItemManager.Instance.AddItem(allOutGameItemList[i].rewardId, allOutGameItemList[i].rewardCount));
            }
        }
        DataManager.Instance.OnLoadOutChanged(res);
    }
}
