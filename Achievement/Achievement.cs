using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 업적 데이터
public class AchievementInfo
{
    public int id;
    public string name;
    public string description;
    public string param;
    public int requirementsValue;
    public string rewardImagePath;
    public int addUnlockPoint;
    public string requirements;
    public bool isComplete = false;
    public string achieveImgPath;
    public AchievementInfo(int id)
    {
        this.id = id;
        Achievements achievements = DataManager.Instance._achievement.GetAchievementData(id);
        this.name = achievements.name;
        this.description = achievements.description;
        this.param = achievements.param;
        this.requirementsValue = achievements.requirementsValue;
        this.requirements = achievements.requirements;
        this.rewardImagePath = achievements.rewardImagePath;
        this.addUnlockPoint = achievements.addUnlockPoint;
        this.achieveImgPath = achievements.achieveImgPath;
    }
}

// 업적 클래스
public class Achievement
{
    public AchievementInfo achievementInfo { get; private set; }

    public bool CheckConditional(int param)
    {
        if (achievementInfo.isComplete)
        {
            return false;
        }
        bool result = param >= achievementInfo.requirementsValue;
        achievementInfo.isComplete = result;
        if (result)
        {
            AchievementManager.Instance.AddUnLockPoint(achievementInfo.addUnlockPoint);
        }
        return result;
    }

    public Achievement(int id)
    {
        achievementInfo = new AchievementInfo(id);
    }
}
