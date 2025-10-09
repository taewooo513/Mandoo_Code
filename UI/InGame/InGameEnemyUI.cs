using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InGameEnemyUI : UIBase
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI maxHpText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI speedText;

    public void UpdateUI(EntityInfo entityInfo)
    {
        nameText.text = entityInfo.name;
        hpText.text = entityInfo.currentHp.ToString();
        maxHpText.text = entityInfo.maxHp.ToString();
        attackText.text = entityInfo.GetTotalBuffStat().attackDmg.ToString();
        defenseText.text = entityInfo.GetTotalBuffStat().defense.ToString();
        speedText.text = entityInfo.GetTotalBuffStat().speed.ToString();
    }
}
