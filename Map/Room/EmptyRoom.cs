using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyRoom : BaseRoom
{
    public override void EnterRoom()
    {
        base.EnterRoom();
        OnEventEnded();
    }
    public override void OnEventEnded()
    {
        base.OnEventEnded();
    }
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}
