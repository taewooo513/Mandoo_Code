using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TutorialCorridorInit
{
    Trap,
    Treasure,
    Empty
}
public class Corridor : INavigatable
{
    public List<Cell> CorridorCells = new();
    public BaseRoom RoomA;
    public BaseRoom RoomB;
    public bool IsAlreadyMade;
    public RoomDirection Direction;
    public BaseRoom DestinationRoom;
    public BaseRoom CurrentRoom;
    public bool AlreadyVisited;
    public void Enter(BaseRoom room = null)
    {
        BackgroundManager.Instance.ChangeBackground(this);
        EnterCorridor(room);
    }
    public void EnterCorridor(BaseRoom room)
    {
        AudioManager.Instance.PlayBGM(AudioInfo.Instance.corridorBGM, AudioInfo.Instance.corridorBGMVolume);

        BattleManager.Instance.isCorridorBattle = true;
        
        if (room == RoomA)
        {
            DestinationRoom = RoomA;
            CurrentRoom = RoomB;
        }
        else
        {
            DestinationRoom = RoomB;
            CurrentRoom = RoomA;
        }
        //Test();
    }

    private void Test()
    {
        ExitCorridor();
    }
    public void ExitCorridor()
    {
        AlreadyVisited = true;
        BattleManager.Instance.isCorridorBattle = false;
        if(GameManager.Instance.CurrentMapIndex == 0)
        {
            switch(AnalyticsManager.Instance.Step)
            {
                case 13:
                    AnalyticsManager.Instance.SendEventStep(14);
                    break;
                case 19:
                    AnalyticsManager.Instance.SendEventStep(20);
                    break;
                case 23:
                    AnalyticsManager.Instance.SendEventStep(24);
                    break;
            }    
        }
            
        Travel(DestinationRoom);
    }

    public void ExitCorridorToCurrentRoom()
    {
        Travel(CurrentRoom);
    }
    public void MakeCells()
    {
        for (int i = 0; i < 4; i++)
        {
            Cell cell = new Cell();
            CorridorCells.Add(cell);
        }
        CellInit();
    }

    public void MakeCells(int index, TutorialCorridorInit init)
    {
        for (int i = 0; i < 4; i++)
        {
            Cell cell = new Cell();
            CorridorCells.Add(cell);
        }
        
        CellInit(index, init);
    }

    private void CellInit()
    {
        int eventCount = 0;
        float random = Random.Range(0f, 1f);
        if (random < 0.3f) eventCount = 0;
        else if (random < 0.8f) eventCount = 1;
        else eventCount = 2;
        List<int> tempList = Enumerable.Range(0, 4).ToList();
        List<int> randomEvent = new();
        for (int i = 0; i < eventCount; i++)
        {
            int index = Random.Range(0, tempList.Count);
            randomEvent.Add(tempList[index]);
            tempList.RemoveAt(index);
        }
        
        for (int i = 0; i < CorridorCells.Count; i++)
        {
            CorridorCells[i].Init(randomEvent.Contains(i));
        }
    }

    public void CellInit(int index, TutorialCorridorInit init)
    {
        switch (init)
        {
            case TutorialCorridorInit.Trap:
                CorridorCells[index].Init(EventType.Trap);
                break;
            case TutorialCorridorInit.Treasure:
                CorridorCells[index].Init(EventType.Treasure);
                break;
            case TutorialCorridorInit.Empty:
                break;
        }
    }
    public void Init(BaseRoom roomA, BaseRoom roomB, RoomDirection direction)
    {
        RoomA = roomA;
        RoomB = roomB;
        Direction = direction;
    }

    public void Travel(INavigatable location)
    {
        MapManager.Instance.ChangeCurrentLocation(location);
    }
}
