using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTutorial : Tutorials
{
    public void OnCloseObjectTutorial()
    {
        base.OnCloseButton();
        AnalyticsManager.Instance.SendEventStep(10);
    }
}
