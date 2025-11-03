using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartRoom : BaseRoom
{
    public override void EnterRoom()
    {
        
        //AnalyticsManager.Instance.SendEventStep(29); ???
        UIManager.Instance.OpenUI<OptionButtonUI>();
        AudioManager.Instance.PlayBGM(AudioInfo.Instance.corridorBGM, AudioInfo.Instance.corridorBGMVolume);
        base.EnterRoom(); //스폰 챙겨오기

        if (!isInteract) //첫 1회만 생성
        {
            Spawn.PlayableCharacterCreate(1004); //시작 플레이어(1004) 생성
            if (SceneManager.GetActiveScene().name != "TutorialScene")
            {
                InGameItemManager.Instance.PushBackItem(InGameItemManager.Instance.AddItem(1001, 1000));
                InGameItemManager.Instance.PushBackItem(InGameItemManager.Instance.AddItem(2001, 3));
            }
            isInteract = true;
        }
        Spawn.PlayableCharacterSpawn(); //플레이어 소환(위치 선정)
        OnEventEnded();
        Tutorials.ShowIfNeeded<MapTutorial>();
        GameManager.Instance.playerCanMove = true;
    }

    public override void OnEventEnded()
    {
        base.OnEventEnded();
    }
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}