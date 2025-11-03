public class StageSaveData
{
    public int StageID { get; set; }
    public bool IsCleared { get; set; }
    public int ClearCount { get; set; }
    
    public StageSaveData()
    {
        StageID = 0;
        IsCleared = false;
        ClearCount = 0;
    }
    public StageSaveData(int id)
    {
        StageID = id;
        IsCleared = false;
        ClearCount = 0;
    }
    public StageSaveData(int id, bool isCleared, int clearCount)
    {
        StageID = id;
        IsCleared = isCleared;
        ClearCount = clearCount;
    }
}
