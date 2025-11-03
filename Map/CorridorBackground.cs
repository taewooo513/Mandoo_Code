using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class CorridorBackground : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private GameObject[] cells;
    private CellCollider[] _cellColliders;
    [SerializeField] private float speed;
    private Vector2 _currentMovement;
    private Corridor _corridor;
    private readonly List<GameObject> _gameObjectList = new();
    private bool _isMoving;
    private CorridorMovement _currentCorridorMovement = CorridorMovement.Stop;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _cellColliders = new CellCollider[cells.Length];
        for (int i = 0; i < cells.Length; i++)
        {
            _cellColliders[i] = cells[i].GetComponent<CellCollider>();
        }
    }

    private void OnDisable()
    {
        foreach (var item in GameManager.Instance.playableCharacter)
        {
            if (item.characterAnimationController == null) return;
            item.characterAnimationController.SetTrigger("Idle");
            item.characterAnimationController.FlipX(false);
        }
    }

    public void Init()
    {
        if (MapManager.Instance.CurrentLocation is Corridor corridor)
        {
            _corridor = corridor;
        }

        foreach (var item in cells)
        {
            var cellCollider = item.GetComponent<CellCollider>();
            cellCollider.hasBattle = false;
            cellCollider.isBattleOver = false;
        }

        Debug.Log($"{_corridor != null} {_corridor.AlreadyVisited}");
        if (_corridor != null && _corridor.AlreadyVisited)
        {
            if (UnityEngine.Random.value <= 0.1f)
            {
                cells[UnityEngine.Random.Range(0, cells.Length)].GetComponent<CellCollider>().hasBattle = true;
            }
        }
        else
        {
            for (int i = 0; i < cells.Length; i++)
            {
                Debug.Log($"{_corridor == null}, {_corridor.CorridorCells[i].hasEvent}, {_corridor.CorridorCells[i].AlreadyVisited}");
                if (_corridor == null || !_corridor.CorridorCells[i].hasEvent ||
                    _corridor.CorridorCells[i].AlreadyVisited) continue;
                switch (_corridor.CorridorCells[i].cellEvent)
                {
                    case EventType.Trap:
                        _cellColliders[i].Init(_corridor.CorridorCells[i]);
                        GameObject trapGameObject =
                            Instantiate(Resources.Load<GameObject>("Prefabs/Trap"), cells[i].transform);
                        Trap trap = trapGameObject.GetComponentInChildren<Trap>();
                        trap.Init(_corridor.CorridorCells[i]);
                        _gameObjectList.Add(trapGameObject);
                        break;
                    case EventType.Treasure:
                        _cellColliders[i].Init(_corridor.CorridorCells[i]);
                        GameObject treasureObj = Instantiate(Resources.Load<GameObject>("Prefabs/TreasureChest"),
                            cells[i].transform);
                        TreasureChest treasureChest = treasureObj.GetComponent<TreasureChest>();
                        treasureChest.Init(_corridor.CorridorCells[i], 2002); //현재 통로쪽 보물이 2002로 고정되어있어서 이렇게 해둠
                        _gameObjectList.Add(treasureObj);
                        break;
                    case EventType.Battle:
                        _cellColliders[i].hasBattle = true;
                        int battleId;
                        if (MapManager.Instance.RoomVisitedCount == 1) battleId = 1004;
                        else battleId = GetEnemyWeight();
                        _cellColliders[i].Init(_corridor.CorridorCells[i], battleId);
                        //해당 CellCollider라는 컴포넌트 안에서 전투 시작 트리거를 켜주세요.
                        break;
                }
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            var input = context.ReadValue<Vector2>();
            // 좌우만 허용
            _currentMovement = new Vector2(input.x, 0f);
        }
        else if (context.canceled)
        {
            _currentMovement = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerCantMove())
        {
            _currentMovement = Vector2.zero;
            CharacterAnimationTrigger();
            return;
        }
        CharacterAnimationTrigger();
        
        Vector2 delta = new Vector2(_currentMovement.x, 0f) * speed * Time.fixedDeltaTime;
        if (this.transform.position.x > 19.8f)
        {
            this.transform.position = new Vector3(19.8f, -0.5f, 0);
        }
        else if (this.transform.position.x < -19.8f)
        {
            this.transform.position = new Vector3(-19.8f, -0.5f, 0);
        }
        else
        {
            _rigidbody2D.MovePosition(_rigidbody2D.position + delta);
        }
    }

    private bool PlayerCantMove()
    {
        return BattleManager.Instance.isBattleStarted || !GameManager.Instance.playerCanMove ||
               GameManager.Instance.playerObjectInteract || !GameManager.Instance.uiPlayerCanMove;
    }

    public void DestroyGameObjects()
    {
        if (_gameObjectList.Count == 0) return;
        foreach (var item in _gameObjectList)
        {
            Destroy(item);
        }

        _gameObjectList.Clear();
    }

    private void CharacterAnimationTrigger()
    {
        CorridorMovement movement = CorridorMovement.Stop;
        
        if (_currentMovement.x == 0)
        {
            movement = CorridorMovement.Stop;
        }
        else if (_currentMovement.x > 0)
        {
            movement = CorridorMovement.Left;
        }
        else if (_currentMovement.x < 0)
        {
            movement = CorridorMovement.Right;
        }
        
        if (_currentCorridorMovement == movement) return;
        _currentCorridorMovement = movement;
        
        switch (movement)
        {
            case CorridorMovement.Stop:
                foreach (var item in GameManager.Instance.playableCharacter)
                {
                    if (item.characterAnimationController == null) return;
                    item.characterAnimationController.SetTrigger("Idle");
                }
                break;
            case CorridorMovement.Right:
                foreach (var item in GameManager.Instance.playableCharacter)
                {
                    if (item.characterAnimationController == null) return;
                    item.characterAnimationController.SetTrigger("Run");
                    item.characterAnimationController.FlipX(false);
                }
                break;
            case CorridorMovement.Left:
                foreach (var item in GameManager.Instance.playableCharacter)
                {
                    if (item.characterAnimationController == null) return;
                    item.characterAnimationController.SetTrigger("Run");
                    item.characterAnimationController.FlipX(true);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private int GetEnemyWeight()
    {
        //var type = DataManager.Instance.Battle.GetRandomDataIDByType(EventType.Battle);
        int groupId = 1; //몬스터 그룹id 더 추가할거면 관련된 로직 작업 더 필요함
        var enemyGroupList = DataManager.Instance.Battle.GetEnemyGroupIdList(groupId);
        List<float> enemyAppearWeight = new List<float>();

        for (int i = 0; i < enemyGroupList.Count; i++)
        {
            enemyAppearWeight.Add(enemyGroupList[i].emergeProb);
        }

        int battleRoomIndex = RandomizeUtility.TryGetRandomPlayerIndexByWeight(enemyAppearWeight);

        int battleRoomId = enemyGroupList[battleRoomIndex].id;

        return battleRoomId;
    }
}