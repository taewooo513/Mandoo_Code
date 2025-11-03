using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PmcRoom : BaseRoom
{
    private GameObject _pmcNPCPrefab;
    
    public override void EnterRoom()
    {
        base.EnterRoom();
        OnEventEnded();
        if (!isInteract)
        { 
            var ui = UIManager.Instance.OpenUI<InGamePMCUI>();
            ui.CloseUI();
            ui.GetData(GetMercenariesID());
            ui.RefreshCardsOnPanel();
            
            _pmcNPCPrefab = Spawn.PMCNPCCreate();
            PmcNPC _pmcNPC = _pmcNPCPrefab.GetComponent<PmcNPC>(); 
            _pmcNPC.NpcInteract(this);

            Vector3 pos = new Vector3(3f, 0, 0);
            _pmcNPCPrefab.transform.position = pos;
        }
        Tutorials.ShowIfNeeded<PMCHireTutorial>();
    }

    private List<int> GetMercenariesID()
    {
        List<int> mercenariesID = DataManager.Instance.Mercenary.GetMercenaryIdList();
        return mercenariesID;
    }
    public override void ExitRoom()
    {
        base.ExitRoom();
        UIManager.Instance.CloseUI<InGamePMCUI>();
        Spawn.DestroyGameObject(_pmcNPCPrefab);
    }
    public override string GetBackgroundPath()
    {
        return "Sprites/Background/RoomBackground" + Random.Range(0,4);
    }
}
