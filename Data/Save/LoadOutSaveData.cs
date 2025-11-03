using System.Collections.Generic;

public class LoadOutSaveData
{
    public int LoadOutUnlockPoint { get; set; }
    public List<int> LoadOutID { get; set; }
    public List<int> CurrentLoadOutID { get; set; }
    
    public LoadOutSaveData()
    {
        LoadOutUnlockPoint = 0;
        LoadOutID = new List<int>();
        CurrentLoadOutID = new List<int>();
    }
    public LoadOutSaveData(int unlockPoint, List<int> loadOutID, List<int> currentLoadOutID)
    {
        LoadOutUnlockPoint = unlockPoint;
        LoadOutID = loadOutID;
        CurrentLoadOutID = currentLoadOutID;
    }
}
