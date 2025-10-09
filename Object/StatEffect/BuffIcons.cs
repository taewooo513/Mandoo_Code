using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuffIcons : MonoBehaviour
{
    BuffIcon[] buffIcons;

    public void Awake()
    {
        buffIcons = GetComponentsInChildren<BuffIcon>();
    }

    public void UpdateIcon(Buff buff)
    {
        int i = 0;

        if (buff != null)
        {
            for (int j = buff._entityCurrentStatus.Count; j < buffIcons.Length; j++)
            {
                buffIcons[j].gameObject.SetActive(false);
            }
            foreach (var item in buff._entityCurrentStatus)
            {
                if (buffIcons.Length <= i)
                    break;
                buffIcons[i].gameObject.SetActive(true);
                buffIcons[i].UpdateUI(item);
                i++;
            }
        }
    }
}
