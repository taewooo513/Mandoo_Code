using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PmcNPC : MonoBehaviour
{
    private RangeCheck _rangeCheck;
    public GameObject outline;
    public Button pmcButton;
    private PmcRoom _pmcRoom;
    
    public void Start()
    {
        _rangeCheck = GetComponent<RangeCheck>();
    }

    public void OnClickPMC()
    {
        if (_rangeCheck != null)
        {
            _rangeCheck.Init(this);
            _rangeCheck.uiIsInteract = true;
        }
        UIManager.Instance.OpenUI<InGamePMCUI>();
        if(_pmcRoom != null)
            _pmcRoom.isInteract = true;
    }

    public void NpcInteract(PmcRoom pmcRoom)
    {
        _pmcRoom = pmcRoom;
    }
}
