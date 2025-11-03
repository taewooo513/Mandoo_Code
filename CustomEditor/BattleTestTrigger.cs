using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTestTrigger : MonoBehaviour
{
    [SerializeField] private GameObject background;
    public bool isDataReceived;
    private ManDooBattleToolData _data;
    public bool isSceneStarted;
    public bool isTestStarted;
    private bool _gotStatInfo;
    private List<(int, int, int, int, float, float)> _playerStatInfo;
    private List<(int, int, int, int, float, float)> _enemyStatInfo;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        DataManager.Instance.Initialize();
        isSceneStarted = true;
    }
    public void GetData(ManDooBattleToolData data)
    {
        _data = data;
        isDataReceived = true;
    }

    public void GetData(ManDooBattleToolData data, 
        List<(int, int, int, int, float, float)> playerStatInfo,
        List<(int, int, int, int, float, float)> enemyStatInfo)
    {
        _data = data;
        isDataReceived = true;
        _gotStatInfo = true;
        _playerStatInfo = playerStatInfo;
        _enemyStatInfo = enemyStatInfo;
    }

    private void ShowData()
    {
        foreach (var item in _data.PlayerIDs)
        {
            if (item != 0)
            {
                Debug.Log(item);
            }
        }
    
        foreach (var item in _data.EnemyIDs)
        {
            if (item != 0)
            {
                Debug.Log(item);
            }
        }
    }

    public void StartTest()
    {
        isTestStarted = true;
        Destroy(background);
        if (!_gotStatInfo) GameManager.Instance.StartBattleTest(_data.PlayerIDs, _data.EnemyIDs);
        else GameManager.Instance.StartBattleTest(_data.PlayerIDs, _data.EnemyIDs, _playerStatInfo, _enemyStatInfo);
    }

    private void OnApplicationQuit()
    {
        
    }
}
