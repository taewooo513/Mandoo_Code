using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunResultTutorial : Tutorials
{
    public void OnCloseRunResultTutorial()
    {
        base.OnCloseButton();
        AnalyticsManager.Instance.SendEventStep(27);

    }
}
