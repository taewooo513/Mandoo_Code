using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRoom : BaseRoom
{
    private GameObject _ShopNPCPrefab;
    
    public override void EnterRoom()
    {
        base.EnterRoom();
        OnEventEnded();
        if (!isInteract)
        {
            _ShopNPCPrefab = Spawn.ShopNPCCreate();
            ShopNPC shopNpc = _ShopNPCPrefab.GetComponent<ShopNPC>();
            shopNpc.NpcInteract(this);

            Vector3 pos = new Vector3(3f, 0, 0);
            _ShopNPCPrefab.transform.position = pos;
        }
        Tutorials.ShowIfNeeded<ShopTutorial>();
    }
    public override void ExitRoom()
    {
        base.ExitRoom();
        UIManager.Instance.CloseUI<InGameShopUI>();
        UIManager.Instance.RemoveUI<InGameShopUI>();
        Spawn.DestroyGameObject(_ShopNPCPrefab);
    }

    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}
