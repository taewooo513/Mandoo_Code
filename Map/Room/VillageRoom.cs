using System.Collections;
using System.Collections.Generic;
using DataTable;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VillageRoom : BaseRoom
{
    public BattleData battleData;
    private List<int> _equipItemIds = new();
    
    private int _dropGoldCount;
    private int _dropItem; //드랍하는 아이템id (골드x)
    private float _battleDropProb; //아이템 드랍 확률 (ex : 0.25)
    private float _goldRandomRatio; //0.9~1.1 사이 랜덤 난수 반환, 골드 떨어지는 랜덤 개수
    private int _randomGoldDropCount; //실제로 떨어지는 금화 개수
    private float _randomPercentage; //0~100 사이 중 랜덤 퍼센트 (랜덤 숫자 뽑기)
    public override void EnterRoom() //방 입장 시
    {
        base.EnterRoom(); //플레이어 소환(위치 선정)
        AudioManager.Instance.PlayBGM(AudioInfo.Instance.bossBGM, AudioInfo.Instance.bossBGMVolume);
        
        if (!isInteract) //처음 입장시에만
        {
            Spawn.EnemySpawn(battleData.battleEnemies); //적 소환
            isInteract = true;
            var playerList = new List<BaseEntity>();
            foreach (var item in GameManager.Instance.playableCharacter)
            {
                playerList.Add(item);
            }
            BattleManager.Instance.BattleStartTrigger(playerList, GameManager.Instance.enemyCharacter, battleData); //전투 시작
        }
        else
        {
            OnEventEnded();
        }
    }
    
    public void BattleEnd()
    {
        GameManager.Instance.GameFinish(true);
    }
    public override void Init(int id) 
    {
        base.Init(id);
        battleData = DataManager.Instance.Battle.GetBattleData(id); //배틀데이터 데이터테이블에 접근
        // _dropGoldCount = battleData.dropGold; //골드 드랍 개수
        // _dropItem = battleData.dropId; //드랍하는 아이템id (골드x)
        // _battleDropProb = battleData.dropProb; //아이템 드랍 확률 (ex : 0.25)
        // _goldRandomRatio = Random.Range(0.9f, 1.1f); //0.9~1.1 사이 랜덤 난수 반환, 골드 떨어지는 랜덤 개수
        // _randomGoldDropCount = (int)(_dropGoldCount * _goldRandomRatio); //실제로 떨어지는 금화 개수
        // _randomPercentage = Random.Range(0f, 100f);
    }
    
    // public void PlayerDeadEquipItem(List<int> id) //플레이어가 죽을 때 가지고 있던 아이템 리스트
    // {
    //     _equipItemIds = id;
    // }
    
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}
