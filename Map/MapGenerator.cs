using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MapGenerator
{
    private readonly List<BaseRoom> _rooms = new();
    private readonly Dictionary<(int, int), BaseRoom> _intsRoomDictionary = new();
    private DataTable.MapData _mapData;
    private int _battleRoomCount;
    private int _itemRoomCount;
    private int _roomCount;
    private int _shopRoomCount;
    private int _pmcRoomCount;
    private int _emptyRoomCount;
    private int _additionalPathCount;
    private float _pathProb;


    public List<BaseRoom> GenerateTutorialMap()
    {
        _rooms.Clear();
        _intsRoomDictionary.Clear();
        
        var startRoom = new StartRoom();
        _rooms.Add(startRoom);
        startRoom.RoomLocation = "";
        _intsRoomDictionary.Add((0,0), startRoom);
        
        //---Room1, PMC---//
        var pmcRoom = new PmcRoom();
        RoomDirection direction = RoomDirection.Right;
        _rooms.Add(pmcRoom);
        ConnectRoomForTutorial(startRoom, pmcRoom, direction, 3, TutorialCorridorInit.Trap);
        
        //---Room2, Shop---//
        var shopRoom = new ShopRoom();
        direction = RoomDirection.Right;
        _rooms.Add(shopRoom);
        ConnectRoomForTutorial(pmcRoom, shopRoom, direction, 3, TutorialCorridorInit.Treasure);
        
        //---Room3, Village---//
        var villageRoom = new VillageRoom();
        direction = RoomDirection.Right;
        _rooms.Add(villageRoom);
        ConnectRoomForTutorial(shopRoom, villageRoom, direction, 3, TutorialCorridorInit.Empty);
        villageRoom.Init(1004);
        //배틀 데이터 넣어줘야함.
        return _rooms;
    }
    public List<BaseRoom> GenerateBattleTestMap(List<int> playerIds, List<int> enemieIds)
    {
        _rooms.Clear();
        _intsRoomDictionary.Clear();
        
        var battleRoom = new BattleRoom();
        _rooms.Add(battleRoom);
        battleRoom.BattleTestInit(playerIds,enemieIds);
        battleRoom.RoomLocation = "";
        _intsRoomDictionary.Add((0,0), battleRoom);
        _roomCount--;
        return _rooms;
    }
    public List<BaseRoom> GenerateBattleTestMap(List<int> playerIds, List<int> enemyIds, 
        List<(int, int, int, int, float, float)> playerStatInfo, List<(int, int, int, int, float, float)> enemyStatInfo)
    {
        _rooms.Clear();
        _intsRoomDictionary.Clear();
        
        var battleRoom = new BattleRoom();
        _rooms.Add(battleRoom);
        battleRoom.BattleTestInit(playerIds,enemyIds, playerStatInfo, enemyStatInfo);
        battleRoom.RoomLocation = "";
        _intsRoomDictionary.Add((0,0), battleRoom);
        _roomCount--;
        return _rooms;
    }
    public List<BaseRoom> GenerateMap(DataTable.MapData mapData)
    {
        //-----Initialize-----//
        
        _mapData = mapData;
        _roomCount = _mapData.totalCnt;
        _battleRoomCount = _mapData.battleCnt;
        _itemRoomCount = _mapData.itemCnt;
        _shopRoomCount = _mapData.shopCnt;
        _pmcRoomCount = _mapData.pmcCnt;
        _emptyRoomCount = _mapData.emptyCnt;
        _additionalPathCount = _mapData.additionalPathCnt;
        _pathProb = _mapData.pathProp;
        _rooms.Clear();
        _intsRoomDictionary.Clear();

        //---Initialize End---//

        var recentlyListedRooms = new List<BaseRoom>();

        var startRoom = new StartRoom();
        _rooms.Add(startRoom);
        startRoom.RoomLocation = "";
        _intsRoomDictionary.Add((0,0), startRoom);
        _roomCount--;
        GenerateRoom(startRoom, ref recentlyListedRooms, true);

        while (_roomCount > 1)
        {
            if (recentlyListedRooms.Count == 0)
            {
                Debug.LogError("Room Count Error");
                break;
            }
            List<BaseRoom> tempList = new List<BaseRoom>();
            foreach (var item in recentlyListedRooms)
            {
                if (_roomCount <= 1) break;
                GenerateRoom(item, ref tempList);
            }
            recentlyListedRooms.Clear();
            recentlyListedRooms.AddRange(tempList);
            if (_roomCount <= 1) break;
        }

        var bossRoom = recentlyListedRooms[Random.Range(0, recentlyListedRooms.Count)];
        GenerateBossRoom(bossRoom);
        return _rooms;
    }

    private BaseRoom GetConnectableRoom(BaseRoom room)
    {
        BaseRoom connectableRoom = room;
        while (connectableRoom.RoomLocation != "")
        {
            List<RoomDirection> possibleDirection = GetPossibleDirection(connectableRoom);
            if (possibleDirection.Count != 0) return connectableRoom;
            
            string shortenedLocation = connectableRoom.RoomLocation.Remove(connectableRoom.RoomLocation.Length - 1, 1);
            connectableRoom = FindRoomWithString(shortenedLocation);
        }

        List<BaseRoom> tempList = new List<BaseRoom>();
        foreach (var item in _rooms)
        {
            if (GetPossibleDirection(item).Count != 0) tempList.Add(item);
        }

        return tempList[Random.Range(0, tempList.Count)];
    }

    private BaseRoom FindRoomWithString(string roomLocation)
    {
        var location = GetRoomLocation(roomLocation);
        return FindRoomWithLocation(location);
    }

    private BaseRoom FindRoomWithLocation((int, int) location)
    {
        if (_intsRoomDictionary.TryGetValue(location, out BaseRoom room)) return room;
        return null;
    }

    private void GenerateRoom(BaseRoom parentRoom, ref List<BaseRoom> recentlyListedRooms, bool isFirstRoom = false)
    {
        List<RoomDirection> possibleDirection = GetPossibleDirection(parentRoom);
        if (possibleDirection.Count == 0 && !isFirstRoom)
        {
            parentRoom = GetConnectableRoom(parentRoom);
            possibleDirection = GetPossibleDirection(parentRoom);
        }
        int random = isFirstRoom ? 1 : Random.Range(1, possibleDirection.Count + 1);

        for (int i = 0; i < random; i++)
        {
            if (_roomCount <= 1) break;
            RoomDirection direction = possibleDirection[Random.Range(0, possibleDirection.Count)];
            possibleDirection.Remove(direction);
            List<RoomType> possibleRoomType = GetPossibleRoomType();
            RoomType randomRoomType = isFirstRoom ? RoomType.PMC : possibleRoomType[Random.Range(0, possibleRoomType.Count)];
            BaseRoom room = null;
            switch (randomRoomType)
            {
                case RoomType.Start:
                    Debug.LogError("RoomType Error");
                    break;
                case RoomType.Empty:
                    room = new EmptyRoom();
                    _emptyRoomCount--;
                    break;
                case RoomType.Battle:
                    room = new BattleRoom();
                    int battleId = GetEnemyWeight(); 
                    room.Init(battleId); //기존 : DataManager.Instance.Battle.GetRandomDataIDByType(EventType.Battle)
                    _battleRoomCount--;
                    break;
                case RoomType.Item:
                    room = new TreasureRoom();
                    room.Init(2001); //기존 : DataManager.Instance.Battle.GetRandomDataIDByType(EventType.Treasure) <<보물룸은 2001번밖에 없어서 하드코딩으로 수정
                    _itemRoomCount--;
                    break;
                case RoomType.Shop:
                    room = new ShopRoom();
                    _shopRoomCount--;
                    break;
                case RoomType.PMC:
                    room = new PmcRoom();
                    _pmcRoomCount--;
                    break;
                default:
                    Debug.LogError("RoomType Error");
                    break;
            }

            if (room != null)
            {
                recentlyListedRooms.Add(room);
                _rooms.Add(room);
                ConnectRoom(parentRoom, room, direction);
            }

            _roomCount--;
        }
    }

    private void GenerateBossRoom(BaseRoom parentRoom)
    {
        List<RoomDirection> possibleDirection = GetPossibleDirection(parentRoom);
        if (possibleDirection.Count == 0) return;
        RoomDirection direction = possibleDirection[Random.Range(0, possibleDirection.Count)];

        var villageRoom = new VillageRoom();
        int battleId = DataManager.Instance.Battle.GetRandomDataIDByType(EventType.Boss);
        villageRoom.Init(battleId);
        _rooms.Add(villageRoom);
        ConnectRoom(parentRoom, villageRoom, direction);
    }
    private void ConnectRoom(BaseRoom parentRoom, BaseRoom childRoom, RoomDirection direction, bool isConnectCorridorOnly = false)
    {
        var corridor = parentRoom.MakeConnection(childRoom, direction);
        childRoom.ApplyConnection(parentRoom, GetOppositeDirection(direction), corridor);
        
        if (isConnectCorridorOnly) _additionalPathCount--;
        else
        {
            string location = childRoom.SetRoomLocation(parentRoom, direction);
            var locationInt = GetRoomLocation(location);
            _intsRoomDictionary.Add(locationInt, childRoom);
        }
    }

    private void ConnectRoomForTutorial(BaseRoom parentRoom, BaseRoom childRoom, RoomDirection direction, 
        int index, TutorialCorridorInit init)
    {
        var corridor = parentRoom.MakeConnectionForTutorial(childRoom, direction, index, init);
        childRoom.ApplyConnection(parentRoom, GetOppositeDirection(direction), corridor);
        string location = childRoom.SetRoomLocation(parentRoom, direction);
        var locationInt = GetRoomLocation(location);
        _intsRoomDictionary.Add(locationInt, childRoom);
    }

    private RoomDirection GetOppositeDirection(RoomDirection direction)
    {
        return (RoomDirection)(((int)direction + 2) % 4);
    }

    private List<RoomDirection> GetPossibleDirection(BaseRoom room)
    {
        //TODO: 훨씬 깊게 탐색할 수 있어야함. - 완료?
        List<RoomDirection> possibleDirection = new();

        (int, int) location = GetRoomLocation(room.RoomLocation);
        
        List<(int,int, RoomDirection)> possibleLocationList = new();
        possibleLocationList.Add((-1, 0, RoomDirection.Left));
        possibleLocationList.Add((1, 0, RoomDirection.Right));
        possibleLocationList.Add((0, 1, RoomDirection.Up));
        possibleLocationList.Add((0, -1, RoomDirection.Down));

        foreach (var (dx, dy, dir) in possibleLocationList)
        {
            if(IsLocationPossible((location.Item1 + dx, location.Item2 + dy)))
            {
                possibleDirection.Add(dir);
            }
            else
            {
                BaseRoom locatedRoom = FindRoomWithLocation((location.Item1 + dx, location.Item2 + dy));
                if (locatedRoom != null && !room.IsConnected(locatedRoom, dir) && _additionalPathCount > 0 && CalculateProbability() )
                    ConnectRoom(room, locatedRoom, dir, true);
            }
        }
        
        return possibleDirection;
    }

    private bool CalculateProbability()
    {
        if (_pathProb > Random.value)
        {
            _pathProb = _mapData.pathProp;
            return true;
        }

        _pathProb *= 1.5f;
        return false;
    }
    private List<RoomType> GetPossibleRoomType()
    {
        List<RoomType> possibleRoomType = new();
        if (_battleRoomCount > 0) possibleRoomType.Add(RoomType.Battle);
        if (_itemRoomCount > 0) possibleRoomType.Add(RoomType.Item);
        if (_shopRoomCount > 0) possibleRoomType.Add(RoomType.Shop);
        if (_pmcRoomCount > 0) possibleRoomType.Add(RoomType.PMC);
        if (_emptyRoomCount > 0) possibleRoomType.Add(RoomType.Empty);
        return possibleRoomType;
    }

    private (int, int) GetRoomLocation(string roomLocation)
    {
        (int, int) locationInt = new();
        char[] possibleLocationArray = roomLocation.ToCharArray();

        foreach (var item in possibleLocationArray)
        {
            switch (item)
            {
                case 'L':
                    locationInt.Item1--;
                    break;
                case 'R':
                    locationInt.Item1++;
                    break;
                case 'U':
                    locationInt.Item2++;
                    break;
                case 'D':
                    locationInt.Item2--;
                    break;
            }
        }
        return locationInt;
    }

    private bool IsLocationPossible((int, int) targetLocationInt)
    {
        return !_intsRoomDictionary.ContainsKey(targetLocationInt);
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