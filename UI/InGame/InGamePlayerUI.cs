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

    public TextMeshProUGUI maxWeaponExpText;
    public TextMeshProUGUI weaponExpText;

    public GameObject[] skillUIObjects;
    public InGameEquipmentUI equipmentUI;
    private string playerIconPath;
    public Image selectedPlayerIcon;
    //[SerializeField] private EquipmentSlotUI[] equipmentSlots;

    EntityInfo entityInfo;
    Skill[] skills;
    PlayableCharacter playableCharacter;

    private int originalSortingIndex;
    private bool isSorted;

    [SerializeField] private SelectSkillButton[] sb;
    [SerializeField] private ChangeIndexButton cib;
    [SerializeField] private SkipTurnButton stb;

    [SerializeField] private GameObject weaponLvl2;
    [SerializeField] private GameObject weaponLvl3;

    private void Awake()
    {
        //equipmentSlots = gameObject.GetComponentsInChildren<EquipmentSlotUI>();
        originalSortingIndex = -1;
        equipmentUI = GetComponent<InGameEquipmentUI>();

        foreach (var item in sb)
        {
            if (item != null)
            {
                item.OnPointerEnterEvent += OnSkillButtonPointerEnter;
                item.OnPointerExitEvent += OnSkillButtonPointerExit;
            }
        }

        cib.OnPointerEnterEvent += OnSkillButtonPointerEnter;
        cib.OnPointerExitEvent += OnSkillButtonPointerExit;
        stb.OnPointerEnterEvent += OnSkillButtonPointerEnter;
        stb.OnPointerExitEvent += OnSkillButtonPointerExit;
    }

    private void OnDestroy()
    {
        foreach (var item in sb)
        {
            if (item != null)
            {
                item.OnPointerEnterEvent -= OnSkillButtonPointerEnter;
                item.OnPointerExitEvent -= OnSkillButtonPointerExit;
            }
        }

        cib.OnPointerEnterEvent -= OnSkillButtonPointerEnter;
        cib.OnPointerExitEvent -= OnSkillButtonPointerExit;
        stb.OnPointerEnterEvent -= OnSkillButtonPointerEnter;
        stb.OnPointerExitEvent -= OnSkillButtonPointerExit;
    }
    private void OnSkillButtonPointerEnter()
    {
        if (!isSorted)
        {
            originalSortingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();
            isSorted = true;
        }
    }

    private void OnSkillButtonPointerExit()
    {
        if (isSorted && originalSortingIndex != -1)
        {
            transform.SetSiblingIndex(originalSortingIndex);
            isSorted = false;
        }
    }

    public void UpdateUI(EntityInfo entityInfo, Skill[] skills, PlayableCharacter playableCharacter)
    {
        this.entityInfo = entityInfo;
        this.skills = skills;
        this.playableCharacter = playableCharacter;
        nameText.text = entityInfo.name;
        jobText.text = entityInfo.role.ToString();
        hpUI.text = entityInfo.maxHp.ToString();
        currentHpUI.text = entityInfo.currentHp.ToString();
        speedText.text = entityInfo.GetTotalBuffStat().speed.ToString();
        attackDmgText.text = entityInfo.GetTotalBuffStat().attackDmg.ToString();
        defText.text = entityInfo.GetTotalBuffStat().defense.ToString();
        equipmentUI.Setting(entityInfo.equips, playableCharacter);
        playerIconPath = entityInfo.iconPath;

        if (entityInfo.equips[0] != null)
        {
            maxWeaponExpText.text = entityInfo.equips[0].maxXp.ToString();
            weaponExpText.text = entityInfo.equips[0].currentXp.ToString();
        }
        else
        {
            maxWeaponExpText.text = "0";
            weaponExpText.text = "0";
        }
        for (int i = 0; i < skills.Length; i++)
        {
            skillUIObjects[i].GetComponent<SelectSkillButton>().SetButton(skills[i]);
        }
        SetImage(playerIconPath);


        if (entityInfo.equips[0] == null)
        {
            weaponLvl2.SetActive(false);
            weaponLvl3.SetActive(false);
        }
        if (entityInfo.equips[0] != null && entityInfo.equips[0].proficiencyLevel == 1)
        {
            weaponLvl2.SetActive(false);
            weaponLvl3.SetActive(false);
        }

        if (entityInfo.equips[0] != null && entityInfo.equips[0].proficiencyLevel == 2)
        {
            weaponLvl2.SetActive(true);
            weaponLvl3.SetActive(false);
        }

        if (entityInfo.equips[0] != null && entityInfo.equips[0].proficiencyLevel == 3)
        {
            weaponLvl2.SetActive(false);
            weaponLvl3.SetActive(true);
        }
    }

    public void UpdateUI()
    {
        if (entityInfo == null)
            return;
        nameText.text = entityInfo.name;
        jobText.text = entityInfo.role.ToString();
        hpUI.text = entityInfo.maxHp.ToString();
        currentHpUI.text = entityInfo.currentHp.ToString();
        speedText.text = entityInfo.GetTotalBuffStat().speed.ToString();
        attackDmgText.text = entityInfo.GetTotalBuffStat().attackDmg.ToString();
        defText.text = entityInfo.GetTotalBuffStat().defense.ToString();
        equipmentUI.Setting(entityInfo.equips, playableCharacter);
        playerIconPath = entityInfo.iconPath;

        if (entityInfo.equips[0] != null)
        {
            maxWeaponExpText.text = entityInfo.equips[0].maxXp.ToString();
            weaponExpText.text = entityInfo.equips[0].currentXp.ToString();
        }
        else
        {
            maxWeaponExpText.text = "";
            weaponExpText.text = "";
        }
        for (int i = 0; i < skills.Length; i++)
        {
            skillUIObjects[i].GetComponent<SelectSkillButton>().SetButton(skills[i]);
        }
        SetImage(playerIconPath);
        if (entityInfo.equips[0] != null)
        {
            if (entityInfo.skills[3] == null)
            {
                entityInfo.skills[3] = new Skill();
            }
            entityInfo.skills[3].Init(entityInfo.equips[0].skillId, playableCharacter);
        }
        if (entityInfo.equips[0] != null && entityInfo.equips[0].proficiencyLevel == 1)
        {
            weaponLvl2.SetActive(false);
            weaponLvl3.SetActive(false);
        }

        if (entityInfo.equips[0] != null && entityInfo.equips[0].proficiencyLevel == 2)
        {
            weaponLvl2.SetActive(true);
            weaponLvl3.SetActive(false);
        }

        if (entityInfo.equips[0] != null && entityInfo.equips[0].proficiencyLevel == 3)
        {
            weaponLvl2.SetActive(false);
            weaponLvl3.SetActive(true);
        }
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

    public void SetImage(string path)
    {
        if (selectedPlayerIcon != null && path != null)
        {
            selectedPlayerIcon.sprite = Resources.Load<Sprite>(path);
        }
    }
}