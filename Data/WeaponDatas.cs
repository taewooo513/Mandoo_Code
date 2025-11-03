using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataTable;

public class WeaponDatas : WeaponData
{
    public WeaponData GetWeaponData(int idx)
    {
        WeaponDataMap.TryGetValue(idx, out WeaponData val);
        return val;
    }
}