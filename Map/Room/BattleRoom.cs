using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class BattleRoom : BaseRoom
{
    private BattleData _battleData; //배틀데이터 데이터테이블
    
    public override void EnterRoom() //방 입장 시
    {
        Tutorials.ShowIfNeeded<BattleTutorial>();
        base.EnterRoom(); //플레이어 소환(위치 선정)
        AudioManager.Instance.PlayBGM(AudioInfo.Instance.battleBGM, AudioInfo.Instance.battleBGMVolume);
        if (GameManager.Instance.isBattleTesting)
        {
            var playerList = new List<BaseEntity>();
            foreach (var item in GameManager.Instance.playableCharacter)
            {
                playerList.Add(item);
            }
            BattleManager.Instance.BattleStartTrigger(playerList, GameManager.Instance.enemyCharacter, _battleData);
        }
        else if (!isInteract) //처음 입장시에만
        { 
            Spawn.EnemySpawn(_battleData.battleEnemies); //적 소환
            isInteract = true;
            var playerList = new List<BaseEntity>();
            foreach (var item in GameManager.Instance.playableCharacter)
            {
                playerList.Add(item);
            }
            BattleManager.Instance.BattleStartTrigger(playerList, GameManager.Instance.enemyCharacter, _battleData); //전투 시작
        }
        else
        {
            OnEventEnded();
        }
    }

    public void BattleTestInit(List<int> playerIds, List<int> enemyIds)
    {
        foreach (var item in playerIds)
        {
            Debug.Log(item);
            if (item != 0)
            {
                Spawn.PlayableCharacterCreate(item);
            }
        }
        
        Spawn.EnemySpawn(enemyIds.FindAll(x => x != 0));
    }
    public void BattleTestInit(List<int> playerIds, List<int> enemyIds, List<(int, int, int, int, float, float)> playerStatInfo, List<(int, int, int, int, float, float)> enemyStatInfo)
    {
        for (int i = 0; i < playerIds.Count; i++)
        {
            if (playerIds[i] != 0)
            {
                var player = Spawn.PlayableCharacterCreate(playerIds[i]);
                player.entityInfo.SetStatInfoForBattleTest(playerStatInfo[i]);
            }
        }
        
        List<BaseEntity> enemies = Spawn.EnemySpawn(enemyIds.FindAll(x => x != 0));
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].entityInfo.SetStatInfoForBattleTest(enemyStatInfo[i]);
        }
    }
    public void BattleEnd()
    {
        OnEventEnded();
    }

    public override void Init(int id)
    {
        base.Init(id);
        _battleData = DataManager.Instance.Battle.GetBattleData(id); //배틀데이터 데이터테이블에 접근
    }

    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0, 4);
    }
}