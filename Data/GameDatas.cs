using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDatas : GameData
{
    public GameData GetGameData(int idx)
    {
        return GameDataList[idx];
    }
}
