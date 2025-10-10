using DataTable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PMCInfo : BaseEntity
{
    private MercenaryData data;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI roleTypeText;
    public TextMeshProUGUI contractGoldText;

    [SerializeField] private int initID; // 용병 id
    public int InitID => initID;

    public void Start()
    {
        SetData(initID);
    }

    private void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);

        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical, data.gameObjectString, data.roleType
        );

        if (nameText != null)
            nameText.text = data.name;
        if (roleTypeText != null)
            roleTypeText.text = data.roleType.ToString();
        if (contractGoldText != null)
            contractGoldText.text = data.contractGold.ToString();
    }
    public void RefreshCardActive()
    {
        bool hasPlayer = GameManager.Instance.HasPlayerById(initID);
        gameObject.SetActive(!hasPlayer);
    }

    public void OnClickHire()// 고용 버튼 클릭
    {
        PMCHire.Instance.SpawnPMC(initID, data.contractGold);
    }
}

