using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScene : BaseScene
{
    public override void Init()
    {
        UIManager.Instance.OpenUI<InGameUIManager>();
        UIManager.Instance.OpenUI<InGamePlayerUI>();
        UIManager.Instance.OpenUI<UIInputHandler>();
        UIManager.Instance.OpenUI<TutorialSkipUI>();

        // 이밑으로 레이어 순서 관리해주세요

        // ******
        MapManager.Instance.GenerateTutorialMap();
        AnalyticsManager.Instance.SendEventStep(3);
    }

    public override void LoadResources()
    {
    }

    public override void Release()
    {
    }
}
