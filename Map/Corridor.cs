using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Corridor : INavigatable
{
    public List<Cell> CorridorCells = new();
    public BaseRoom RoomA;
    public BaseRoom RoomB;
    public bool IsAlreadyMade;
    public RoomDirection Direction;
    public BaseRoom DestinationRoom;
    public bool AlreadyVisited;
    public void Enter(BaseRoom room = null)
    {
        BackgroundManager.Instance.ChangeBackground(this);
        EnterCorridor(room);
    }
    public void EnterCorridor(BaseRoom room)
    {
        if (room == RoomA)
        {
            DestinationRoom = RoomA;
        }
        else
        {
            DestinationRoom = RoomB;
        }
        //Test();
    }

    private void Test()
    {
        ExitCorridor();
    }
    public void ExitCorridor()
    {
        Travel(DestinationRoom);
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
