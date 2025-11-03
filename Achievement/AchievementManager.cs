using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : Singleton<AchievementManager>
{
    public int unLockPoint = 0;
    private Dictionary<string, int> achievementParam;
    private List<Achievement> _achievements;
    private AchievementNotification an;
    private Achievement testAchievement; // 테스트; 나중에 지울 예정.
    public void Start()
    {
        unLockPoint = DataManager.Instance.SaveData.LoadOutSave.LoadOutUnlockPoint;
        _achievements = new List<Achievement>();
        achievementParam = new Dictionary<string, int>();

        for (int i = 0; i < DataManager.Instance._achievement.GetAchievementsList().Count; i++)
        {
            var temp = DataManager.Instance._achievement.GetAchievementsList()[i];
            Achievement achievement = new Achievement(temp.Id);
            _achievements.Add(achievement);
            var val = DataManager.Instance.SaveData.AchievementSaveList.Find(e => e.ID == temp.Id);
            if (val != null)
            {
                achievement.achievementInfo.isComplete = val.IsCleared;
                InsertParam(achievement.achievementInfo.param, val.Count);
            }
            else
            {
                InsertParam(achievement.achievementInfo.param, 0);
            }
        }
    }
    public void AddUnLockPoint(int val)
    {
        unLockPoint += val;
        DataManager.Instance.OnLoadOutPointChanged(unLockPoint);
    }
    public bool RemoveUnLockPoint(int val)
    {
        bool result = unLockPoint - val >= 0;
        if (result)
        {
            unLockPoint -= val;
            DataManager.Instance.OnLoadOutPointChanged(unLockPoint);
        }
        return result;
    }

    public void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    AddParam("playedOverOneHr", 1);
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    AddParam("winCount", 1);
        //}

        // // 테스트입니다; 나중에 지울거에요
        // if (Input.GetKeyDown(KeyCode.A))
        // {
        //     int rand = Random.Range(1, 27);
        //     testAchievement = new Achievement(rand); // 테스트
        //     if (an == null)
        //         an = UIManager.Instance.OpenUI<AchievementNotification>();
        //     an.Popup(testAchievement);
        // }
    }
    public void InsertParam(string key, int val)
    {
        if (achievementParam.TryGetValue(key, out var value))
        {
            // Debug.Log("중복된키");
            return;
        }
        else
        {
            achievementParam.Add(key, val);
        }
    }

    public int GetParam(string key)
    {
        achievementParam.TryGetValue(key, out int res);
        return res;
    }

    public void AddParam(string key, int val)
    {
        if (GameManager.Instance.isBattleTesting) return;
        if (achievementParam.TryGetValue(key, out int res))
        {
            achievementParam[key] += val;
            foreach (var item in _achievements)
            {
                if (item.achievementInfo.param == key)
                {
                    DataManager.Instance.OnAchievementCountChanged(item.achievementInfo.id, achievementParam[key]);
                }
            }
        }
        else
        {
            Debug.Log(key + "is Not Find");
        }
    }

    public void SetParam(string key, int val)
    {
        if (GameManager.Instance.isBattleTesting) return;

        if (achievementParam.TryGetValue(key, out int res))
        {
            achievementParam[key] = val;
        }
        else
        {
            Debug.Log(key + "is Not Find");
        }
    }
    public int GetCount(string param)
    {
        return achievementParam[param];
    }

    public List<Achievement> GetCompletedAchievement()
    {
        List<Achievement> result = new List<Achievement>();
        for (int i = 0; i < _achievements.Count; i++)
        {
            if (_achievements[i].achievementInfo.isComplete)
            {
                result.Add(_achievements[i]);
            }
        }
        return result;
    }
    public List<Achievement> GetNotCompletedAchievement()
    {
        List<Achievement> result = new List<Achievement>();
        for (int i = 0; i < _achievements.Count; i++)
        {
            if (!_achievements[i].achievementInfo.isComplete)
            {
                result.Add(_achievements[i]);
            }
        }
        return result;
    }

    private List<Achievement> CheckConditional()
    {
        List<Achievement> results = new List<Achievement>();
        for (int i = 0; i < _achievements.Count; i++)
        {
            bool res = _achievements[i].CheckConditional(GetParam(_achievements[i].achievementInfo.param));
            if (res == true)
            {
                AddUnLockPoint(_achievements[i].achievementInfo.addUnlockPoint);
                results.Add(_achievements[i]);
                if (an == null)
                    an = UIManager.Instance.OpenUI<AchievementNotification>();
                an.Popup(_achievements[i]);
                DataManager.Instance.OnAchievementCleared(_achievements[i].achievementInfo.id);
            }
        }
        return results;
    }



    private void FixedUpdate()
    {
        CheckConditional();
    }
}
