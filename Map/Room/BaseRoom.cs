using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class BaseRoom : INavigatable
{
    public Dictionary<RoomDirection, BaseRoom> connectedRooms = new();
    public Dictionary<RoomDirection, Corridor> corridors = new();
    public Dictionary<int, GameObject> playableCharacterDic; //int에 키값, 게임오브젝트에 대응하는 프리팹
    public string RoomLocation;//시작 지점으로부터의 방향을 뜻함
    protected int roomId;
    public bool isInteract = false;
    public RoomUI RoomUI;
    private bool IsAlreadyVisited = false;
    
    public virtual void EnterRoom()
    {
        if (!IsAlreadyVisited)
        {
            MapManager.Instance.RoomVisitedCount++;
            IsAlreadyVisited = true;
            GameDatas gameData = GameManager.Instance.gameDatas;
            if (gameData != null)
            {
                var gameDataExplorerExp = gameData.GetGameData(GameManager.Instance.CurrentMapIndex).explorerExp;
                foreach (var item in GameManager.Instance.playableCharacter)
                {
                    if (item.entityInfo.equips[0] != null)
                        item.entityInfo.equips[0].AddWeaponExp(gameDataExplorerExp);
                }
            }
        }
        BackgroundManager.Instance.ChangeBackground(this);
        Spawn.PlayableCharacterSpawn(); //플레이어 소환(위치 선정)
        RoomUI.ActivateIcon();
    }

    public void Enter(BaseRoom room = null)
    {
        EnterRoom();

    }
    public string SetRoomLocation(BaseRoom parentRoom, RoomDirection direction)
    {
        //???이런 것도 된다고?
        RoomLocation = parentRoom.RoomLocation;
        switch (direction)
        {
            case RoomDirection.Up:
                RoomLocation += "U";
                break;
            case RoomDirection.Down:
                RoomLocation += "D";
                break;
            case RoomDirection.Left:
                RoomLocation += "L";
                break;
            case RoomDirection.Right:
                RoomLocation += "R";
                break;
        }
        return RoomLocation;
    }
    public virtual void ExitRoom()
    {
        UIManager.Instance.GetUI<InGameUIManager>().OnHighlightMap(false);
    }

    public virtual void OnEventEnded()
    {
        UIManager.Instance.GetUI<InGameUIManager>().OnHighlightMap(true);

        foreach (var item in connectedRooms.Values)
        {
            item.RoomUI.ActivateButton();
        }
    }
    public virtual void Init(int id) //전투 보상 등등 기본값 세팅
    {
        roomId = id;
    }

    public Corridor MakeConnection(BaseRoom room, RoomDirection direction)
    {
        connectedRooms.Add(direction, room);
        var corridor = new Corridor();
        corridor.Init(this, room, direction);
        corridors.Add(direction, corridor);
        corridor.MakeCells();

        return corridor;
    }

    public Corridor MakeConnectionForTutorial(BaseRoom room, RoomDirection direction, int index, TutorialCorridorInit init)
    {
        connectedRooms.Add(direction, room);
        var corridor = new Corridor();
        corridor.Init(this, room, direction);
        corridors.Add(direction, corridor);
        corridor.MakeCells(index, init);
        
        return corridor;
    }

    public void ApplyConnection(BaseRoom room, RoomDirection direction, Corridor corridor)
    {
        connectedRooms.Add(direction, room);
        corridors.Add(direction, corridor);
    }

    public bool IsConnected(BaseRoom room, RoomDirection direction)
    {
        if (connectedRooms.TryGetValue(direction, out BaseRoom connectedRoom))
        {
            return connectedRoom == room;
        }

        return false;
    }
    
    public void SetRoomUI(RoomUI roomUI)
    {
        RoomUI = roomUI;
    }

    public void Travel(INavigatable room)
    {
        ExitRoom();
        foreach (var item in connectedRooms.Keys)
        {
            if (connectedRooms[item] == room)
            {
                MapManager.Instance.ChangeCurrentLocation(corridors[item]);
                break;
            }
        }
    }
    public virtual string GetBackgroundPath()
    {
        return "";
    }
}
