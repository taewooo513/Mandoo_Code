using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUIContentGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roomPrefab;
    [SerializeField] private GameObject corridorPrefab;
    [SerializeField] private Transform elements;
    private Transform _content;
    private List<BaseRoom> _rooms = new();
    private List<(BaseRoom, int, int)> _roomPositions = new();
    private int _xPos = 300;
    private int _yPos = 300;
    private int xMin = 0;
    private int xMax = 0;
    private int yMin = 0;
    private int yMax = 0;
    public void Init(Transform content, List<BaseRoom> rooms)
    {
        _roomPositions.Clear();
        _content = content;
        _rooms = rooms;
        _xPos = 300;
        _yPos = 300;
        xMin = 0;
        xMax = 0;
        yMin = 0;
        yMax = 0;
    }

    public void GenerateMapUI(out List<RoomUI> roomUIList, out List<CorridorUI> corridorUIList)
    {
        roomUIList = new();
        corridorUIList = new();
        FillRoomPositionList();
        foreach (var item in _roomPositions)
        {
            var go = Instantiate(roomPrefab, elements);
            var rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = new Vector2(_xPos * item.Item2, _yPos*item.Item3);
            }
            var roomUI = go.GetComponent<RoomUI>();
            roomUI.Init(item.Item1);
            roomUIList.Add(roomUI);

            var corridors = item.Item1.corridors;
            if (corridors.ContainsKey(RoomDirection.Left) && !corridors[RoomDirection.Left].IsAlreadyMade)
            {
                var go2 = Instantiate(corridorPrefab, elements);
                corridors[RoomDirection.Left].IsAlreadyMade = true;
                var rt2 = go2.GetComponent<RectTransform>();
                if (rt2 != null)
                {
                    rt2.anchoredPosition = new Vector2(_xPos * item.Item2-150, _yPos * item.Item3);
                }

                CorridorUI ui = go2.GetComponent<CorridorUI>();
                ui.RearrangeCells(RoomDirection.Left);
                corridorUIList.Add(ui);
            }

            if (corridors.ContainsKey(RoomDirection.Right) && !corridors[RoomDirection.Right].IsAlreadyMade)
            {
                var go2 = Instantiate(corridorPrefab, elements);
                corridors[RoomDirection.Right].IsAlreadyMade = true;
                var rt2 = go2.GetComponent<RectTransform>();
                if (rt2 != null)
                {
                    rt2.anchoredPosition = new Vector2(_xPos * item.Item2+150, _yPos * item.Item3);
                }
                CorridorUI ui = go2.GetComponent<CorridorUI>();
                ui.RearrangeCells(RoomDirection.Right);
                corridorUIList.Add(ui);
            }

            if (corridors.ContainsKey(RoomDirection.Up) && !corridors[RoomDirection.Up].IsAlreadyMade)
            {
                var go2 = Instantiate(corridorPrefab, elements);
                corridors[RoomDirection.Up].IsAlreadyMade = true;
                var rt2 = go2.GetComponent<RectTransform>();
                if (rt2 != null)
                {
                    rt2.anchoredPosition = new Vector2(_xPos * item.Item2, _yPos * item.Item3+150);
                    rt2.rotation = Quaternion.Euler(0, 0, 90);
                }
                CorridorUI ui = go2.GetComponent<CorridorUI>();
                ui.RearrangeCells(RoomDirection.Up);
                corridorUIList.Add(ui);
            }

            if (corridors.ContainsKey(RoomDirection.Down) && !corridors[RoomDirection.Down].IsAlreadyMade)
            {
                var go2 = Instantiate(corridorPrefab, elements);
                corridors[RoomDirection.Down].IsAlreadyMade = true;
                var rt2 = go2.GetComponent<RectTransform>();
                if (rt2 != null)
                {
                    rt2.anchoredPosition = new Vector2(_xPos * item.Item2, _yPos * item.Item3-150);
                    rt2.rotation = Quaternion.Euler(0, 0, 90);
                }
                CorridorUI ui = go2.GetComponent<CorridorUI>();
                ui.RearrangeCells(RoomDirection.Down);
                corridorUIList.Add(ui);
            }
        }
        var elementRt = elements.GetComponent<RectTransform>();
        elementRt.anchoredPosition = new Vector2((xMax+xMin) * -150, (yMax + yMin) * -150);
        var contentRt = _content.GetComponent<RectTransform>();
        contentRt.sizeDelta = new Vector2((xMax - xMin) * 300 + 200, (yMax - yMin) * 300 + 200);
    }

    private void FillRoomPositionList()
    {
        (int, int) locationInt = new();
        foreach (var item in _rooms)
        {
            locationInt = GetRoomLocation(item);
            _roomPositions.Add((item, locationInt.Item1, locationInt.Item2));
        }
    }
    
    private (int, int) GetRoomLocation(BaseRoom room)
    {
        (int, int) locationInt = new();
        char[] possibleLocationArray = room.RoomLocation.ToCharArray();

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
        xMin = Mathf.Min(xMin, locationInt.Item1);
        xMax = Mathf.Max(xMax, locationInt.Item1);
        yMin = Mathf.Min(yMin, locationInt.Item2);
        yMax = Mathf.Max(yMax, locationInt.Item2);
        return locationInt;
    }
}
