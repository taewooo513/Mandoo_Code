using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using UnityEngine;

public class Weapon : Item
{
    WeaponType type;
    int proficiencyLevel;
    int attack;
    int def;
    int speed;
    float eva;
    float critcal;
    int skillId;
    int maxXp;

    public override void InitItem(int id)
    {
        base.InitItem(id);
        var weapon = DataManager.Instance.Weapon.GetWeaponData(itemInfo.weaponId);
        attack = weapon.attack;
        def = weapon.defense;
        speed = weapon.speed;
        eva = weapon.evasion;
        critcal = weapon.critical;
        skillId = weapon.skillId;
        proficiencyLevel = weapon.proficiencyLevel;
        type = weapon.weaponType;
    }
}
