using System.Collections;
using System.Collections.Generic;
using DataTable;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TreasureRoom : BaseRoom
{
    private GameObject _treasureChestPrefab; //상자 프리팹

    public override void Init(int id)
    {
        base.Init(id);
    }

    public override void EnterRoom()
    {
        base.EnterRoom(); //플레이어 소환(위치 선정)
        OnEventEnded(); //상자를 열어도 되고, 안 열어도 되니 바로 버튼 활성화되도록 함
        GameManager.Instance.playerObjectInteract = false; //혹시몰라서 움직임 가능하게끔 함
        
        if (!isInteract) //상호작용을 안 했을 시
        {
            _treasureChestPrefab = Spawn.TresureChestCreate(); //상자 생성
            var treasureChest = _treasureChestPrefab.GetComponentInChildren<TreasureChest>(); //상자의 cs 가져옴
            treasureChest.Init(this, roomId);

            Vector3 pos = new Vector3(3.5f, 0, 0);
            _treasureChestPrefab.transform.position = pos; //상자 생성해줌
        }
    }

    public override void ExitRoom() //나갈 때
    {
        base.ExitRoom();
        Spawn.DestroyGameObject(_treasureChestPrefab); //상자 파괴
    }
    
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}
