using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureTutorial : Tutorials
{
    public void OnCloseTreasureTutorial()
    {
        base.OnCloseButton();
        AnalyticsManager.Instance.SendEventStep(18);
        GameManager.Instance.playerCanMove = true;
    }
}
