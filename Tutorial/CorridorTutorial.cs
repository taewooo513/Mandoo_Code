using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorridorTutorial : Tutorials
{
    public void OnCloseCorridorTutorial()
    {
        base.OnCloseButton();
        AnalyticsManager.Instance.SendEventStep(9);
        GameManager.Instance.playerCanMove = true;
    }
}
