using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMCHireTutorial : Tutorials
{
    void Start()
    {
        InGameItemManager.Instance.PushBackItem(InGameItemManager.Instance.AddItem(1001, 1000)); //골드 지급
        UIManager.Instance.CloseUI<MapUI>();
        UIManager.Instance.OpenUI<InGameInventoryUI>();
    }
    public void OnClosePMCHireTutorial()
   {
       base.OnCloseButton();
       AnalyticsManager.Instance.SendEventStep(15);
    }
}
