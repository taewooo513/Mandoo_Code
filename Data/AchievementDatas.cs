using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AchievementDatas : Achievements
{
    public List<Achievements> GetAchievementsList()
    {
        return AchievementsList;
    }
    public Achievements GetAchievementData(int idx)
    {
        return AchievementsMap[idx];
    }
}
