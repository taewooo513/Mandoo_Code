using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<StageSaveData> StageSaveList { get; set; }
    public SettingSaveData SettingSave { get; set; }
    public LoadOutSaveData LoadOutSave { get; set; }
    public List<AchievementSaveData> AchievementSaveList { get; set; }
    
    public SaveData()
    {
        StageSaveList = new List<StageSaveData>();
        SettingSave = new SettingSaveData();
        LoadOutSave = new LoadOutSaveData();
        AchievementSaveList = new List<AchievementSaveData>();
    }
    
    public bool IsStageCleared(int stageNumber)
    {
        return StageSaveList.Find(x => x.StageID == stageNumber).IsCleared;
    }

    public bool HasOutGameItem(int itemID)
    {
        return LoadOutSave.LoadOutID.Contains(itemID);
    }
}
