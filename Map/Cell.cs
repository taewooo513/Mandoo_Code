using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public bool hasEvent;
    public EventType cellEvent;
    //private Image icon

    public void Init(bool Event = false)
    {
        hasEvent = Event;
        if (hasEvent)
        {
            cellEvent = (EventType)Random.Range(0, 3);
        }
    }

    public void StartEvent()
    {
        if (cellEvent == EventType.Battle)
        {
            
        }
    }

    public void OnLoad()
    {
        
    }
}
