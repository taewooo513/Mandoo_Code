using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCrashCheck : MonoBehaviour
{
    public Trap trap;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        trap.TrapActive(GameManager.Instance.PlayableCharacter[0]); //제일 앞에 있는 플레이어 넘겨서 트랩 활성화
    }
}
