using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class CorridorBackground : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    [SerializeField] private GameObject[] Cells;
    [SerializeField] private float speed;
    private Vector2 _currentMovement;
    private Corridor _corridor;
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    public void Init()
    {
        if (MapManager.Instance.CurrentLocation is Corridor corridor)
        {
            _corridor = corridor;
        }

        foreach (var item in Cells)
        {
            var cellCollider = item.GetComponent<CellCollider>();
            cellCollider.hasBattle = false; 
            cellCollider.isBattleOver = false;
        }
        
        if (_corridor != null && _corridor.AlreadyVisited)
        {
            if (UnityEngine.Random.value <= 0.1f)
            {
                Cells[UnityEngine.Random.Range(0, Cells.Length)].GetComponent<CellCollider>().hasBattle = true;
            }
        }

        else
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (!_corridor.CorridorCells[i].hasEvent) continue;
                switch (_corridor.CorridorCells[i].cellEvent)
                {
                    case EventType.Trap:
                        //GameObject trap = Instantiate(Resources.Load<GameObject>("Prefabs/Trap"),Cells[i].transform);
                        //private trap.GetComponent<Trap>();
                        
                        break;
                    case EventType.Treasure:
                        //GameObject treasure = Instantiate(Resources.Load<GameObject>("Prefabs/TreasureChest"),Cells[i].transform);
                        //private treasure.GetComponent<TreasureChest>();
                        break;
                    case EventType.Battle:
                        //Cells[i].GetComponent<CellCollider>().hasBattle = true;
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
        if (BattleManager.Instance.IsBattleStarted) return;
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
    
}
