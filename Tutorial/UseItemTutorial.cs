using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItemTutorial : Tutorials
{
    void Start()
    {
        InGameItemManager.Instance.PushBackItem(InGameItemManager.Instance.AddItem(2001, 3)); //회복약 지급
        //ShowIfNeeded<StopWPTutorial>();
        UIManager.Instance.OpenUI<InGameInventoryUI>();
        UIManager.Instance.CloseUI<MapUI>();
        GameManager.Instance.playerCanMove = false;
    }
    public void OnCloseUseItemTutorial()
    {
        ShowIfNeeded<StopCATutorial>();
        base.OnCloseButton();
        AnalyticsManager.Instance.SendEventStep(12);
    }
}
