using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopNPC : MonoBehaviour
{
    private RangeCheck _rangeCheck;
    public GameObject outline;
    public Button shopButton;
    private ShopRoom _shopRoom;
    
    public void Start()
    {
        _rangeCheck = GetComponent<RangeCheck>();
    }

    public void OnClickShop()
    {
        if (_rangeCheck != null)
        {
            _rangeCheck.Init(this);
            _rangeCheck.uiIsInteract = true;
        }
        UIManager.Instance.OpenUI<InGameShopUI>();
        if(_shopRoom != null)
            _shopRoom.isInteract = true;
    }
    
    public void NpcInteract(ShopRoom shopRoom)
    {
        _shopRoom = shopRoom;
    }
}
