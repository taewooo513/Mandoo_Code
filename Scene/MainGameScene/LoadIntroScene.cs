using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadIntroScene : MonoBehaviour
{
     async UniTask Start()
    {
        await AnalyticsManager.Instance.Init();
        DataManager.Instance.Init();
        await DataManager.Instance.Initialize();
        GameManager.Instance.Init();
        AudioManager.Instance.Init();
        ResourceManager.Instance.Init();
        SceneLoadManager.Instance.Init();
        SpriteManager.Instance.Init();
        AchievementManager.Instance.Init();
        OutGameItemManager.Instance.Init();
        SceneLoadManager.Instance.LoadScene(SceneKey.titleScene);
    }


}
