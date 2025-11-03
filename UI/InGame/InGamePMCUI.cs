using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGamePMCUI : UIBase
{
    [SerializeField] private PMCCardManager pmCCardManager;
    private List<int> _ids = new();
    private List<int> _mercenariesID = new();
    [SerializeField] private List<PlayerRemoveButton> firedButtons = new();
    void Start()
    {
        UIManager.Instance.OpenUI<InGameInventoryUI>();
        UIManager.Instance.CloseUI<MapUI>();
        foreach (var button in firedButtons)
        {
            button.Init(this);
        }
    }

    public void GetData(List<int> ids)
    {
        _mercenariesID.Clear();
        _mercenariesID = ids;
    }

    private void SetIDs()
    {
        _ids.Clear();
        foreach (var item in _mercenariesID)
        {
            if (!GameManager.Instance.HasPlayerById(item))
            {
                _ids.Add(item);
            }
        }
    }

    public void RefreshCardsOnPanel()
    {
        SetIDs();
        pmCCardManager.GetData(_ids, this);
        pmCCardManager.RefreshCardsOnPanel();
    }
}