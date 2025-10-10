using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoom : BaseRoom
{
    public override void EnterRoom()
    {
        base.EnterRoom();
        OnEventEnded();
        UIManager.Instance.OpenUI<InGameShopUI>();
    }
    public override void ExitRoom()
    {
        UIManager.Instance.CloseUI<InGameShopUI>();
    }

    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}
