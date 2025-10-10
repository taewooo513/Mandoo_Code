using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCollider : MonoBehaviour
{
    [SerializeField] private int index;
    public bool hasBattle;
    public bool isBattleOver;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasBattle && !isBattleOver)
        {
            isBattleOver = true;
            Debug.Log("Battle");//여기서 배틀 시작해주세요
        }
    }
    
    //메서드를 따로 만들어서 보상까지 주어야합니다.
}
