using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameShopUI : UIBase
{
    protected override void OnOpen()
    {
        base.OnOpen();
        GameManager.Instance.isShop = true;
    }
    public ShopItemManager shopItemManager;
    void Start()
    {
        UIManager.Instance.OpenUI<InGameInventoryUI>();
        UIManager.Instance.CloseUI<MapUI>();

        if (shopItemManager == null)
            shopItemManager = GetComponentInChildren<ShopItemManager>();
        shopItemManager.OpenShop();

    }

    protected override void OnClose()
    {
        base.OnClose();
        GameManager.Instance.isShop = false;
    }
}
