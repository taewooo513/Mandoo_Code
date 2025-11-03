using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AchievementUI : MonoBehaviour
{
    AchievementUISlot[] achievements;
    private void Awake()
    {
        achievements = GetComponentsInChildren<AchievementUISlot>();
    }

    private void OnEnable()
    {
        var complete = AchievementManager.Instance.GetCompletedAchievement();
        for (int i = 0; i < achievements.Length; i++)
        {
            if (i < AchievementManager.Instance.GetCompletedAchievement().Count)
            {
                achievements[i].Init(complete[i]);
                achievements[i].gameObject.SetActive(true);
            }
            else if (i - complete.Count < AchievementManager.Instance.GetNotCompletedAchievement().Count)
            {
                achievements[i].Init(AchievementManager.Instance.GetNotCompletedAchievement()[i - complete.Count]);
                achievements[i].gameObject.SetActive(true);
            }
            else
            {
                achievements[i].gameObject.SetActive(false);
            }
        }
    }
}
