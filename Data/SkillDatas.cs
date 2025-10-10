using System.Collections;
using System.Collections.Generic;
using DataTable;
using UnityEngine;

public class SkillDatas : SkillData
{
    public SkillData GetSkillData(int idx)
    {
        return SkillDataMap[idx];
    }
}
