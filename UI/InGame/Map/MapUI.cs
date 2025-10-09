using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : UIBase
{
    private MapUIContentGenerator _mapUIContentGenerator;
    private List<BaseRoom> _rooms;
    [SerializeField] private Transform content;
    private List<RoomUI> _roomUIs = new();
    private List<CorridorUI> _corridorUIs = new();
    private void Awake()
    {
        _mapUIContentGenerator = gameObject.GetComponentInChildren<MapUIContentGenerator>();
    }

    private void Start()
    {
        
    }

    public void Init(List<BaseRoom> rooms)
    {
        _rooms = rooms;
    }

    public void GenerateMapUI()
    {
        ClearMapUI();
        _mapUIContentGenerator.Init(content, _rooms);
        _mapUIContentGenerator.GenerateMapUI(out _roomUIs, out _corridorUIs);
    }

    private void ClearMapUI()
    {
        foreach (var item in _roomUIs)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in _corridorUIs)
        {
            Destroy(item.gameObject);
        }
        _roomUIs.Clear();
        _corridorUIs.Clear();
    }
}
