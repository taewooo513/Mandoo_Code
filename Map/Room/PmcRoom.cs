using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmcRoom : BaseRoom
{
    public override void EnterRoom()
    {
        base.EnterRoom();
        OnEventEnded();
        UIManager.Instance.OpenUI<InGamePMCUI>();
    }
    public override void ExitRoom()
    {
        UIManager.Instance.CloseUI<InGamePMCUI>();
    }
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}
