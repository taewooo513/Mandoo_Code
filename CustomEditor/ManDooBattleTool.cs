using System;
using System.Collections.Generic;
using System.ComponentModel;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR

public class ManDooBattleTool : EditorWindow
{

    private ManDooBattleToolData _data = new();
    private BattleTestTrigger _trigger;

    private int[] _playerIDs = new int[4];
    private int[] _enemyIDs = new int[4];

    private (int, int, int, int, float, float)[] _playerStatInfo = new (int, int, int, int, float, float)[4]; //health, attack, defense, speed, evasion, critical
    private (int, int, int, int, float, float)[] _enemyStatInfo = new (int, int, int, int, float, float)[4];
    private Color _originColor;
    private string _errorMsg;
    private bool _isStatInfoOpened;
    private bool _receivedStatData;

    private void OnEnable()
    {
        EditorApplication.update += WaitForTrigger;
    }

    private void OnDisable()
    {
        EditorApplication.update -= WaitForTrigger;
    }

    [MenuItem("BattleTest/Setting")] // 메뉴 등록
    private static void Init()
    {
        // 현재 활성화된 윈도우 가져오며, 없으면 새로 생성
        ManDooBattleTool window = (ManDooBattleTool)GetWindow(typeof(ManDooBattleTool));
        window.Show();


        // 윈도우 타이틀 지정
        window.titleContent.text = "ManDoo Battle Tool";

        // 최소, 최대 크기 지정
        window.minSize = new Vector2(570f, 250f);
        window.maxSize = new Vector2(1000f, 250f);
    }

    void OnGUI()
    {

        // 굵은 글씨 
        _originColor = EditorStyles.boldLabel.normal.textColor;
        EditorStyles.boldLabel.normal.textColor = Color.yellow;

        // Header =====================================================================

        if (!string.IsNullOrEmpty(_errorMsg))
        {
            EditorGUILayout.HelpBox(_errorMsg, MessageType.Error);
            EditorStyles.boldLabel.normal.textColor = _originColor;
        }

        if ((_trigger != null && !_trigger.isSceneStarted) || !EditorApplication.isPlaying)
        {
            EditorGUILayout.HelpBox("전투 테스트 씬이 활성 상태가 아닙니다...!", MessageType.Error);
            EditorStyles.boldLabel.normal.textColor = _originColor;
            return;
        }

        // Player IDs - 가로 배치
        GUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width(60));
        GUILayout.Label("Player IDs", EditorStyles.boldLabel, GUILayout.Width(255));
        GUILayout.Label("Enemy IDs", EditorStyles.boldLabel, GUILayout.Width(255));

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width(60));
        GUILayout.Label("3", GUILayout.Width(59));
        GUILayout.Label("2", GUILayout.Width(59));
        GUILayout.Label("1", GUILayout.Width(59));
        GUILayout.Label("0", GUILayout.Width(59));
        GUILayout.Label("0", GUILayout.Width(59));
        GUILayout.Label("1", GUILayout.Width(59));
        GUILayout.Label("2", GUILayout.Width(59));
        GUILayout.Label("3", GUILayout.Width(59));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("EntityID", GUILayout.Width(60));
        _playerIDs[3] = EditorGUILayout.IntField(_playerIDs[3], GUILayout.Width(60));
        _playerIDs[2] = EditorGUILayout.IntField(_playerIDs[2], GUILayout.Width(60));
        _playerIDs[1] = EditorGUILayout.IntField(_playerIDs[1], GUILayout.Width(60));
        _playerIDs[0] = EditorGUILayout.IntField(_playerIDs[0], GUILayout.Width(60));

        // Enemy IDs - 가로 배치

        _enemyIDs[0] = EditorGUILayout.IntField(_enemyIDs[0], GUILayout.Width(60));
        _enemyIDs[1] = EditorGUILayout.IntField(_enemyIDs[1], GUILayout.Width(60));
        _enemyIDs[2] = EditorGUILayout.IntField(_enemyIDs[2], GUILayout.Width(60));
        _enemyIDs[3] = EditorGUILayout.IntField(_enemyIDs[3], GUILayout.Width(60));

        GUILayout.EndHorizontal();

        GUILayout.Space(10f);

        if (_isStatInfoOpened)
        {
            OpenStatInfo();
        }

        // Horizontal =================================================================
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Stat Info"))
        {
            _isStatInfoOpened = !_isStatInfoOpened;
            _receivedStatData = false;
        }
        if (GUILayout.Button("Send Data"))
        {
            _errorMsg = "";
            if (ValidateData())
            {
                _data.PlayerIDs.AddRange(_playerIDs);
                _data.EnemyIDs.AddRange(_enemyIDs);
                if (!_isStatInfoOpened) _trigger.GetData(_data);
                else
                {
                    if (ValidateStatInfo()) _trigger.GetData(_data, GetStatInfo(true), GetStatInfo(false));
                }
            }
        }

        GUILayout.EndHorizontal();
        // ============================================================================

        if (_trigger != null && _trigger.isDataReceived && !_trigger.isTestStarted)
        {
            _trigger.StartTest();
        }

        EditorStyles.boldLabel.normal.textColor = _originColor;
    }

    private void WaitForTrigger()
    {
        if (_trigger == null)
        {
            _trigger = FindObjectOfType<BattleTestTrigger>();
            if (_trigger != null)
            {
                EditorApplication.update -= WaitForTrigger;
            }
        }
    }

    private bool ValidateData()
    {
        int _playerIDSum = 0;
        for (int i = 0; i < 4; i++)
        {
            _playerIDSum += _playerIDs[i];
        }

        if (_playerIDSum == 0)
        {
            _errorMsg = "플레이어의 리스트가 비어있습니다!";
            EditorStyles.boldLabel.normal.textColor = _originColor;
            return false;
        }

        int _enemyIDSum = 0;
        for (int i = 0; i < 4; i++)
        {
            _enemyIDSum += _enemyIDs[i];
        }

        if (_enemyIDSum == 0)
        {
            _errorMsg = "적의 리스트가 비어있습니다!";
            EditorStyles.boldLabel.normal.textColor = _originColor;
            return false;
        }

        bool isNull = false;
        for (int i = 0; i < 4; i++)
        {
            if (isNull && _playerIDs[i] != 0)
            {
                _errorMsg = "플레이어의 순서가 잘못되었습니다!";
                EditorStyles.boldLabel.normal.textColor = _originColor;
                return false;
            }

            if (_playerIDs[i] != 0 && !DataManager.Instance.Mercenary.HasKey(_playerIDs[i]))
            {
                _errorMsg = $"{i}번의 플레이어 ID에 해당하는 데이터가 존재하지 않습니다!";
                EditorStyles.boldLabel.normal.textColor = _originColor;
                return false;
            }

            if (_playerIDs[i] == 0)
            {
                isNull = true;
            }

        }

        isNull = false;
        for (int i = 0; i < 4; i++)
        {
            if (isNull && _enemyIDs[i] != 0)
            {
                _errorMsg = "적의 순서가 잘못되었습니다!";
                return false;
            }

            if (_enemyIDs[i] != 0 && !DataManager.Instance.Enemy.HasKey(_enemyIDs[i]))
            {
                _errorMsg = $"{i}번의 적 ID에 해당하는 데이터가 존재하지 않습니다!";
                return false;
            }

            if (_enemyIDs[i] == 0)
            {
                isNull = true;
            }
        }
        return true;
    }

    private void OpenStatInfo()
    {
        if (!_receivedStatData)
        {
            for (int i = 0; i < 4; i++)
            {
                if (_playerIDs[i] != 0)
                {
                    if (_playerIDs[i] != 0 && !DataManager.Instance.Mercenary.HasKey(_playerIDs[i]))
                    {
                        _errorMsg = $"{i}번의 플레이어 ID에 해당하는 데이터가 존재하지 않습니다!";
                        EditorStyles.boldLabel.normal.textColor = _originColor;
                        return;
                    }

                    var playerStatInfo = DataManager.Instance.Mercenary.GetMercenaryData(_playerIDs[i]);
                    _playerStatInfo[i] = (playerStatInfo.health, playerStatInfo.attack, playerStatInfo.defense,
                        playerStatInfo.speed, playerStatInfo.evasion, playerStatInfo.critical);
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (_enemyIDs[i] != 0)
                {
                    if (_enemyIDs[i] != 0)
                    {
                        if (_enemyIDs[i] != 0 && !DataManager.Instance.Enemy.HasKey(_enemyIDs[i]))
                        {
                            _errorMsg = $"{i}번의 적 ID에 해당하는 데이터가 존재하지 않습니다!";
                            EditorStyles.boldLabel.normal.textColor = _originColor;
                            return;
                        }

                        var enemyStatInfo = DataManager.Instance.Enemy.GetEnemyData(_enemyIDs[i]);
                        _enemyStatInfo[i] = (enemyStatInfo.health, enemyStatInfo.attack, enemyStatInfo.defense,
                            enemyStatInfo.speed, enemyStatInfo.evasion, enemyStatInfo.critical);
                    }
                }
            }
            _receivedStatData = true;
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("Health", GUILayout.Width(60));
        for (int i = 3; i >= 0; i--)
        {
            _playerStatInfo[i].Item1 = EditorGUILayout.IntField(_playerStatInfo[i].Item1, GUILayout.Width(60));
        }

        for (int i = 0; i < 4; i++)
        {
            _enemyStatInfo[i].Item1 = EditorGUILayout.IntField(_enemyStatInfo[i].Item1, GUILayout.Width(60));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Attack", GUILayout.Width(60));
        for (int i = 3; i >= 0; i--)
        {
            _playerStatInfo[i].Item2 = EditorGUILayout.IntField(_playerStatInfo[i].Item2, GUILayout.Width(60));
        }

        for (int i = 0; i < 4; i++)
        {
            _enemyStatInfo[i].Item2 = EditorGUILayout.IntField(_enemyStatInfo[i].Item2, GUILayout.Width(60));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Defense", GUILayout.Width(60));
        for (int i = 3; i >= 0; i--)
        {
            _playerStatInfo[i].Item3 = EditorGUILayout.IntField(_playerStatInfo[i].Item3, GUILayout.Width(60));
        }

        for (int i = 0; i < 4; i++)
        {
            _enemyStatInfo[i].Item3 = EditorGUILayout.IntField(_enemyStatInfo[i].Item3, GUILayout.Width(60));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Speed", GUILayout.Width(60));
        for (int i = 3; i >= 0; i--)
        {
            _playerStatInfo[i].Item4 = EditorGUILayout.IntField(_playerStatInfo[i].Item4, GUILayout.Width(60));
        }

        for (int i = 0; i < 4; i++)
        {
            _enemyStatInfo[i].Item4 = EditorGUILayout.IntField(_enemyStatInfo[i].Item4, GUILayout.Width(60));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Evasion", GUILayout.Width(60));
        for (int i = 3; i >= 0; i--)
        {
            _playerStatInfo[i].Item5 = EditorGUILayout.FloatField(_playerStatInfo[i].Item5, GUILayout.Width(60));
        }

        for (int i = 0; i < 4; i++)
        {
            _enemyStatInfo[i].Item5 = EditorGUILayout.FloatField(_enemyStatInfo[i].Item5, GUILayout.Width(60));
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Critical", GUILayout.Width(60));
        for (int i = 3; i >= 0; i--)
        {
            _playerStatInfo[i].Item6 = EditorGUILayout.FloatField(_playerStatInfo[i].Item6, GUILayout.Width(60));
        }

        for (int i = 0; i < 4; i++)
        {
            _enemyStatInfo[i].Item6 = EditorGUILayout.FloatField(_enemyStatInfo[i].Item6, GUILayout.Width(60));
        }
        GUILayout.EndHorizontal();
    }

    private List<(int, int, int, int, float, float)> GetStatInfo(bool isPlayer)
    {
        List<(int, int, int, int, float, float)> result = new();
        if (isPlayer)
        {
            for (int i = 0; i < 4; i++)
            {
                if (ValidateStat(_playerStatInfo[i])) result.Add(_playerStatInfo[i]);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (ValidateStat(_enemyStatInfo[i])) result.Add(_enemyStatInfo[i]);
            }
        }

        return result;
    }
    private bool ValidateStatInfo()
    {
        for (int i = 0; i < 4; i++)
        {
            if (_playerIDs[i] == 0 && !IsStatEmpty(_playerStatInfo[i]))
            {
                _errorMsg = $"{i}번의 플레이어의 ID는 비어있는데, 스탯값은 들어있습니다!";
                return false;
            }

            if (_playerIDs[i] != 0 && !ValidateStat(_playerStatInfo[i]))
            {
                _errorMsg = $"{i}번의 플레이어의 ID는 들어있는데, 빈 스탯값이 있습니다!";
                return false;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            if (_enemyIDs[i] == 0 && !IsStatEmpty(_enemyStatInfo[i]))
            {
                _errorMsg = $"{i}번의 적의 ID는 비어있는데, 스탯값은 들어있습니다!";
                return false;
            }

            if (_enemyIDs[i] != 0 && !ValidateStat(_enemyStatInfo[i]))
            {
                _errorMsg = $"{i}번의 적의 ID는 들어있는데, 빈 스탯값이 있습니다!";
                return false;
            }
        }

        return true;
    }

    private bool IsStatEmpty((int, int, int, int, float, float) statInfo)
    {
        return statInfo.Item1 == 0 && statInfo.Item2 == 0 && statInfo.Item3 == 0 && statInfo.Item4 == 0;
    }

    private bool ValidateStat((int, int, int, int, float, float) statInfo)
    {
        return statInfo.Item1 > 0 && statInfo.Item2 > 0 && statInfo.Item3 > 0 && statInfo.Item4 > 0;
    }
}
#endif