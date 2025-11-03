using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : BaseScene
{
    public override void Init()
    {
        UIManager.Instance.OpenUI<MainTitleUI>();
        if(AnalyticsManager.Instance.Step == 0)
        {
            // 다른 방식으로 예외 해도 되지만 이게 덜 수정할 것 같아서 이렇게 함.
            AnalyticsManager.Instance.SendEventStep(1);
            return;
        }
        AnalyticsManager.Instance.SendEventStep(28);
    }

    public override void LoadResources()
    {
    }

    public override void Release()
    {
    }
}
