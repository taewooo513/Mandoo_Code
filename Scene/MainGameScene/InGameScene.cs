using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameScene : BaseScene
{
    public override void Init()
    {
        // UIManager.Instance.OpenUI<InGameUIManager>();
        // UIManager.Instance.OpenUI<InGamePlayerUI>();
        // UIManager.Instance.OpenUI<UIInputHandler>();

        // 이밑으로 레이어 순서 관리해주세요
        UIManager.Instance.OpenUI<InGameUIManager>();
        UIManager.Instance.OpenUI<UIInputHandler>();
        UIManager.Instance.OpenUI<MapUI>();
        UIManager.Instance.OpenUI<OptionButtonUI>();
        UIManager.Instance.OpenUI<FadeInOut>();
        UIManager.Instance.OpenUI<MapTutorial>();
        UIManager.Instance.OpenUI<InGamePlayerUI>();
        UIManager.Instance.CloseUI<FadeInOut>();
        UIManager.Instance.CloseUI<MapTutorial>();
        // ******

        MapManager.Instance.GenerateMap(GameManager.Instance.CurrentMapIndex);
        OutGameItemManager.Instance.StartGameItemInput();
    }

    public override void LoadResources()
    {

    }

    public override void Release()
    {
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
