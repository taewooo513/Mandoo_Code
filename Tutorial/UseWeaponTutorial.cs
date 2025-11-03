using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseWeaponTutorial : Tutorials
{
    void Start()
    {
        InGameItemManager.Instance.PushBackItem(InGameItemManager.Instance.AddItem(40011, 1)); //장비 지급    
    }
    public void OnCloseUseWeaponTutorial()
    {
        base.OnCloseButton();
        AnalyticsManager.Instance.SendEventStep(6);
        UIManager.Instance.OpenUI<InGameInventoryUI>();
        UIManager.Instance.CloseUI<MapUI>();
        ShowIfNeeded<StopWPTutorial>();
    }
}
