using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager :Singleton<MapManager>
{
    private MapGenerator _mapGenerator;
    private MapUI _mapUI;
    [SerializeField] private int index;

    public List<BaseRoom> rooms = new();
    public INavigatable CurrentLocation;

    public Corridor CurrentCorridor;
    public int RoomVisitedCount;
    protected override void Awake()
    {
        base.Awake();
        _mapGenerator = GetComponent<MapGenerator>();
        if(_mapGenerator == null) _mapGenerator = gameObject.AddComponent<MapGenerator>();
            
    }

    private void Start()
    {
        
        
    }

    public void Initialize()
    {
        //UIManager.Instance.CloseUI<InGameInventoryUI>();
        rooms.Clear();
        DataManager.Instance.Initialize();
        DataTable.MapData mapData = DataManager.Instance.Map.GetMapData(1);
        rooms = _mapGenerator.GenerateMap(mapData);
        _mapUI = UIManager.Instance.OpenUI<MapUI>();
        _mapUI.Init(rooms);
        _mapUI.GenerateMapUI();
        CurrentLocation = rooms[0];
        rooms[0].EnterRoom();
    }

    private BaseRoom FindDestinationRoom(INavigatable destinationRoom)//통로를 받아와 통로가 가고자 하는 목적지를 반환함.
    {
        if (CurrentLocation is BaseRoom room && destinationRoom is Corridor corridor)
        {
            foreach (var item in room.corridors.Keys)
            {
                if (corridor == room.corridors[item])
                {
                    CurrentCorridor = corridor;
                    return room.connectedRooms[item];
                }
            }
        }
        return null;
    }
    public void ChangeCurrentLocation(INavigatable destination)
    {
        if (CurrentLocation is BaseRoom)
        {
            BaseRoom destinationRoom = FindDestinationRoom(destination);
            CurrentLocation = destination;
            DeActivateButtonUI();
            CurrentLocation.Enter(destinationRoom);//TODO: 이 매개변수가 아예 필요 없어질 수도 있지 않을까?, 이를테면 CurrentLocation을 나중에 바꾼다고 하면 Corridor에서 검사할 수 있을 것.
        }
        else if(CurrentLocation is Corridor)
        {
            CurrentLocation = destination;
            DeActivateButtonUI();
            CurrentLocation.Enter();
        }
    }

    public void DeActivateButtonUI()
    {
        foreach (var item in rooms)
        {
            item.RoomUI.DeactivateButton();
        }
    }
}
 