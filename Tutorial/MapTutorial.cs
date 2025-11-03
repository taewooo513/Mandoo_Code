using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTutorial : Tutorials
{
    public void OnCloseMapUI()
    {    
        base.OnCloseButton(); 
        AnalyticsManager.Instance.SendEventStep(4);
    }
}
