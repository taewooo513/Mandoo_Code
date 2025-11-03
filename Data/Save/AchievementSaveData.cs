public class AchievementSaveData
{
    public int ID { get; set; }
    public string Param { get; set; }
    public int Count { get; set; }
    public bool IsCleared { get; set; }
    
    public AchievementSaveData()
    {
        ID = 0;
        Param = "";
        Count = 0;
        IsCleared = false;
    }
    public AchievementSaveData(int id, string param, int count, bool isCleared)
    {
        ID = id;
        Param = param;
        Count = count;
        IsCleared = isCleared;
    }
}