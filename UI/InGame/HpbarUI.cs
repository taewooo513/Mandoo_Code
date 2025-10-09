using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpbarUI : MonoBehaviour
{
    private BaseEntity entity;
    private Image hpBar;

    private void Start()
    {
        entity = GetComponentInParent<BaseEntity>();
        hpBar = GetComponent<Image>();
    }
    public void UpdateUI()
    {
        if (hpBar != null || entity != null)
        {
            float val = 1f / entity.entityInfo.maxHp * entity.entityInfo.currentHp;
            if (val < 0f)
            {
                hpBar.fillAmount = 0f;
            }
            else
            {
                hpBar.fillAmount = val;
            }
        }
    }
}
