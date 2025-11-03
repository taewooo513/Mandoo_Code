using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class CellCollider : MonoBehaviour
{
    [SerializeField] private int index;
    private Cell _cell;
    private BattleData _battleData;
    public bool hasBattle;
    public bool isBattleOver;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_cell != null && _cell.cellEvent != EventType.Trap && _cell.cellEvent != EventType.Treasure) _cell.AlreadyVisited = true;
        
        if (hasBattle && !isBattleOver)
        {
            isBattleOver = true;
            BattleStart();
        }
    }

    public void Init(Cell cell, int id = 0)
    {
        _cell = cell;
        if(id != 0) _battleData = DataManager.Instance.Battle.GetBattleData(id);
    }

    private void BattleStart()
    {
        if (_battleData == null || _battleData.battleEnemies == null) return;
        Spawn.EnemySpawn(_battleData.battleEnemies);
        var playerList = new List<BaseEntity>();
        foreach (var item in GameManager.Instance.playableCharacter)
        {
            playerList.Add(item);
        }
        BattleManager.Instance.BattleStartTrigger(playerList, GameManager.Instance.enemyCharacter, _battleData);
    }
    
    //메서드를 따로 만들어서 보상까지 주어야합니다.
}
