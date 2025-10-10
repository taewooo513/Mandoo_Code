using System.Collections.Generic;
using UnityEngine;
using System;

public class InGamePMCUI : UIBase
{
    void Start()
    {
        //UIManager.Instance.OpenUI<InGameInventoryUI>();
        UIManager.Instance.CloseUI<MapUI>();
        PMCCardManager.Instance.RefreshCardsOnPanel();
    }
}