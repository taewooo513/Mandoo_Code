using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameShopUI : UIBase
{
    void Start()
    { 
        //UIManager.Instance.OpenUI<InGameInventoryUI>();
        UIManager.Instance.CloseUI<MapUI>();
    }
}
