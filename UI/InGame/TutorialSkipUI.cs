using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSkipUI : UIBase
{
    public void OnClickSkip()
    {
        AchievementManager.Instance.AddParam("skippedTutorial", 1);
        BattleManager.Instance.EndBattle();
        GameManager.Instance.GameFinish(true);
    }
}
