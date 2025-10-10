using DataTable;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InGamePlayerUI : UIBase
{
    public bool isTestMode = true;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI jobText;

    public TextMeshProUGUI hpUI;
    public TextMeshProUGUI currentHpUI;

    public TextMeshProUGUI attackDmgText;
    public TextMeshProUGUI defText;
    public TextMeshProUGUI speedText;


    public GameObject[] skillUIObjects;

    //[SerializeField] private EquipmentSlotUI[] equipmentSlots;

    private void Awake()
    {
        //equipmentSlots = gameObject.GetComponentsInChildren<EquipmentSlotUI>();
    }

    public void UpdateUI(EntityInfo entityInfo, Skill[] skills)
    {
        nameText.text = entityInfo.name;
        jobText.text = entityInfo.role.ToString();
        hpUI.text = entityInfo.maxHp.ToString();
        currentHpUI.text = entityInfo.currentHp.ToString();
        speedText.text = entityInfo.GetTotalBuffStat().speed.ToString();
        attackDmgText.text = entityInfo.GetTotalBuffStat().attackDmg.ToString();
        defText.text = entityInfo.GetTotalBuffStat().defense.ToString();

        for (int i = 0; i < skills.Length; i++)
        {
            skillUIObjects[i].GetComponent<SelectSkillButton>().SetButton(skills[i]);
        }
        //if (entityInfo.equipWeapon != null)
        //{
        //    skillUIObjects[3].GetComponent<SelectSkillButton>().SetButton(entityInfo.equipWeapon.skill);
        //}


        //for (int i = 0; i < equipmentSlots.Length; i++)
        //{
        //    equipmentSlots[i].UpdateUI(entityInfo);
        //}

    }

    public void RefeshSlots()
    {
        //if (equipmentSlots == null) return;

        //for (int i = 0; i < equipmentSlots.Length; i++)
        //{
        //    if (equipmentSlots[i] != null)
        //        equipmentSlots[i].RefreshIcon();
        //}
    }
}