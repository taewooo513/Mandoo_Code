using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTutorial : Tutorials
{
    public void OnCloseBattleTutorial()
    {
        base.OnCloseButton();
        AnalyticsManager.Instance.SendEventStep(25);
    }
}
