using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTutorial : Tutorials
{
    public void OnCloseWeaponTutorial()
   {
       base.OnCloseButton();
       AnalyticsManager.Instance.SendEventStep(8);
       ShowIfNeeded<CorridorTutorial>();
    }
}
