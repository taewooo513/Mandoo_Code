using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using UnityEngine;

public class Weapon : InGameItem
{
    public WeaponType type;
    int id = 0;
    public int proficiencyLevel;
    public int attack;
    public int def;
    public int speed;
    public float eva;
    public float critcal;
    public int skillId;
    public int maxXp;
    public int currentXp;
    public override void InitItem(int id)
    {
        base.InitItem(id);
        this.id = id;
        var weapon = DataManager.Instance.Weapon.GetWeaponData(id);
        attack = weapon.attack;
        def = weapon.defense;
        speed = weapon.speed;
        eva = weapon.evasion;
        critcal = weapon.critical;
        skillId = weapon.skillId;
        proficiencyLevel = weapon.proficiencyLevel;
        maxXp = weapon.maxExp;
        type = weapon.weaponType;
    }

    public override void UseItem(int amount)
    {

    }
    public void AddWeaponExp(int xp)
    {
        currentXp += xp;
        if (currentXp >= maxXp)
        {
            currentXp = maxXp;
            LevelUpWeapon();
        }
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI();
    }

    private void LevelUpWeapon()
    {
        var temp = DataManager.Instance.Weapon.GetWeaponData(id + 1);
        if (temp != null)
        {
            InitItem(id + 1);
            currentXp = 0;
            UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI();
        }
    }

    public void Downgrad()
    {
        if (proficiencyLevel == 3)
        {
            InitItem(id - 1);
        }
    }
}
