using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTutorial : Tutorials
{
   public void OnCloseShopTutorial()
   {
       base.OnCloseButton();
       AnalyticsManager.Instance.SendEventStep(21);
    }
}
