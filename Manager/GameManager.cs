using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool isShop = false;
    public List<BaseEntity> playableCharacter;
    public List<BaseEntity> enemyCharacter;
    public List<InGameItem> deadEquipWeapons = new();
    public bool isBattleTesting = false;
    private int _currentMapIndex = 0;
    public int CurrentMapIndex => _currentMapIndex;
    public bool playerCanMove = true;
    public bool uiPlayerCanMove = true; //true = 이동 가능, false = 이동 불가능
    public bool playerObjectInteract = false; //true = 이동 불가능, false = 이동 가능
    public GameDatas gameDatas;

    protected override void Awake()
    {
        base.Awake();
        playableCharacter = new List<BaseEntity>();
        enemyCharacter = new List<BaseEntity>();
        gameDatas = DataManager.Instance.Game;
    }

    public void AddPlayer(BaseEntity baseEntity)
    {
        playableCharacter.Add(baseEntity);
    }

    public void DeletePlayableCharacter(BaseEntity baseEntity)
    {
        Destroy(baseEntity);
        playableCharacter.Remove(baseEntity);
    }
    public void AddEnemy(BaseEntity baseEntity)
    {
        enemyCharacter.Add(baseEntity);
    }
    public bool HasPlayerById(int id)
    {
        return playableCharacter.Exists(pc => pc.id == id);//중복체크용
    }
    public void RemovePlayer(int id)
    {
        playableCharacter.RemoveAll(pc => pc.id == id);
    }

    public void StartGame(int index)
    {
        //TODO:이 시점에서 들고간 로드아웃 저장
        _currentMapIndex = index;
        playableCharacter.Clear();
        enemyCharacter.Clear();
        //TODO:인벤토리 비우는 것도 필요함.
        UIManager.Instance.ClearUI();
        SceneLoadManager.Instance.LoadScene(SceneKey.inGameScene);
        if(AnalyticsManager.Instance.Step == 30)
            AnalyticsManager.Instance.SendEventStep(31);
    }

    public void StartTutorial()
    {

        _currentMapIndex = 0;
        playableCharacter.Clear();
        enemyCharacter.Clear();
        UIManager.Instance.ClearUI();
        SceneLoadManager.Instance.LoadScene(SceneKey.tutorialScene);
    }

    public void GameFinish(bool isClear)
    {
        bool isNewlyCleared = false;
        if (isClear)
        {
            SaveData save = DataManager.Instance.SaveData;

            StageSaveData stageSave;
            
            if (_currentMapIndex == 0) stageSave = save.StageSaveList[0];
            else stageSave = save.StageSaveList.Find(x=>x.StageID == _currentMapIndex);
            
            if (stageSave == null) return;
            
            if(!stageSave.IsCleared)
            {
                isNewlyCleared = true;
                
            }
            DataManager.Instance.OnStageClear(_currentMapIndex);
        }
        
        UIManager.Instance.ClearUI();
        InGameItemManager.Instance.ClearInventory();
        var ui = UIManager.Instance.OpenUI<InGameRunResultUI>();
        ui.Description(_currentMapIndex , isClear, isNewlyCleared);
    }

    public void StartBattleTest(List<int> playerIds, List<int> enemyIds)
    {
        isBattleTesting = true;
        playableCharacter.Clear();
        enemyCharacter.Clear();
        //TODO:인벤토리 비우는 것도 필요함.
        UIManager.Instance.ClearUI();
        //SceneManager.LoadScene("1.Scenes/MapTest");
        MapManager.Instance.GenerateBattleTestMap(playerIds, enemyIds);
        UIManager.Instance.OpenUI<InGameUIManager>();
        UIManager.Instance.OpenUI<InGamePlayerUI>();
        UIManager.Instance.OpenUI<UIInputHandler>();
    }

    public void StartBattleTest(List<int> playerIds, List<int> enemyIds,
        List<(int, int, int, int, float, float)> playerStatInfo, List<(int, int, int, int, float, float)> enemyStatInfo)
    {
        isBattleTesting = true;
        playableCharacter.Clear();
        enemyCharacter.Clear();
        //TODO:인벤토리 비우는 것도 필요함.
        UIManager.Instance.ClearUI();
        //SceneManager.LoadScene("1.Scenes/MapTest");
        MapManager.Instance.GenerateBattleTestMap(playerIds, enemyIds, playerStatInfo, enemyStatInfo);
        UIManager.Instance.OpenUI<InGameUIManager>();
        UIManager.Instance.OpenUI<InGamePlayerUI>();
        UIManager.Instance.OpenUI<UIInputHandler>();
    }
    public void StartBattle()
    {
        BattleManager.Instance.BattleStartTrigger(playableCharacter, enemyCharacter);
    }

    public void EndGame()
    {
    }

    public void PlayableCharacterPosition(List<BaseEntity> playerPositionList) //캐릭터 스폰(위치 지정)
    {
        playableCharacter = playerPositionList;
    }
}