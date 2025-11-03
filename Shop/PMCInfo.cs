using DataTable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PMCInfo : MonoBehaviour
{
    private MercenaryData _data;
    private int _initID;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI roleTypeText;
    [SerializeField] private TextMeshProUGUI contractGoldText;
    private InGamePMCUI _inGamePmcui;
    private string playerIconPath;
    public Image selectedPlayerIcon;
    
    public void Init(MercenaryData data, InGamePMCUI inGamePmcui)
    {
        _data = data;
        _inGamePmcui = inGamePmcui;
        SetData();
    }
    private void SetData()
    {
        _initID = _data.id;

        if (nameText != null)
            nameText.text = _data.name;
        if (roleTypeText != null)
            roleTypeText.text = _data.roleType.ToString();
        if (contractGoldText != null)
            contractGoldText.text = _data.contractGold.ToString() + "G";
        playerIconPath = _data.gameObjectString;
        SetImage(playerIconPath);
    }

    public void OnClickHire()// 고용 버튼 클릭
    {
        if(PMCHire.Instance.SpawnPMC(_initID, _data.contractGold)) _inGamePmcui.RefreshCardsOnPanel();
    }
    
    public void SetImage(string path)
    {
        if (selectedPlayerIcon != null && path != null)
        {
            selectedPlayerIcon.sprite = Resources.Load<Sprite>(path);
        }
    }
}

